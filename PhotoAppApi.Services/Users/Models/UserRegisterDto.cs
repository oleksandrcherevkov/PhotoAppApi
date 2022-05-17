using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Users.Models
{
    public class UserRegisterDto : UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
