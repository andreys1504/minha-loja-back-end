using MediatR;
using MinhaLoja.Core.Domain.Events;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.Events.UsuarioAdministrador.CadastroUsuarioMaster
{
    public class UsuarioMasterCadastradoEvent : DomainEvent, INotification
    {
        public UsuarioMasterCadastradoEvent(
            int idUsuarioAdministrador,
            string nome,
            string username)
        {
            IdUsuarioAdministrador = idUsuarioAdministrador;
            Nome = nome;
            Username = username;
        }

        public int IdUsuarioAdministrador { get; private set; }
        public string Nome { get; private set; }
        public string Username { get; private set; }
    }
}
