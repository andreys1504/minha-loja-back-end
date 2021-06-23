using MinhaLoja.Domain.ContaUsuarioAdministrador.Entities;
using System;
using System.Linq.Expressions;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.Queries
{
    public static class VendedorQueries
    {
        public static Expression<Func<Vendedor, bool>> CodigoValidacaoEmailValido(string codigo)
        {
            return vendedor => vendedor.CodigoValidacaoEmail == codigo;
        }

        public static Expression<Func<Vendedor, bool>> CadastroUsuarioAprovacaoPendente()
        {
            return vendedor => vendedor.CadastroAprovado.HasValue == false;
        }

        public static Expression<Func<Vendedor, bool>> CadastroUsuarioRejeitado()
        {
            return vendedor => vendedor.CadastroAprovado.HasValue && vendedor.CadastroAprovado.Value == false;
        }

        public static Expression<Func<Vendedor, bool>> PermissaoValidacaoEmail()
        {
            return vendedor => vendedor.DataMaximaCodigoValidacaoEmail >= DateTime.Now.Date;
        }

        public static Expression<Func<Vendedor, bool>> EmailUsuarioValidado()
        {
            return vendedor => vendedor.EmailValidado == true;
        }
    }
}
