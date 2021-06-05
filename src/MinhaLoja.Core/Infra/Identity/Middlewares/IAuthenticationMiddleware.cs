using Microsoft.Extensions.DependencyInjection;

namespace MinhaLoja.Core.Infra.Identity.Middlewares
{
    public interface IAuthenticationMiddleware
    {
        IServiceCollection AddAuthenticationApplication(IServiceCollection services);
    }
}
