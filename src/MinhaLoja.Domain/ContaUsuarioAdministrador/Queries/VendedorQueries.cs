using MinhaLoja.Domain.ContaUsuarioAdministrador.Entities;
using System;
using System.Linq.Expressions;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.Queries
{
    public static class VendedorQueries
    {
        public static Expression<Func<Entities.Vendedor, bool>> CodigoValidacaoEmailValido(string codigo)
        {
            return vendedor => vendedor.CodigoValidacaoEmail == codigo;
        }


        public static bool PermissaoValidacaoEmail(this Vendedor vendedor)
        {
            return vendedor.DataMaximaCodigoValidacaoEmail >= DateTime.Now.Date;
        }

        public static bool CadastroUsuarioAprovacaoPendente(this Vendedor vendedor)
        {
            return vendedor.CadastroAprovado.HasValue is false;
        }

        public static bool CadastroUsuarioAprovado(this Vendedor vendedor)
        {
            return vendedor.CadastroAprovado.HasValue && vendedor.CadastroAprovado.Value;
        }

        public static bool EmailUsuarioValidado(this Vendedor vendedor)
        {
            return vendedor.EmailValidado;
        }
    }
}
