using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Comments.Models
{
    public class PostCommentListDto
    {
        public int Id { get; set; }
        public string CreatorLogin { get; set; }
        public string CreatorAvatar { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime  { get; set; }
    }
}
