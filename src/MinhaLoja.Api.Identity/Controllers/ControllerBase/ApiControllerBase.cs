using Microsoft.AspNetCore.Mvc;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Mediator;
using System.Threading.Tasks;

namespace MinhaLoja.Api.Identity.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        private IMediatorHandler MediatorHandler
        {
            get => (IMediatorHandler)HttpContext.RequestServices.GetService(typeof(IMediatorHandler));
        }

        protected async Task<object> SendRequestService(RequestService request)
        {
            return await MediatorHandler.SendRequestServiceToHandlerAsync(request);
        }
    }
}
