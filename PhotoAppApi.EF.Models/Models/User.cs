using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.EF.Models
{
    public class User
    {
        [Key]
        public string Login { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Confirmed { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Role { get; set; }

        public Avatar Avatar { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Post> Likes { get; set; }
    }
}
