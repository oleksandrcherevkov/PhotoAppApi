using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PhotoAppApi.EF.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Users.JWT
{
    public class JWTConstructor
    {
        private readonly JWTOptions _jwtOptions;
        public JWTOptions JWTOptions { get { return _jwtOptions; } }

        public JWTConstructor(IOptions<JWTOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string CreateJWTAsync(string login, string role)
        {
            var identity = GetIdentityAsync(login, role);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: _jwtOptions.ISSUER,
                    audience: _jwtOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromDays(_jwtOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(_jwtOptions.KEY), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
        private ClaimsIdentity GetIdentityAsync(string login, string role)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
