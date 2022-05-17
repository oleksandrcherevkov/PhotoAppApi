using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PhotoAppApi.DAL.Generic;
using PhotoAppApi.EF;
using PhotoAppApi.EF.Models;
using PhotoAppApi.EF.Models.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Photos
{
    public class PhotoService<T> : ServiceErrors where T : PhotoBase
    {
        private readonly IGenericRepository<T> _repo;

        public PhotoService(IGenericRepository<T> repo)
        {
            _repo = repo;
        }

        public async Task UploadAsync(IPhotoAddWrap<T> addDto)
        {
            string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };

            var ext = Path.GetExtension(addDto.File.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                AddError($"The file of type [.{ext}] is not exeptable.", nameof(addDto.File));
                return;
            }

            using (var memoryStream = new MemoryStream())
            {
                await addDto.File.CopyToAsync(memoryStream);

                if (memoryStream.Length < addDto.MaxLength)
                {
                    T photo = addDto.MapToModel(memoryStream.ToArray());

                    try
                    {
                        await _repo.CreateAsync(photo);
                    }
                    catch (Exception)
                    {
                        AddError($"Error occured during adding photo [{addDto.File.FileName}] to post.", nameof(addDto.File));
                    }
                    
                }
                else
                    AddError($"The file [{addDto.File.FileName}] is too large.", nameof(addDto.File));
            }
        }

        public async Task<T> ReadAsync(int id)
        {
            //T photo = await _dbContext.Set<T>
            //        .AsNoTracking()
            //        .FirstOrDefaultAsync(p => p.Id == readOptions.Id);
            //IQueryable<T> photoQuery;
            //switch (readOptions.PhotoType)
            //{
            //    case (PhotoType.Avatar):
            //        photoQuery = (IQueryable<T>)_dbContext.Avatars.AsQueryable();
            //        break;
            //    case (PhotoType.PostPhoto):
            //        photoQuery = (IQueryable<T>)_dbContext.PostPhotos.AsQueryable();
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException(nameof(readOptions.PhotoType));
            //}

            T photo = await _repo.FindByIdAsync(id);

            //T photo = await photoQuery
            //    .AsNoTracking()
            //    .Where(p => p.Id == readOptions.Id)
            //    .FirstOrDefaultAsync();

            if (photo == null)
            {
                AddError($"Photo with ID [{id}] found.", nameof(id));
            }

            return photo;
        }

        public async Task DeleteAsync(int id)
        {
            if (id > 0)
            {
                try
                {
                    await _repo.RemoveByIdAsync(id);
                }
                catch (Exception)
                {
                    AddError($"Error occured during deleting photo with ID [{id}].", nameof(id));
                }
            }
            else
            {
                AddError($"Invalid photo ID [{id}].", nameof(id));
            }
        }
    }
}
