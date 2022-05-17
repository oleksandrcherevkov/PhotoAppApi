using Microsoft.AspNetCore.Http;
using PhotoAppApi.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Photos.PostPhotos
{
    public class PostPhotoAddWrap : IPhotoAddWrap<PostPhoto>
    {
        public int MaxLength { get; }

        public IFormFile File { get; }

        private int PostId { get; }

        public PostPhotoAddWrap(IFormFile file, int maxLength, int postId)
        {
            File = file;
            MaxLength = maxLength;
            PostId = postId;
        }

        public PostPhoto MapToModel(byte[] data)
        {
            return new PostPhoto()
            {
                PostId = PostId,
                Name = File.FileName,
                Data = data
            };
        }
    }
}
