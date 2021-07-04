using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using System.Collections.Generic;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.TiposProdutos
{
    public class TiposProdutosRequest : RequestAppService, IRequest<IResponseAppService<IList<TiposProdutosDataResponse>>>
    {
        public TiposProdutosRequest(int? idTipoProdutoSuperior)
        {
            IdTipoProdutoSuperior = idTipoProdutoSuperior;
        }

        public int? IdTipoProdutoSuperior { get; private set; }
        public override Guid IdUsuarioEnvioRequest { get; }

        public override bool Validate()
        {
            return IsValid;
        }
    }
}
