using PhotoAppApi.EF.Models;
using PhotoAppApi.Services.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Users.QueryObjects
{
    public static class UserListDtoSelect
    {
        public static IQueryable<UserListDto> SelectListDto(this IQueryable<User> query, string host)
        {
            return query.Select(u => new UserListDto()
            {
                Login = u.Login,
                Role = u.Role,
                Avatar = u.Avatar == null ? "" : $"https://{host}/api/photo/avatar/{u.Avatar.Id}/{u.Avatar.Name}",
            });

        }
    }
}
