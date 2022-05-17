using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Posts.Models
{
    public class PostListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string CreatorLogin { get; set; }
        public string CreatorAvatar { get; set; }
        public int LikesCount { get; set; }
        public int ViewsCount { get; set; }
        public int CommentsCount { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsLikedByUser { get; set; }
    }
}
