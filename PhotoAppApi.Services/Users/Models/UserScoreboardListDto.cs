using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Users.Models
{
    public class UserScoreboardListDto : UserListDto
    {
        public int LikesTotal { get; set; }
        public int LikesInPeriod { get; set; }
        public int PostsTotal { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
