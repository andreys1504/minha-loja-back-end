using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Domain.Catalogo.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.TiposProdutos
{
    public class TiposProdutosAppService : AppService<IList<TiposProdutosDataResponse>>,
        IRequestHandler<TiposProdutosRequest, IResponseAppService<IList<TiposProdutosDataResponse>>>
    {
        private readonly ITipoProdutoRepository _tipoProdutoRepository;

        public TiposProdutosAppService(
            ITipoProdutoRepository tipoProdutoRepository,
            DependenciesAppService dependenciesAppService) : base(dependenciesAppService)
        {
            _tipoProdutoRepository = tipoProdutoRepository;
        }

        public async Task<IResponseAppService<IList<TiposProdutosDataResponse>>>
            Handle(TiposProdutosRequest request, CancellationToken cancellationToken)
        {
            if (request.Validate() == false)
            {
                return ReturnNotifications(request.Notifications);
            }

            IQueryable<Entities.TipoProduto> tiposProduto =
                _tipoProdutoRepository
                    .GetEntity()
                    .AsQueryable();

            if (request.IdTipoProdutoSuperior.HasValue)
            {
                tiposProduto = tiposProduto
                    .Where(tipo => tipo.TipoProdutoSuperiorId == request.IdTipoProdutoSuperior);
            }

            var tiposProdutoRetorno =
                await tiposProduto
                    .Select(tipo => new TiposProdutosDataResponse
                    {
                        IdTipoProduto = tipo.Id,
                        NomeTipoProduto = tipo.Nome,
                        IdTipoProdutoSuperior = tipo.TipoProdutoSuperiorId
                    })
                    .ToListAsync();

            return ReturnData(tiposProdutoRetorno);
        }
    }
}
