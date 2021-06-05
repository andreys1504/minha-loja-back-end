using MediatR;
using MinhaLoja.Core.Domain.Events;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.Events.Vendedor.AprovacaoCadastro
{
    public class CadastroVendedorAprovadoEvent : DomainEvent, INotification
    {
        public CadastroVendedorAprovadoEvent(
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
