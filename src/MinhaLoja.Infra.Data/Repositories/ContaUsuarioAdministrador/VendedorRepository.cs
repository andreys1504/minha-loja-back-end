using MinhaLoja.Domain.ContaUsuarioAdministrador.Entities;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using MinhaLoja.Infra.Data.DataSources;
using MinhaLoja.Infra.Data.Repositories.RepositoryBase;

namespace MinhaLoja.Infra.Data.Repositories.ContaUsuarioAdministrador
{
    public class VendedorRepository : RepositoryBase<Vendedor>, IVendedorRepository
    {
        public VendedorRepository(DependenciesRepositories dependenciesRepositories)
            : base(dependenciesRepositories)
        {
        }
    }
}
