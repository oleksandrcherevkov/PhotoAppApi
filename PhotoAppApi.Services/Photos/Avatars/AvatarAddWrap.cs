using Microsoft.AspNetCore.Http;
using PhotoAppApi.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Photos.Avatars
{
    public class AvatarAddWrap : IPhotoAddWrap<Avatar>
    {
        public int MaxLength { get; }

        public IFormFile File { get; }

        private string UserLogin { get; }

        public AvatarAddWrap(IFormFile file, int maxLength, string userLogin)
        {
            File = file;
            MaxLength = maxLength;
            UserLogin = userLogin;
        }

        public Avatar MapToModel(byte[] data, byte[] compressedData)
        {
            return new Avatar()
            {
                Name = File.FileName,
                UserLogin = UserLogin,
                Data = data,
                CompressedData = compressedData
            };
        }
    }
}
