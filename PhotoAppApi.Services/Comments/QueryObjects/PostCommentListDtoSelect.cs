using PhotoAppApi.EF.Models;
using PhotoAppApi.Services.Comments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Comments.QueryObjects
{
    public static class PostCommentListDtoSelect
    {
        public static IQueryable<PostCommentListDto> SelectListDto(this IQueryable<PostComment> query, string host)
        {
            return query.Select(c => new PostCommentListDto()
            {
                Id = c.Id,
                CreatorAvatar = c.Creator.Avatar == null ? "" : $"https://{host}/api/photo/avatar/{c.Creator.Avatar.Id}/{c.Creator.Avatar.Name}/compressed",
                CreatorLogin = c.CreatorLogin,
                Text = c.Text,
                CreationTime = c.CreationTime
            });
        }
    }
}
