using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace web_api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class DemoController : Controller
    {
        [HttpGet, Authorize(Startup.REQUIRE_DATA_READER_POLICY)]
        public dynamic GetData()
        {
            return new 
            {
                message = "Hello from web API GET endpoint."
            };
        }

        [HttpPost, Authorize(Startup.REQUIRE_DATA_WRITER_POLICY)]
        public dynamic PostData()
        {
            return new 
            {
                message = "Hello from web API POST endpoint."
            };
        }
    }
}
