using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using MensagensVendedor = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.AprovarCadastro
{
    public class AprovarCadastroUsuarioVendedorRequest : RequestAppService, IRequest<IResponseAppService<bool>>
    {
        public AprovarCadastroUsuarioVendedorRequest(
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
                .IsGreaterOrEqualsThan(this.IdVendedor, 1, nameof(this.IdVendedor), MensagensVendedor.Vendedor_Aprovar_IdVendedorIsGreaterOrEqualsThan)
            );

            return IsValid;
        }
    }
}
