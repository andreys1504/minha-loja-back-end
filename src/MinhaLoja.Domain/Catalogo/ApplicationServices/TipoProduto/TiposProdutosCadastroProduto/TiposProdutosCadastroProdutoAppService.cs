using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Domain.Catalogo.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.TiposProdutosCadastroProduto
{
    public class TiposProdutosAppService : AppService<IList<TiposProdutosCadastroProdutoDataResponse>>,
        IRequestHandler<TiposProdutosCadastroProdutoRequest, IResponseAppService<IList<TiposProdutosCadastroProdutoDataResponse>>>
    {
        private readonly ITipoProdutoRepository _tipoProdutoRepository;

        public TiposProdutosAppService(
            ITipoProdutoRepository tipoProdutoRepository,
            DependenciesAppService dependenciesAppService) : base(dependenciesAppService)
        {
            _tipoProdutoRepository = tipoProdutoRepository;
        }

        public async Task<IResponseAppService<IList<TiposProdutosCadastroProdutoDataResponse>>>
            Handle(TiposProdutosCadastroProdutoRequest request, CancellationToken cancellationToken)
        {
            if (request.Validate() == false)
            {
                return ReturnNotifications(request.Notifications);
            }

            IQueryable<Entities.TipoProduto> tiposProduto =
                _tipoProdutoRepository
                    .GetEntity()
                    .Where(tipo => tipo.Ativo == true)
                    .AsQueryable();

            if (request.IdTipoProdutoSuperior.HasValue)
            {
                tiposProduto = tiposProduto
                    .Where(tipo => tipo.TipoProdutoSuperiorId == request.IdTipoProdutoSuperior);
            }

            var tiposProdutoRetorno =
                await tiposProduto
                    .Select(tipo => new TiposProdutosCadastroProdutoDataResponse
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
