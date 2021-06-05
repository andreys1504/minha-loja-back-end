using MediatR;
using MinhaLoja.Core.Domain.Events;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.Events.Vendedor.RejeicaoCadastro
{
    public class CadastroVendedorRejeitadoEvent : DomainEvent, INotification
    {
        public CadastroVendedorRejeitadoEvent(
            int idVendedor, 
            string email)
        {
            IdVendedor = idVendedor;
            Email = email;
        }

        public int IdVendedor { get; private set; }
        public string Email { get; private set; }
    }
}
