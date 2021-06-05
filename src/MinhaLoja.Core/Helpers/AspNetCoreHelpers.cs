using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace MinhaLoja
{
    public static partial class Helpers
    {
        public static T GetServiceInConfigureServices<T>(this IServiceCollection services)
        {
            ServiceDescriptor serviceDescriptorAuthenticationMiddleware = services.First(x => x.ServiceType == typeof(T));
            
            return (T)serviceDescriptorAuthenticationMiddleware.ImplementationInstance;
        }
    }
}
