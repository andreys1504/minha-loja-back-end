using Microsoft.AspNetCore.Mvc;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Infra.Identity.Services;
using MinhaLoja.Core.Mediator;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MinhaLoja.Api.AdminLoja.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        private IMediatorHandler MediatorHandler
        {
            get => (IMediatorHandler)HttpContext.RequestServices.GetService(typeof(IMediatorHandler));
        }

        private IIdentityService IdentityService
        {
            get => (IIdentityService)HttpContext.RequestServices.GetService(typeof(IIdentityService));
        }

        protected Guid IdUsuario(ClaimsPrincipal user)
        {
            string id2Usuario = IdentityService.GetUserId(user);
            _ = Guid.TryParse(id2Usuario, out Guid idUsuarioRetorno);

            return idUsuarioRetorno;
        }

        protected int? IdVendedor(ClaimsPrincipal user)
        {
            string idVendedor = IdentityService.GetSellerId(user);
            if (string.IsNullOrWhiteSpace(idVendedor))
                return null;

            _ = int.TryParse(idVendedor, out int idVendedorRetorno);

            return idVendedorRetorno;
        }

        protected async Task<object> SendRequestService(RequestAppService request)
        {
            return await MediatorHandler.SendRequestServiceToHandlerAsync(request);
        }

        protected ObjectResult ReturnApi(HttpStatusCode statusCode, object value = null)
        {
            if (value is IResponse response)
                if (response.Success is false)
                    statusCode = HttpStatusCode.OK;

            return StatusCode((int)statusCode, value);
        }
    }
}
