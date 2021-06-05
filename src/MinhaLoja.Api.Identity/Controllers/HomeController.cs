using Microsoft.AspNetCore.Mvc;

namespace MinhaLoja.Api.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ApiControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "MinhaLoja.Api.Identity";
        }
    }
}
