using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.EF.Models
{
    public class PostComment
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public string CreatorLogin { get; set; }
        public int PostId { get; set; }
        public DateTime CreationTime { get; set; }

        public User Creator { get; set; }
    }
}
