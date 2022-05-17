using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoAppApi.DAL.Generic;
using PhotoAppApi.EF;
using PhotoAppApi.EF.Models;
using PhotoAppApi.HelperExtensions;
using PhotoAppApi.Services.Comments;
using PhotoAppApi.Services.Posts;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAppApi.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly IGenericRepository<Post> _postsRepo;

        public AdminController(PostService postService, IGenericRepository<Post> postsRepo)
        {
            _postService = postService;
            _postsRepo = postsRepo;
        }

        [HttpDelete("post/{id}")]
        public async Task<IActionResult> DeletePots([Range(0, int.MaxValue)] [FromRoute] int id)
        {
            if (id < 0)
                return NotFound();

            string postCreatorLogin = _postsRepo.Read(p => p.Id == id)
                .Select(p => p.CreatorLogin)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(postCreatorLogin))
            {
                ModelState.AddModelError(nameof(id), $"Post with ID [{id}] not found.");
                return BadRequest(ModelState);
            }

            await _postService.DeleteAsync(id, postCreatorLogin);

            if (_postService.HasErrors)
            {
                ModelState.AddServiceErrors(_postService);
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}

