using Microsoft.AspNetCore.Mvc;

namespace Oxagile.Internal.Api.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Found)]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}