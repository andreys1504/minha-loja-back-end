using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using System.Collections.Generic;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.Autenticacao
{
    public class AutenticacaoUsuarioAdministradorDataResponse
    {
        public Guid IdUsuario { get; set; }
        public int? IdVendedor { get; set; }
        public string Nome { get; set; }
        public string Username { get; set; }
        public IList<string> Permissions { get; set; }
    }
}
