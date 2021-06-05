using MediatR;
using MinhaLoja.Core.Domain.Events.EventHandler;
using MinhaLoja.Core.Infra.Services.Mail;
using MinhaLoja.Core.Settings;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Events.Vendedor.AprovacaoCadastro;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Events.Vendedor.RejeicaoCadastro;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Events.Vendedor.ValidacaoEmail;
using System.Threading;
using System.Threading.Tasks;
using MensagensUsuario = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.EventsHandlers
{
    public class VendedorEventsHandlers : DomainEventHandler,
        INotificationHandler<CadastroVendedorAprovadoEvent>,
        INotificationHandler<CadastroVendedorRejeitadoEvent>,
        INotificationHandler<EmailUsuarioVendedorValidadoEvent>,
        INotificationHandler<GeradoNovoCodigoValidacaoEmailVendedorEvent>
    {
        private readonly IMailService _mailService;
        private readonly GlobalSettings _globalSettings;

        public VendedorEventsHandlers(
            IMailService mailService,
            GlobalSettings globalSettings,
            DependenciesEventHandler dependenciesEventHandler)
            : base(dependenciesEventHandler)
        {
            _mailService = mailService;
            _globalSettings = globalSettings;
        }

        public async Task Handle(CadastroVendedorAprovadoEvent notification, CancellationToken cancellationToken)
        {
            await _mailService.SendMailAsync(
                to: notification.Email,
                subject: MensagensUsuario.Vendedor_AssuntoMensagemAprovacaoCadastro,
                body: Entities.Vendedor.CorpoMensagemAprovacaoCadastro()
            );
        }

        public async Task Handle(CadastroVendedorRejeitadoEvent notification, CancellationToken cancellationToken)
        {
            await _mailService.SendMailAsync(
                to: notification.Email,
                subject: MensagensUsuario.Vendedor_AssuntoMensagemAprovacaoCadastro,
                body: Entities.Vendedor.CorpoMensagemRejeicaoCadastro()
            );
        }

        public Task Handle(EmailUsuarioVendedorValidadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task Handle(GeradoNovoCodigoValidacaoEmailVendedorEvent notification, CancellationToken cancellationToken)
        {
            await _mailService.SendMailAsync(
                to: notification.EmailVendedor,
                subject: MensagensUsuario.Vendedor_AssuntoMensagemValidacaoEmail,
                body: Entities.Vendedor.CorpoMensagemValidacaoEmail(
                    globalSettings: _globalSettings,
                    codigoValidacaoEmail: notification.CodigoValidacaoEmail
                )
            );
        }
    }
}
