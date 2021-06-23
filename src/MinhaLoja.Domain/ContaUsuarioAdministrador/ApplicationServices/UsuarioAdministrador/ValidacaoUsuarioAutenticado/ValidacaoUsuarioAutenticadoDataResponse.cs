using System;
using System.Collections.Generic;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.Validacao
{
    public class ValidacaoUsuarioAutenticadoDataResponse
    {
        public Guid IdUsuario { get; set; }
        public int? IdVendedor { get; set; }
        public string Nome { get; set; }
        public string Username { get; set; }
        public IList<string> Permissoes { get; set; }
    }
}
