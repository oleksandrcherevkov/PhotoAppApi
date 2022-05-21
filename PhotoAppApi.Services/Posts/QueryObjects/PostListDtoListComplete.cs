using Microsoft.EntityFrameworkCore;
using PhotoAppApi.EF.Models;
using PhotoAppApi.Services.GeneralOptions;
using PhotoAppApi.Services.Posts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Posts.QueryObjects
{
    public static class PostListDtoListComplete
    {
        public static async Task<PostsPageDto> ReadPostsPagedAsync(this IQueryable<Post> query, PostsListOptions listOptions, string login, string host, bool compressedPhoto = false)
        {
            var newQuery = query
                          .AsNoTracking()
                          .OrderByDescending(p => p.CreationTime)
                          .SelectListDto(login, host, compressedPhoto);

            listOptions.CheckPaging(query);

            var page = new PostsPageDto()
            { 
                Posts = await newQuery.Page(listOptions.PageNum, listOptions.PageSize).ToListAsync(),
                CurrentPage = listOptions.PageNum,
                TotalPages = listOptions.NumPages
            };

            return page;
        }
    } 
}
