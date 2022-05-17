using PhotoAppApi.EF.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.EF.Models
{
    public class Avatar : PhotoBase
    {
        public string UserLogin { get; set; }
    }
}
