using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MinhaLoja.Api.AdminLoja.Controllers
{
    [ApiController]
    [Route("home")]
    public class HomeController : ApiControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok("MinhaLoja.Api.AdminLoja");
        }

        [HttpGet("teste")]
        public IActionResult Teste()
        {
            return Ok("MinhaLoja.Api.AdminLoja");
        }
    }
}
