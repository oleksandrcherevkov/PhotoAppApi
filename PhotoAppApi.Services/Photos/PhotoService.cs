using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
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
    public class PhotoService<T> : ServiceErrors where T : PhotoBase, new()
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

            using (var originalPhotoStream = new MemoryStream())
            {
                using (var compressedPhotoStream = new MemoryStream())
                {
                    using (var photoStream = File.Create(Path.GetFullPath("Photo.webp")))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                        {
                            imageFactory.Load(addDto.File.OpenReadStream())
                                        .Format(new WebPFormat())
                                        .Quality(5)
                                        .Save(compressedPhotoStream);
                        }
                    }
                    addDto.File.CopyTo(originalPhotoStream);
                    if (originalPhotoStream.Length < addDto.MaxLength)
                    {
                        T photo = addDto.MapToModel(originalPhotoStream.ToArray(), compressedPhotoStream.ToArray());

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
        }

        private void Save(MemoryStream compressedPhotoStream)
        {
            throw new NotImplementedException();
        }

        public async Task<T> ReadAsync(int id, bool compressed = false)
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

            T photo = await _repo
                .Read(p => p.Id == id)
                .Select(p => new T()
                {
                    Id = p.Id,
                    Data = compressed ? null : p.Data,
                    CompressedData = compressed ? p.CompressedData : null,
                    Name = p.Name
                })
                .FirstOrDefaultAsync();

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
