using MinhaLoja.Domain.ContaUsuarioAdministrador.Entities;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using System.Collections.Generic;

namespace MinhaLoja.Tests.Fakes.Infra.Data.Repositories.ContaUsuarioAdministrador
{
    public class VendedorRepositoryFake : RepositoryBaseFake<Vendedor>, IVendedorRepository
    {
        public VendedorRepositoryFake(IList<Vendedor> entities) : base(entities)
        {
        }
    }
}
