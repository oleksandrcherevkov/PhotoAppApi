using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoAppApi.EF;
using PhotoAppApi.EF.Models;
using PhotoAppApi.HelperExtensions;
using PhotoAppApi.Services.Photos;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("post/{id}/{name}")]
        public async Task<IActionResult> DownloadPostPhoto([Range(0, int.MaxValue)] [FromRoute] int id, [FromServices] PhotoService<PostPhoto> photoService)
        {
            if(id > 0)
            {
                var photo = await photoService.ReadAsync(id);

                if (photo != null)
                {
                    var ext = Path.GetExtension(photo.Name).ToLowerInvariant().Replace(".", "");

                    return File(photo.Data, $"image/{ext}", photo.Name);
                }
                else
                {
                    ModelState.AddServiceErrors(photoService);
                    return BadRequest(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError(nameof(id) ,"Invalid photo id.");
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpGet("avatar/{id}/{name}")]
        public async Task<IActionResult> DownloadAvatar([Range(0, int.MaxValue)] [FromRoute] int id, [FromServices] PhotoService<Avatar> avatarService)
        {
            if (id > 0)
            {
                var photo = await avatarService.ReadAsync(id);

                if (photo != null)
                {
                    var ext = Path.GetExtension(photo.Name).ToLowerInvariant().Replace(".", "");

                    return File(photo.Data, $"image/{ext}", photo.Name);
                }
                else
                {
                    ModelState.AddServiceErrors(avatarService);
                    return BadRequest(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError(nameof(id), "Invalid photo id.");
                return BadRequest(ModelState);
            }
        }
    }

    
}

