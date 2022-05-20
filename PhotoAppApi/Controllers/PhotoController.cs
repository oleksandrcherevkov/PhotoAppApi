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
        [HttpGet("post/{id}/{name}/{state?}")]
        public async Task<IActionResult> DownloadPostPhoto([Range(0, int.MaxValue)] [FromRoute] int id, [FromServices] PhotoService<PostPhoto> photoService, [FromRoute] string state = null)
        {
            bool compressed = !string.IsNullOrEmpty(state);

            var photo = await photoService.ReadAsync(id, compressed);

            if (photo != null)
            {
                var ext = Path.GetExtension(photo.Name).ToLowerInvariant().Replace(".", "");
                if (compressed)
                    return File(photo.CompressedData, $"image/{ext}", photo.Name);
                else
                    return File(photo.Data, $"image/{ext}", photo.Name);
            }
            else
            {
                ModelState.AddServiceErrors(photoService);
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpGet("avatar/{id}/{name}/{state?}")]
        public async Task<IActionResult> DownloadAvatar([Range(0, int.MaxValue)] [FromRoute] int id,  [FromServices] PhotoService<Avatar> avatarService, [FromRoute] string state = null)
        {
            bool compressed = !string.IsNullOrEmpty(state);
            var photo = await avatarService.ReadAsync(id, compressed);

            if (photo != null)
            {
                var ext = Path.GetExtension(photo.Name).ToLowerInvariant().Replace(".", "");
                if(compressed)
                    return File(photo.CompressedData, $"image/{ext}", photo.Name);
                else
                    return File(photo.Data, $"image/{ext}", photo.Name);
            }
            else
            {
                ModelState.AddServiceErrors(avatarService);
                return BadRequest(ModelState);
            }
        }
    }

    
}

