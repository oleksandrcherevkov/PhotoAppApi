using PhotoAppApi.EF.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.EF.Models
{
    public class PostPhoto : PhotoBase
    {
        
        public int PostId { get; set; }
    }
}
