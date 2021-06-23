using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using System.Collections.Generic;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.VendedoresValidacaoCadastroPendente
{
    public class VendedoresValidacaoCadastroPendenteRequest : RequestAppService,
        IRequest<IResponseAppService<List<VendedoresValidacaoCadastroPendenteDataResponse>>>
    {
        public override Guid IdUsuario { get; }

        public override bool Validate()
        {
            return IsValid;
        }
    }
}
