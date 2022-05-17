using Microsoft.AspNetCore.Http;
using PhotoAppApi.DAL.Generic;
using PhotoAppApi.EF.Models;
using PhotoAppApi.Services.Comments.Models;
using PhotoAppApi.Services.Comments.QueryObjects;
using PhotoAppApi.Services.GeneralOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.Comments
{
    public class PostCommentService : ServiceErrors
    {
        private readonly string _hostName;
        private readonly IGenericRepository<PostComment> _repo;

        public PostCommentService(IHttpContextAccessor httpContextAccessor, IGenericRepository<PostComment> repo)
        {
            _repo = repo;
            _hostName = httpContextAccessor.HttpContext.Request.Host.Host;
        }

        public async Task<int> AddAsync(PostCommentAddDto commentInfo, string currentUserLogin)
        {
            var comment = new PostComment()
            {
                CreatorLogin = currentUserLogin,
                PostId = commentInfo.PostId,
                Text = commentInfo.Text,
                CreationTime = DateTime.UtcNow
            };

            try
            {
                await _repo.CreateAsync(comment);
            }
            catch (Exception)
            {
                AddError($"Error occured during adding comment to post with ID [{commentInfo.PostId}].", nameof(commentInfo.PostId));
            }
            

            return comment.Id;
        }

        public IQueryable<PostCommentListDto> ListComments(int postId)
        {
            var comments = _repo.Read(c => c.PostId == postId)
                .OrderByDescending(c => c.CreationTime)
                .SelectListDto(_hostName);

            return comments;
        }

        public async Task DeleteAsync(int id, string currentUserLogin)
        {
            PostComment comment = await _repo.FindByIdAsync(id);

            if (comment != null)
            {
                if (comment.CreatorLogin == currentUserLogin)
                {
                    await _repo.RemoveAsync(comment);
                }
                else
                    AddError($"You are not permitted to delete comment with ID [{id}].", nameof(id));
            }
            else
                AddError($"Comment with ID [{id}] not found.", nameof(id));
        }
    }
}
