using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoAppApi.EF.Models;
using PhotoAppApi.Services.Users;
using System;
using System.Linq;
using System.Threading.Tasks;
using PhotoAppApi.HelperExtensions;
using PhotoAppApi.Services.Mail;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using PhotoAppApi.Services.Users.Models;
using PhotoAppApi.Services.Users.QueryObjects;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using PhotoAppApi.Services.Users.JWT;
using Microsoft.Extensions.Options;

namespace PhotoAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly EmailService _emailService;
        private readonly JWTConstructor _jwtConstructor;
        private readonly string _webAppLoginUrl;
        public UserController(UserService userService, EmailService emailService, JWTConstructor jwtConstructor, IConfiguration config)
        {
            _userService = userService;
            _emailService = emailService;
            _jwtConstructor = jwtConstructor;
            _webAppLoginUrl = config["WebAppUrl"] + "/login";
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto user)
        {
            await _userService.AddAsync(user);

            if (!_userService.HasErrors)
            {
                string hash = _emailService.CreateHash(user.Login, _jwtConstructor.JWTOptions.KEY);

                var callbackUrl = Url.Action(
                            "ConfirmEmail",
                            "User",
                            new { login = user.Login, hash = hash },
                            protocol: HttpContext.Request.Scheme);

                await _emailService.SendEmailAsync(user.Email, "Confirm your account",
                    $"Подтвердите регистрацию, перейдя по ссылке: {callbackUrl}");

                return Ok();
            }
            else
            {
                ModelState.AddServiceErrors(_userService);
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpGet("register/{login}/{hash}")]
        public async Task<IActionResult> ConfirmEmail([StringLength(20, MinimumLength = 6)] [FromRoute] string login, [FromRoute] string hash)
        {
            if (!String.IsNullOrEmpty(login) || !String.IsNullOrEmpty(hash))
            {
                if (_emailService.ValidateHash(login, _jwtConstructor.JWTOptions.KEY, hash))
                {
                    await _userService.ConfirmAsync(login);
                    if (_userService.HasErrors)
                    {
                        ModelState.AddServiceErrors(_userService);
                        return BadRequest(ModelState);
                    }
                    return Redirect(_webAppLoginUrl);
                }
                else
                {
                    ModelState.AddModelError(nameof(hash), "Hash is not valid.");
                    return BadRequest(ModelState);
                }
            }
            else
                return NotFound();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginInfo)
        {
            User user = await _userService.ReadConfirmedWithPasswordAsync(loginInfo.Login, loginInfo.Password);

            if (user == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            string encodedJwt = _jwtConstructor.CreateJWTAsync(user.Login, user.Role);

            string host = HttpContext.Request.Host.Host;
            UserListDto userInfo = await _userService
                .ReadConfirmedAsync(u => u.SelectListDto(host),loginInfo.Login);

            var response = new
            {
                Token = encodedJwt,
                Login = userInfo.Login,
                Role = userInfo.Role,
                Avatar = userInfo.Avatar
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> GetInfo()
        {
            string host = HttpContext.Request.Host.Host;
            UserListDto user = await _userService
                .ReadConfirmedAsync(u => u.SelectListDto(host), User.Identity.Name);

            if (user == null)
                return BadRequest();

            return Ok(user);
        }

        [Authorize]
        [HttpGet("{login}")]
        public async Task<IActionResult> GetInfo([StringLength(20, MinimumLength = 6)] [FromRoute] string login)
        {
            string host = HttpContext.Request.Host.Host;
            var now = DateTime.UtcNow;
            var serchingTime = TimeSpan.FromDays(7);
            UserListDto user = await _userService
                .ReadConfirmedAsync(u => u.SelectScoreboardDto(host, now, serchingTime), login);

            if (user == null)
            {
                ModelState.AddModelError(nameof(login), $"User with LOGIN [{login}] not found.");
                return BadRequest();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpPost("avatar/")]
        public async Task<IActionResult> ChangeAvatar([FromForm] IFormFile file)
        {

                await _userService.ChangeAvatarAsync(file, User.Identity.Name);

                if (_userService.HasErrors)
                {
                    ModelState.AddServiceErrors(_userService);
                    return BadRequest(ModelState);
                }

                return Ok();
        }

    }
}
