using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoAppApi.HelperExtensions;
using PhotoAppApi.Services.Comments;
using PhotoAppApi.Services.Comments.Models;
using PhotoAppApi.Services.GeneralOptions;
using PhotoAppApi.Services.Posts;
using PhotoAppApi.Services.Posts.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PhotoAppApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly PostCommentService _commentService;

        public PostController(PostService postService, PostCommentService commentService)
        {
            _postService = postService;
            _commentService = commentService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Publish([FromForm] PostAddDto postInfo)
        {
            int postId = await _postService.AddAsync(postInfo, User.Identity.Name);

            if (_postService.HasErrors)
            {
                ModelState.AddServiceErrors(_postService);
                return BadRequest(ModelState);
            }

            return Ok(new {Id = postId});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostWithComments([Range(0, int.MaxValue)] [FromRoute] int id)
        {
            if (id > 0)
            {
                PostListDto post = await _postService.ReadPostAsync(id, User.Identity.Name);

                if (_postService.HasErrors)
                {
                    ModelState.AddServiceErrors(_postService);
                    return BadRequest(ModelState);
                }

                List<PostCommentListDto> comments = await _commentService
                    .ListComments(post.Id)
                    .ToListAsync();


                return Ok(new { Post = post, Comments = comments });
            }
            else
                return NotFound();
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListPhotos([FromQuery] PostsListOptions listOptions)
        {
            PostsPageDto page = await _postService
                .ListPostsAsync(listOptions, User.Identity.Name);

            return Ok(page);
        }

        [HttpGet("list/{login}")]
        public async Task<IActionResult> ListUsersPhotos([FromQuery] PostsListOptions listOptions,[StringLength(20, MinimumLength = 6)][FromRoute] string login)
        {
            PostsPageDto page = await _postService
                .ListPostsAsync(listOptions, User.Identity.Name, login);

            return Ok(page);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([Range(0, int.MaxValue)] [FromRoute] int id)
        {
            if (id > 0)
            {
                await _postService.DeleteAsync(id, User.Identity.Name);

                if (_postService.HasErrors)
                {
                    ModelState.AddServiceErrors(_postService);
                    return BadRequest(ModelState);
                }

                return Ok();
            }
            else
                return NotFound();
        }

        [HttpPut("like/{id}")]
        public async Task<IActionResult> Like([Range(0, int.MaxValue)] [FromRoute] int id)
        {
            if (id > 0)
            {
                await _postService.LikeAsync(id, User.Identity.Name);

                if (_postService.HasErrors)
                {
                    ModelState.AddServiceErrors(_postService);
                    return BadRequest(ModelState);
                }

                return Ok();
            }
            else
                return NotFound();
        }
    }
}
