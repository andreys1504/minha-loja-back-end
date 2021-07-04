using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using MensagensUsuario = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.Autenticacao
{
    public class AutenticacaoUsuarioAdministradorRequest : RequestAppService, IRequest<IResponseAppService<AutenticacaoUsuarioAdministradorDataResponse>>
    {
        public AutenticacaoUsuarioAdministradorRequest(
            string username,
            string senha)
        {
            Username = username.TrimString();
            Senha = senha.TrimString();
        }

        public string Username { get; private set; }
        public string Senha { get; private set; }
        public override Guid IdUsuarioEnvioRequest { get; }

        public override bool Validate()
        {
            string mensagemPadrao = MensagensUsuario.UsuarioAdministrador_Autenticacao_UsernameSenhaMensagemGenerica;

            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.Username, nameof(this.Username), MensagensUsuario.UsuarioAdministrador_Autenticacao_UsernameIsNotNullOrWhiteSpace)
                .IsLowerOrEqualsThan(this.Username, 45, nameof(this.Username), mensagemPadrao)
            );

            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.Senha, nameof(this.Senha), MensagensUsuario.UsuarioAdministrador_Autenticacao_SenhaIsNotNullOrWhiteSpace)
                .IsGreaterOrEqualsThan(this.Senha, 8, nameof(this.Senha), mensagemPadrao)
                .IsLowerOrEqualsThan(this.Senha, 20, nameof(this.Senha), mensagemPadrao)
            );

            return IsValid;
        }
    }
}
