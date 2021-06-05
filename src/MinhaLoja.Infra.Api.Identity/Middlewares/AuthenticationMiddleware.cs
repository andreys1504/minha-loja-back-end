using Microsoft.Extensions.DependencyInjection;
using MinhaLoja.Core.Infra.Identity.Middlewares;
using MinhaLoja.Infra.Identity.IdentityStorage;
using NetDevPack.Security.JwtSigningCredentials;

namespace MinhaLoja.Infra.Identity.Middlewares
{
    public class AuthenticationMiddleware : IAuthenticationMiddleware
    {
        public IServiceCollection AddAuthenticationApplication(IServiceCollection services)
        {
            services.AddJwksManager(options =>
            {
                options.Jws = JwsAlgorithm.ES256;
                options.Jwe = JweAlgorithm.RsaOAEP.WithEncryption(Encryption.Aes128CbcHmacSha256);
            })
            .PersistKeysToDatabaseStore<IdentityMinhaLojaContext>();

            return services;
        }
    }
}
