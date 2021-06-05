using System;
using System.Linq.Expressions;

namespace MinhaLoja.Domain.Catalogo.Queries
{
    public static class ProdutoQueries
    {
        public static Expression<Func<Entities.Produto, bool>> ProdutoExistenteSistemaParaCadastro(
            string nomeProduto,
            int idMarca,
            int? idVendedor)
        {
            return produto => produto.Nome.ToUpper() == nomeProduto.Trim().ToUpper()
                            && produto.MarcaId == idMarca
                            && produto.VendedorId == idVendedor;
        }
    }
}
