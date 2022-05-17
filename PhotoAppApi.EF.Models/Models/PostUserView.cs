using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.EF.Models
{
    public class PostUserView
    {
        public string UserLogin { get; set; }
        public int PostId { get; set; }
    }
}
