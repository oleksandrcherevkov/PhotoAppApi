using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Photos
{
    public interface IPhotoAddWrap<TModel>
    {
        public int MaxLength { get; }
        public IFormFile File { get; }
        public TModel MapToModel(byte[] data, byte[] compressedData);
    }
}
