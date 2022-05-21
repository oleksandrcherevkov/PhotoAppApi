using PhotoAppApi.EF.Models;
using PhotoAppApi.Services.Posts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PhotoAppApi.Services.Posts.QueryObjects
{
    public static class PostListDtoSelect
    {
        public static IQueryable<PostListDto> SelectListDto(this IQueryable<Post> query, string currentUserLogin, string host, bool compressedPhoto = false)
        {
            return query.Select(p => new PostListDto()
            {
                Id = p.Id,
                CreatorLogin = p.CreatorLogin,
                CreatorAvatar = p.Creator.Avatar == null ? "" : $"https://{host}/api/photo/avatar/{p.Creator.Avatar.Id}/{p.Creator.Avatar.Name}/compressed",
                Title = p.Title,
                Description = p.Description ?? String.Empty,
                Photo = $"https://{host}/api/photo/post/{p.Photo.Id}/{p.Photo.Name}" + (compressedPhoto ? "/compressed" : ""),
                LikesCount = p.LikesCount,
                ViewsCount = p.Views.Count(),
                CommentsCount = p.Comments.Count(),
                CreationTime = p.CreationTime,
                IsLikedByUser = p.Liked.Where(u => u.Login == currentUserLogin).Any()
            });
        }
    }
}
