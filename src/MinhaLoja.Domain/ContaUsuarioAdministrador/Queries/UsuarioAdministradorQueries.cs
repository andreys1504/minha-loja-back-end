using MinhaLoja.Core.Settings;
using System;
using System.Linq.Expressions;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.Queries
{
    public static class UsuarioAdministradorQueries
    {
        public static Expression<Func<Entities.UsuarioAdministrador, bool>>
            UsuarioExistenteSistema(string username)
        {
            return usuario => usuario.Username == username.TrimString();
        }

        public static Expression<Func<Entities.UsuarioAdministrador, bool>>
            UsuarioVendedorExistenteSistema(string email, string cnpj)
        {
            return usuario => usuario.Username == email.TrimString()
                            || usuario.Vendedor.Cnpj == cnpj.GetNumbers();
        }

        public static Expression<Func<Entities.UsuarioAdministrador, bool>>
            Autenticar(GlobalSettings globalSettings, string username, string senha)
        {
            senha = Entities.UsuarioAdministrador.CriptografarSenha(
                    globalSettings: globalSettings,
                    senha: senha);

            return usuario => usuario.Username == username.Trim()
                && usuario.Senha == senha;
        }

        public static Expression<Func<Entities.UsuarioAdministrador, bool>> PermitirExclusaoVendedorPorCadastroNaoAprovado()
        {
            return usuario => usuario.Vendedor.CadastroAprovado.HasValue
                && usuario.Vendedor.CadastroAprovado.Value == false
                && usuario.Vendedor.DataCadastro.Date <= DateTime.Now.Date.AddDays(-7);
        }

        public static Expression<Func<Entities.UsuarioAdministrador, bool>> 
            ValidarUsuarioAutenticado(
                Guid idUsuario,
                string username)
        {
            return usuario => usuario.Id2 == idUsuario
                && usuario.Username == username;
        }
    }
}
