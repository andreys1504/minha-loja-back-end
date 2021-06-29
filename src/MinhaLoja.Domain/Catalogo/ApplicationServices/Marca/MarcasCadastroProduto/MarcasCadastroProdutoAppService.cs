using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Domain.Catalogo.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.Marca.MarcasCadastroProduto
{
    public class MarcasCadastroProdutoAppService : AppService<IList<MarcasCadastroProdutoDataResponse>>,
        IRequestHandler<MarcasCadastroProdutoRequest, IResponseAppService<IList<MarcasCadastroProdutoDataResponse>>>
    {
        private readonly IMarcaRepository _marcaRepository;

        public MarcasCadastroProdutoAppService(
            IMarcaRepository marcaRepository,
            DependenciesAppService dependenciesAppService)
            : base(dependenciesAppService)
        {
            _marcaRepository = marcaRepository;
        }

        public async Task<IResponseAppService<IList<MarcasCadastroProdutoDataResponse>>>
            Handle(MarcasCadastroProdutoRequest request, CancellationToken cancellationToken)
        {
            if (request.Validate() == false)
            {
                return ReturnNotifications(request.Notifications);
            }

            var marcas =
                await _marcaRepository
                    .GetEntity()
                    .Select(marca => new MarcasCadastroProdutoDataResponse
                    {
                        IdMarca = marca.Id,
                        NomeMarca = marca.Nome
                    })
                    .ToListAsync();

            return ReturnData(marcas);
        }
    }
}
