using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Core.Domain.Exceptions;
using MinhaLoja.Domain.Catalogo.Entities;
using MinhaLoja.Domain.Catalogo.Events.Produto.Cadastro;
using MinhaLoja.Domain.Catalogo.Queries;
using MinhaLoja.Domain.Catalogo.Repositories;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MensagensProduto = MinhaLoja.Domain.MessagesDomain.Catalogo;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.Produto.Cadastro
{
    public class CadastroProdutoAppService : AppService<CadastroProdutoDataResponse>,
        IRequestHandler<CadastroProdutoRequest, IResponseAppService<CadastroProdutoDataResponse>>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMarcaRepository _marcaRepository;
        private readonly ITipoProdutoRepository _tipoProdutoRepository;
        private readonly IVendedorRepository _vendedorRepository;

        public CadastroProdutoAppService(
            IProdutoRepository produtoRepository,
            IMarcaRepository marcaRepository,
            ITipoProdutoRepository tipoProdutoRepository,
            IVendedorRepository vendedorRepository,
            DependenciesAppService dependenciesAppService)
            : base(dependenciesAppService)
        {
            _produtoRepository = produtoRepository;
            _marcaRepository = marcaRepository;
            _tipoProdutoRepository = tipoProdutoRepository;
            _vendedorRepository = vendedorRepository;
        }

        public async Task<IResponseAppService<CadastroProdutoDataResponse>> Handle(
            CadastroProdutoRequest request,
            CancellationToken cancellationToken)
        {
            if (!request.Validate())
                return ReturnNotifications(request.Notifications);


            IResponseAppService<CadastroProdutoDataResponse> validacoesItensVinculadoProduto =
                ValidarItensVinculadoProduto(request: request);

            if (validacoesItensVinculadoProduto != null)
                return validacoesItensVinculadoProduto;


            var produto = new Entities.Produto(
                nome: request.NomeProduto,
                idMarca: request.IdMarca,
                valor: request.Valor,
                descricao: request.DescricaoProduto,
                idExterno: request.IdExterno,
                caracteristicasProduto: request.CaracteristicaProduto,
                idTipoProduto: request.IdTipoProduto,
                idVendedor: request.IdVendedor,
                idUsuario: request.IdUsuario);

            if (produto.IsValid is false)
                return ReturnNotifications(produto.Notifications);

            await _produtoRepository.AddEntityAsync(produto);

            if (await CommitAsync())
            {
                await PublishEventAsync(
                    @event: new ProdutoCadastradoEvent(
                        idProduto: produto.Id,
                        nomeProduto: produto.Nome,
                        valor: produto.ValorAtual,
                        idMarca: produto.MarcaId,
                        descricaoProduto: produto.Descricao,
                        idExterno: produto.IdExterno,
                        idTipoProduto: produto.TipoProdutoId,
                        idVendedor: produto.VendedorId,
                        caracteristicaProduto:
                            produto
                            .CaracteristicasProduto
                            .Select(caracteristica =>
                                (caracteristica.Id, caracteristica.Descricao)
                             )
                        ),
                    aggregateRoot: produto
                );

                var produtoCadastrado = new CadastroProdutoDataResponse
                {
                    IdProduto = produto.Id,
                    NomeProduto = produto.Nome,
                    ValorAtual = produto.ValorAtual,
                    IdMarca = produto.MarcaId,
                    DescricaoProduto = produto.Descricao,
                    IdExterno = produto.IdExterno,
                    IdTipoProduto = produto.TipoProdutoId
                };

                if (produto.CaracteristicasProduto?.Count > 0)
                    produtoCadastrado.CaracteristicasProduto =
                        produto.CaracteristicasProduto
                            .Select(caracteristica => caracteristica.Descricao);

                return ReturnData(produtoCadastrado);
            }

            throw new DomainException("erro na realização do cadastro do Produto");
        }


        private IResponseAppService<CadastroProdutoDataResponse>
            ValidarItensVinculadoProduto(CadastroProdutoRequest request)
        {
            bool marcaExistente =
                _marcaRepository
                    .GetEntity()
                    .Any(marca => marca.Id == request.IdMarca);

            if (marcaExistente is false)
                return ReturnNotification(nameof(request.IdMarca), MensagensProduto.Produto_Cadastro_IdMarcaInvalida);


            var tipoProduto =
                _tipoProdutoRepository
                    .GetEntity()
                    .Where(TipoProdutoQueries.TipoProdutoValido())
                    .Select(tipoProduto => new
                    {
                        tipoProduto.Id,
                        tipoProduto.NumeroOrdemHierarquiaGrupo,
                        tipoProduto.CodigoGrupoTipoProduto
                    })
                    .FirstOrDefault(tipoProduto => tipoProduto.Id == request.IdTipoProduto);

            if (tipoProduto == null)
                return ReturnNotification(nameof(request.IdTipoProduto), MensagensProduto.Produto_Cadastro_IdTipoProdutoInvalido);


            if (request.IdVendedor.HasValue)
            {
                bool vendedor =
                    _vendedorRepository
                        .GetEntity()
                        .Any(vendedor => vendedor.Id == request.IdVendedor.Value);

                if (vendedor is false)
                    return ReturnNotification(nameof(request.IdVendedor), MensagensProduto.Produto_Cadastro_IdVendedorInvalido);
            }


            bool produtoExistente =
                _produtoRepository
                    .GetEntity()
                    .Any(ProdutoQueries.ProdutoExistenteSistemaParaCadastro(
                        nomeProduto: request.NomeProduto,
                        idMarca: request.IdMarca,
                        idVendedor: request.IdVendedor)
                    );

            if (produtoExistente)
                return ReturnNotification(nameof(request.NomeProduto), MensagensProduto.Produto_Cadastro_ProdutoJaCadastradaSistema);


            foreach (var (idCaracteristicaProduto, _) in request.CaracteristicaProduto)
            {
                var caracteristicaTpProduto =
                    _tipoProdutoRepository
                        .GetEntityAggregate<TipoProdutoCaracteristica>()
                        .Include(caracteristicaTpProduto => caracteristicaTpProduto.TipoProduto)
                        .Where(caracteristicaTpProduto => caracteristicaTpProduto.Id == idCaracteristicaProduto)
                        .Where(TipoProdutoQueries.ValidarCaracteristicaCadastroProduto(
                            codigoGrupoTipoProduto: tipoProduto.CodigoGrupoTipoProduto,
                            numeroOrdemHierarquiaGrupo: tipoProduto.NumeroOrdemHierarquiaGrupo)
                        )
                        .Select(caracteristicaTpProduto => new
                        {
                            caracteristicaTpProduto.Id,
                            caracteristicaTpProduto.TipoProduto.CodigoGrupoTipoProduto,
                            caracteristicaTpProduto.TipoProduto.NumeroOrdemHierarquiaGrupo
                        })
                        .FirstOrDefault();

                if (caracteristicaTpProduto == null)
                    return ReturnNotification(nameof(request.CaracteristicaProduto), MensagensProduto.Produto_Cadastro_CaracteristicaProdutoInvalida);
            }


            return null;
        }
    }
}
