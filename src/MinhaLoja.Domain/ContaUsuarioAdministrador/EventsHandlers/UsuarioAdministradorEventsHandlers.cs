using MediatR;
using MinhaLoja.Core.Domain.Events.EventHandler;
using MinhaLoja.Core.Infra.Services.Mail;
using MinhaLoja.Core.Settings;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Events.UsuarioAdministrador.CadastroUsuarioMaster;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Events.UsuarioAdministrador.CadastroUsuarioVendedor;
using System.Threading;
using System.Threading.Tasks;
using MensagensVendedor = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.EventsHandlers
{
    public class UsuarioAdministradorEventsHandlers : DomainEventHandler,
        INotificationHandler<UsuarioMasterCadastradoEvent>,
        INotificationHandler<UsuarioVendedorCadastradoEvent>
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IMailService _mailService;

        public UsuarioAdministradorEventsHandlers(
            GlobalSettings globalSettings,
            IMailService mailService,
            DependenciesEventHandler dependenciesEventHandler) 
            : base(dependenciesEventHandler)
        {
            _globalSettings = globalSettings;
            _mailService = mailService;
        }

        public Task Handle(UsuarioMasterCadastradoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task Handle(UsuarioVendedorCadastradoEvent notification, CancellationToken cancellationToken)
        {
            await _mailService.SendMailAsync(
                to: notification.EmailVendedor,
                subject: MensagensVendedor.Vendedor_AssuntoMensagemValidacaoEmail,
                body: Entities.Vendedor.CorpoMensagemValidacaoEmail(
                    globalSettings: _globalSettings,
                    codigoValidacaoEmail: notification.CodigoValidacaoEmail
                )
            );
        }
    }
}
