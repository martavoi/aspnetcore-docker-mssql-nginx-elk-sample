using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Oxagile.Internal.Api.Services;

namespace Oxagile.Internal.Api.Controllers
{
    [Route("api/nullref")]
    public class NullRefController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult NullRef()
        {
            throw new System.NullReferenceException();
        }
    }
}