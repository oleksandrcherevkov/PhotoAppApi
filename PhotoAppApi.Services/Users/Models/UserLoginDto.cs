using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Users.Models
{
    public class UserLoginDto
    {
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Login { get; set; }
        [Required]
        [StringLength(24, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
