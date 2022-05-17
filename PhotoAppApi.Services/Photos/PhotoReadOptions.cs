using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Photos
{
    public enum PhotoType
    {
        Avatar,
        PostPhoto
    }
    public class PhotoReadOptions
    {
        public int Id { get;}
        public PhotoType PhotoType { get; }
        public PhotoReadOptions(int id, PhotoType photoType)
        {
            Id = id;
            PhotoType = photoType;
        }
    }
}
