using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.ExcluirVendedorCadastroRejeitado
{
    public class ExcluirVendedoresCadastroRejeitadoRequest : RequestAppService, IRequest<IResponseAppService<bool>>
    {
        public ExcluirVendedoresCadastroRejeitadoRequest()
        {
            IdUsuario = Guid.Empty;
        }

        public override Guid IdUsuario { get; }

        public override bool Validate()
        {
            return true;
        }
    }
}
