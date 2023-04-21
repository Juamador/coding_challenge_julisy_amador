using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DroneDeliveryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("The service is running successfully!!!");
        }
    }
}
