using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MinhaLoja.Api.AdminLoja.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ApiControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public string Get()
        {
            return "MinhaLoja.Api.AdminLoja";
        }
    }
}
