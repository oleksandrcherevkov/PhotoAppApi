using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.EF.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public PostPhoto Photo { get; set; }
#nullable enable
        public string? Description { get; set; }
#nullable disable
        public DateTime CreationTime { get; set; }

        public string CreatorLogin { get; set; }
        public User Creator { get; set; }
        public int LikesCount { get; set; }

        public ICollection<User> Liked { get; set; }
        public ICollection<PostComment> Comments  { get; set; }
        public ICollection<PostUserView> Views { get; set; }
    }
}
