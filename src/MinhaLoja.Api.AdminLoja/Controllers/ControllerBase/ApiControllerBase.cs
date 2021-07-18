using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Infra.Identity.Services;
using MinhaLoja.Core.Mediator;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        private IClaimService ClaimService
        {
            get => (IClaimService)HttpContext.RequestServices.GetService(typeof(IClaimService));
        }

        protected Guid IdUsuario(ClaimsPrincipal user)
        {
            string id2Usuario = ClaimService.GetUserId(user);
            _ = Guid.TryParse(id2Usuario, out Guid idUsuarioRetorno);

            return idUsuarioRetorno;
        }

        protected int? IdVendedor(ClaimsPrincipal user)
        {
            string idVendedor = ClaimService.GetSellerId(user);
            if (string.IsNullOrWhiteSpace(idVendedor))
            {
                return null;
            }

            _ = int.TryParse(idVendedor, out int idVendedorRetorno);

            return idVendedorRetorno;
        }

        protected async Task<object> SendRequestService(RequestAppService request)
        {
            return await MediatorHandler.SendRequestToHandlerAsync(request);
        }

        protected IActionResult ReturnApi(HttpStatusCode statusCode, object value = null, bool valueJson = false)
        {
            if (value is IResponse response)
            {
                if (response.Success == false)
                {
                    statusCode = HttpStatusCode.BadRequest;
                }
            }

            if (valueJson)
            {
                return Content(value.ToString(), "application/json");
            }

            return StatusCode((int)statusCode, value);
        }

        protected string SerializeReturnApi(object data)
        {
            return JsonConvert.SerializeObject(
                value: data,
                settings: new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        protected DistributedCacheEntryOptions GetOptionsSaveCache(TimeSpan? expiration = null)
        {
            var opcoesCache = new DistributedCacheEntryOptions();
            opcoesCache.SetAbsoluteExpiration(expiration ?? TimeSpan.FromMinutes(1));

            return opcoesCache;
        }
    }
}
