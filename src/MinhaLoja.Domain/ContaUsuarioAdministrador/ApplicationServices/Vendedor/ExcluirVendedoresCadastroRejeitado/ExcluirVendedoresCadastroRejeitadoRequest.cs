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
            IdUsuarioEnvioRequest = Guid.Empty;
        }

        public override Guid IdUsuarioEnvioRequest { get; }

        public override bool Validate()
        {
            return true;
        }
    }
}
