using MediatR;
using MinhaLoja.Core.Domain.Events;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.Events.Vendedor.ValidacaoEmail
{
    public class EmailUsuarioVendedorValidadoEvent : DomainEvent, INotification
    {
        public EmailUsuarioVendedorValidadoEvent(int idVendedor)
        {
            IdVendedor = idVendedor;
        }

        public int IdVendedor { get; private set; }
    }
}
