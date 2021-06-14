using MediatR;
using MinhaLoja.Core.Domain.Events;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.Events.Vendedor.ExcluirVendedoresCadastroRejeitado
{
    public class VendedoresExcluidosPorCadastroRejeitadoEvent : DomainEvent, INotification
    {
        public VendedoresExcluidosPorCadastroRejeitadoEvent(int idVendedor)
        {
            IdVendedor = idVendedor;
        }

        public int IdVendedor { get; private set; }
    }
}
