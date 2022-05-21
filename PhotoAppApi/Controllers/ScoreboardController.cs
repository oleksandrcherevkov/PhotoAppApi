using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoAppApi.DAL.Generic;
using PhotoAppApi.EF;
using PhotoAppApi.EF.Models;
using PhotoAppApi.Services.Posts.Models;
using PhotoAppApi.Services.Posts.QueryObjects;
using PhotoAppApi.Services.Users.Models;
using PhotoAppApi.Services.Users.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAppApi.Controllers
{
[Route("api/[controller]")]
    [ApiController]
    public class ScoreboardController : ControllerBase
    {
        private readonly IGenericRepository<User> _usersRepo;
        private readonly IGenericRepository<Post> _postsRepo;

        public ScoreboardController(IGenericRepository<User> usersRepo, IGenericRepository<Post> postsRepo)
        {
            _usersRepo = usersRepo;
            _postsRepo = postsRepo;
        }
        
        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> GetScoreBoard()
        {
            string hostName = HttpContext.Request.Host.Host;
            var now = DateTime.UtcNow;
            var searchingTime = TimeSpan.FromDays(7);


            List<UserScoreboardListDto> users = await _usersRepo
                .Read(u => u.Posts.Where(p => p.CreationTime > now.Subtract(searchingTime)).Any())
                .SelectScoreboardDto(hostName, now, searchingTime, compressedAvatar: true)
                .OrderByDescending(dto => dto.LikesInPeriod)
                .Take(4)
                .ToListAsync();


            List<PostListDto> posts = await _postsRepo
                .Read(p => p.CreationTime > now.Subtract(searchingTime))
                .OrderByDescending(p => p.LikesCount + p.Comments.Count())
                .SelectListDto(User.Identity.Name, hostName, compressedPhoto: true)
                .Take(3)
                .ToListAsync();

            var scoreboard = new 
            {
                TopPosts = posts,
                TopUsers = users
            };

            return Ok(scoreboard);
        }
    }
}
