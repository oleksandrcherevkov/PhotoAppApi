using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Users.JWT
{
    public class JWTOptions
    {
        public const string Position = "AuthOptions";
        public string ISSUER { get; set; }
        public string AUDIENCE { get; set; }
        public string KEY { get; set; }
        public double LIFETIME { get; set; }
    }
}
