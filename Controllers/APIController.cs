using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [Route("veryhiddenapikey")]
    public class APIController : Controller
    {
        private readonly IConfiguration _configuration;
        public APIController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult GetSecretValue()
        {
            var apiKey = _configuration["APIKEY"];

            ViewBag.ApiKey = apiKey;

            return Content(apiKey, "text/html");
        }
    }
}
