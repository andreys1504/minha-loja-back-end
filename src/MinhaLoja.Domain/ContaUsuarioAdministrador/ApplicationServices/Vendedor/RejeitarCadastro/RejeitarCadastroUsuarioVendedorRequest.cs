using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using MensagensVendedor = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.RejeitarCadastro
{
    public class RejeitarCadastroUsuarioVendedorRequest : RequestAppService, IRequest<IResponseAppService<bool>>
    {
        public RejeitarCadastroUsuarioVendedorRequest(
            int idVendedor,
            Guid idUsuario)
        {
            IdVendedor = idVendedor;
            IdUsuario = idUsuario;
        }

        public int IdVendedor { get; private set; }
        public override Guid IdUsuario { get; }

        public override bool Validate()
        {
            AddNotifications(new Contract<Notification>()
                .IsGreaterOrEqualsThan(this.IdVendedor, 1, nameof(this.IdVendedor), MensagensVendedor.Vendedor_Rejeitar_IdVendedorIsGreaterOrEqualsThan)
            );

            return IsValid;
        }
    }
}
