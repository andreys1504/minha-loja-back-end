using System;
using System.Linq.Expressions;

namespace MinhaLoja.Domain.Catalogo.Queries
{
    public static class MarcaQueries
    {
        public static Expression<Func<Entities.Marca, bool>>
            MarcaExistenteSistema(string nomeMarca)
        {
            return marca => marca.Nome.ToUpper() == nomeMarca.Trim().ToUpper();
        }
    }
}
