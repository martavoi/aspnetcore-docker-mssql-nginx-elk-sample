using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Oxagile.Internal.Api.Services;

namespace Oxagile.Internal.Api.Controllers
{
    [Route("api/images")]
    public class PicController : ControllerBase
    {
        private readonly Settings settings;
        private readonly IBlobStorage blobStorage;
        private readonly IImageProcessor imageProcessor;

        public PicController(
            IBlobStorage blobStorage,
            IImageProcessor imageProcessor,
            IOptions<Settings> options)
        {
            this.settings = options.Value;
            this.blobStorage = blobStorage;
            this.imageProcessor = imageProcessor;
        }

        [HttpGet("{id}.jpeg")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id, [FromQuery]int w = 100, [FromQuery]int h = 100)
        {
            if (!blobStorage.Exists(id))
            {
                return NotFound(new { Result = "error", Message = $"image {id}.jpeg does not exist"});
            }

            if (w < 100 || h < 100)
            {
                return BadRequest(new { Result = "error", Message = "width and height should be greater that or equal to 100." });
            }

            var file = await blobStorage.LoadAsync(id);
            var (jpeg, mimeType) = imageProcessor.ToJpeg(file, w, h);
            return File(jpeg, mimeType);
        }
    }
}