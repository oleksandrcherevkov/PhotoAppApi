using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Comments.Models
{
    public class PostCommentAddDto
    {
        [Required]
        [StringLength(1000)]
        public string Text { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must equal or be greater than {1}.")]
        public int PostId { get; set; }
    }
}
