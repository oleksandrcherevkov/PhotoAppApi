using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Posts.Models
{
    public class PostsPageDto
    {
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public List<PostListDto> Posts { get; set; }
    } 
}
