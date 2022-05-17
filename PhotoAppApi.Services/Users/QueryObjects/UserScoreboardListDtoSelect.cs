using PhotoAppApi.EF.Models;
using PhotoAppApi.Services.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Users.QueryObjects
{
    public static class UserScoreboardListDtoSelect
    {
        public static IQueryable<UserScoreboardListDto> SelectScoreboardDto(this IQueryable<User> query, string host, DateTime now, TimeSpan searchingTime)
        {
            return query.Select(u => new UserScoreboardListDto()
            {
                Login = u.Login,
                Role = u.Role,
                Avatar = u.Avatar == null ? "" : $"https://{host}/api/photo/avatar/{u.Avatar.Id}/{u.Avatar.Name}",
                PostsTotal = u.Posts.Count(),
                LikesInPeriod = u.Posts.Select(p => p.LikesCount).Sum(),
                LikesTotal = u.Posts.Where(p => p.CreationTime > now.Subtract(searchingTime))
                                        .Select(p => p.LikesCount).Sum(),
                RegistrationDate = u.RegistrationDate
            });
        }
    }
}
