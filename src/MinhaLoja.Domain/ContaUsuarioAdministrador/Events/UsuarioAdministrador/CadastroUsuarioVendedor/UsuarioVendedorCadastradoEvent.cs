using MediatR;
using MinhaLoja.Core.Domain.Events;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.Events.UsuarioAdministrador.CadastroUsuarioVendedor
{
    public class UsuarioVendedorCadastradoEvent : DomainEvent, INotification
    {
        public UsuarioVendedorCadastradoEvent(
            int idUsuarioAdministrador,
            int idVendedor,
            string nomeVendedor,
            string emailVendedor,
            string cnpj, 
            string codigoValidacaoEmail)
        {
            IdUsuarioAdministrador = idUsuarioAdministrador;
            IdVendedor = idVendedor;
            NomeVendedor = nomeVendedor;
            EmailVendedor = emailVendedor;
            Cnpj = cnpj;
            CodigoValidacaoEmail = codigoValidacaoEmail;
        }

        public int IdUsuarioAdministrador { get; private set; }
        public int IdVendedor { get; private set; }
        public string NomeVendedor { get; private set; }
        public string EmailVendedor { get; private set; }
        public string Cnpj { get; private set; }
        public string CodigoValidacaoEmail { get; private set; }
    }
}
