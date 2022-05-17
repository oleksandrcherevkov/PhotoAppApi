using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Users.Models
{
    public class UserListDto
    {
        public string Login { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
    }
}
