using Microsoft.Extensions.DependencyInjection;
using MinhaLoja.Domain.Catalogo.Repositories;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using MinhaLoja.Infra.Data.Repositories.Catalogo;
using MinhaLoja.Infra.Data.Repositories.ContaUsuarioAdministrador;

namespace MinhaLoja.Infra.Data
{
    public static class MappingDependenciesRepositories
    {
        public static void RegisterMappings(IServiceCollection services)
        {
            Catalogo(services);
            ContaUsuarioProfissional(services);
        }

        public static void Catalogo(IServiceCollection services)
        {
            services.AddTransient<IMarcaRepository, MarcaRepository>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<ITipoProdutoRepository, TipoProdutoRepository>();
        }

        public static void ContaUsuarioProfissional(IServiceCollection services)
        {
            services.AddTransient<IUsuarioAdministradorRepository, UsuarioAdministradorRepository>();
            services.AddTransient<IVendedorRepository, VendedorRepository>();
        }
    }
}
