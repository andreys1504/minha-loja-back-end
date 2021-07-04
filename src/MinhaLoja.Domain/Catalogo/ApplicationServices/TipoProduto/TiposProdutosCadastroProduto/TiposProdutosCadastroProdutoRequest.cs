using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Request;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using System;
using System.Collections.Generic;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.TiposProdutosCadastroProduto
{
    public class TiposProdutosCadastroProdutoRequest : RequestAppService, IRequest<IResponseAppService<IList<TiposProdutosCadastroProdutoDataResponse>>>
    {
        public TiposProdutosCadastroProdutoRequest(int? idTipoProdutoSuperior)
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
