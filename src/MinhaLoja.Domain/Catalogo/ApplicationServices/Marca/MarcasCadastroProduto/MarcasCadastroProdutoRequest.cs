using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using System.Collections.Generic;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.Marca.MarcasCadastroProduto
{
    public class MarcasCadastroProdutoRequest : RequestAppService, IRequest<IResponseAppService<IList<MarcasCadastroProdutoDataResponse>>>
    {
        public override Guid IdUsuario { get; }

        public override bool Validate()
        {
            return IsValid;
        }
    }
}
