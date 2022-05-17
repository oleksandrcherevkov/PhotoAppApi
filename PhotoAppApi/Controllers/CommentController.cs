using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoAppApi.HelperExtensions;
using PhotoAppApi.Services.Comments;
using PhotoAppApi.Services.Comments.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PhotoAppApi.Controllers
{
    [Authorize]
    [Route("api/post/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly PostCommentService _commentService;
        public CommentController(PostCommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddComment(PostCommentAddDto addDto)
        {
            int commentId = await _commentService.AddAsync(addDto, User.Identity.Name);

            if (_commentService.HasErrors)
            {
                ModelState.AddServiceErrors(_commentService);
                return BadRequest(ModelState);
            } 

            return Ok(new { Id = commentId });
        }

        //[HttpGet("list/{postId}")]
        //public async Task<IActionResult> ListComments([FromRoute] int postId)
        //{
        //    if (postId < 0)
        //        return NotFound();

        //    List<PostCommentListDto> comments = await _commentService
        //        .ListComments(postId)
        //        .ToListAsync();

        //    return Ok(comments);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([Range(0, int.MaxValue)] [FromRoute] int id)
        {
            if (id < 0)
                return NotFound();

            await _commentService.DeleteAsync(id, User.Identity.Name);

            if (_commentService.HasErrors)
            {
                ModelState.AddServiceErrors(_commentService);
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
