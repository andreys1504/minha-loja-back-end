using MediatR;
using MinhaLoja.Core.Domain.Events;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.Events.Vendedor.ValidacaoEmail
{
    public class GeradoNovoCodigoValidacaoEmailVendedorEvent : DomainEvent, INotification
    {
        public GeradoNovoCodigoValidacaoEmailVendedorEvent(
            int idUsuarioAdministrador,
            int idVendedor,
            string emailVendedor,
            string codigoValidacaoEmail)
        {
            IdUsuarioAdministrador = idUsuarioAdministrador;
            IdVendedor = idVendedor;
            EmailVendedor = emailVendedor;
            CodigoValidacaoEmail = codigoValidacaoEmail;
        }

        public int IdUsuarioAdministrador { get; private set; }
        public int IdVendedor { get; private set; }
        public string EmailVendedor { get; private set; }
        public string CodigoValidacaoEmail { get; private set; }
    }
}
