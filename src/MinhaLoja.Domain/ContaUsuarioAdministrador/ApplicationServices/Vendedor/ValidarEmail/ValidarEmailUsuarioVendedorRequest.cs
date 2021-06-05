using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using System.Collections.Generic;
using MensagensVendedor = MinhaLoja.Domain.MessagesDomain.ContaUsuarioAdministrador;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.ValidarEmail
{
    public class ValidarEmailUsuarioVendedorRequest : RequestService, IRequest<IResponseService<IList<string>>>
    {
        public ValidarEmailUsuarioVendedorRequest(string codigo)
        {
            Codigo = codigo.TrimString();
        }

        public string Codigo { get; private set; }
        public override Guid IdUsuario { get; }

        public override bool Validate()
        {
            AddNotifications(new Contract<Notification>()
                .IsNotNullOrWhiteSpace(this.Codigo, nameof(Codigo), MensagensVendedor.Vendedor_ValidarEmail_CodigoIsNotNullOrWhiteSpace)
            );

            return IsValid;
        }
    }
}
