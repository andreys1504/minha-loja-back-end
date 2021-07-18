using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.UsuarioAdministrador.Validacao
{
    public class ValidacaoUsuarioAutenticadoRequest : RequestAppService, IRequest<IResponseAppService<ValidacaoUsuarioAutenticadoDataResponse>>
    {
        public ValidacaoUsuarioAutenticadoRequest(
            Guid idUsuario,
            string username)
        {
            IdUsuarioEnvioRequest = idUsuario;
            Username = username.TrimString();
        }

        public override Guid IdUsuarioEnvioRequest { get; }
        public string Username { get; private set; }

        public override bool Validate()
        {
            AddNotifications(new Contract<Notification>()
                .AreNotEquals(this.IdUsuarioEnvioRequest, Guid.Empty, "IdUsuário inválido")
                .AreNotEquals(this.Username, Guid.Empty, "Username inválido")
            );

            return IsValid;
        }
    }
}
