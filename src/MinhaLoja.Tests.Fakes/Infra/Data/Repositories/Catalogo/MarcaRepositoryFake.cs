using MinhaLoja.Domain.Catalogo.Entities;
using MinhaLoja.Domain.Catalogo.Repositories;
using System.Collections.Generic;

namespace MinhaLoja.Tests.Fakes.Infra.Data.Repositories.Catalogo
{
    public class MarcaRepositoryFake : RepositoryBaseFake<Marca>, IMarcaRepository
    {
        public MarcaRepositoryFake(IList<Marca> entities) : base(entities)
        {
        }
    }
}
