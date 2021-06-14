using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Core.Domain.Exceptions;
using MinhaLoja.Domain.Catalogo.Entities;
using MinhaLoja.Domain.Catalogo.Events.TipoProduto.Cadastro;
using MinhaLoja.Domain.Catalogo.Queries;
using MinhaLoja.Domain.Catalogo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MensagensTipoProduto = MinhaLoja.Domain.MessagesDomain.Catalogo;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.Cadastro
{
    public class CadastroTipoProdutoAppService : AppService<CadastroTipoProdutoDataResponse>,
        IRequestHandler<CadastroTipoProdutoRequest, IResponseAppService<CadastroTipoProdutoDataResponse>>
    {
        private readonly ITipoProdutoRepository _tipoProdutoRepository;

        public CadastroTipoProdutoAppService(
            ITipoProdutoRepository tipoProdutoRepository,
            DependenciesAppService dependenciesAppService)
            : base(dependenciesAppService)
        {
            _tipoProdutoRepository = tipoProdutoRepository;
        }

        public async Task<IResponseAppService<CadastroTipoProdutoDataResponse>> Handle(
            CadastroTipoProdutoRequest request,
            CancellationToken cancellationToken)
        {
            if (!request.Validate())
                return ReturnNotifications(request.Notifications);


            Guid? codigoGrupoTipoProduto = null;
            int? numeroAtualOrdemHierarquiaGrupo = null;
            if (request.IdTipoProdutoSuperior.HasValue)
            {
                IResponseAppService<CadastroTipoProdutoDataResponse> validacaoTipoProdutoSuperior =
                    ValidarTipoProdutoSuperior(
                        idTipoProdutoSuperior: request.IdTipoProdutoSuperior.Value,
                        codigoGrupoTipoProduto: out codigoGrupoTipoProduto,
                        numeroAtualOrdemHierarquiaGrupo: out numeroAtualOrdemHierarquiaGrupo);

                if (validacaoTipoProdutoSuperior != null)
                    return validacaoTipoProdutoSuperior;
            }


            IResponseAppService<CadastroTipoProdutoDataResponse> validacaoCaracteristicas = ValidarCaracteristicas(
                caracteristicasTipoProduto: request.CaracteristicasTipoProduto,
                codigoGrupoTipoProduto: codigoGrupoTipoProduto);

            if (validacaoCaracteristicas != null)
                return validacaoCaracteristicas;


            bool tipoProdutoExistente =
                _tipoProdutoRepository
                    .GetEntity()
                    .Any(TipoProdutoQueries.TipoProdutoExistenteSistema(
                        nomeTipoProduto: request.NomeTipoProduto,
                        idTipoProdutoSuperior: request.IdTipoProdutoSuperior)
                    );

            if (tipoProdutoExistente)
                return ReturnNotification(nameof(request.NomeTipoProduto), MensagensTipoProduto.TipoProduto_Cadastro_TipoProdutoExistente);

            Entities.TipoProduto tipoProduto = await SalvarTipoProduto(
                request: request,
                codigoGrupoTipoProduto: codigoGrupoTipoProduto,
                numeroAtualOrdemHierarquiaGrupo: numeroAtualOrdemHierarquiaGrupo);

            if (tipoProduto == null)
                throw new DomainException("erro na realização do cadastro do Tipo de Produto");

            if (tipoProduto.IsValid is false)
                return ReturnNotifications(tipoProduto.Notifications);


            return ReturnData(ConfigurarRetorno(tipoProduto));
        }


        private IResponseAppService<CadastroTipoProdutoDataResponse> ValidarTipoProdutoSuperior(
            int idTipoProdutoSuperior,
            out Guid? codigoGrupoTipoProduto,
            out int? numeroAtualOrdemHierarquiaGrupo)
        {
            codigoGrupoTipoProduto = null;
            numeroAtualOrdemHierarquiaGrupo = null;

            var tipoProdutoSuperior =
                    _tipoProdutoRepository
                        .GetEntity()
                        .Select(tipoProduto => new
                        {
                            tipoProduto.Id,
                            tipoProduto.CodigoGrupoTipoProduto,
                            tipoProduto.NumeroOrdemHierarquiaGrupo
                        })
                        .FirstOrDefault(tipoProduto => tipoProduto.Id == idTipoProdutoSuperior);

            if (tipoProdutoSuperior == null)
                return ReturnNotification(
                    nameof(idTipoProdutoSuperior), MensagensTipoProduto.TipoProduto_Cadastro_TipoProdutoSuperiorNulo
                );

            codigoGrupoTipoProduto = tipoProdutoSuperior.CodigoGrupoTipoProduto;
            numeroAtualOrdemHierarquiaGrupo = tipoProdutoSuperior.NumeroOrdemHierarquiaGrupo;


            return null;
        }

        private IResponseAppService<CadastroTipoProdutoDataResponse> ValidarCaracteristicas(
            IList<(string nome, string observacao)> caracteristicasTipoProduto,
            Guid? codigoGrupoTipoProduto)
        {
            IEnumerable<string> nomesCaracteristicasParaCadastro =
                caracteristicasTipoProduto
                    .Select(caracteristica => caracteristica.nome);

            if (TipoProdutoQueries.CaracteristicasIguaisExistentes(nomesCaracteristicasParaCadastro))
                return ReturnNotification(nameof(caracteristicasTipoProduto), MensagensTipoProduto.TipoProduto_Cadastro_CaracteristicasRepetidasEnviada);

            if (codigoGrupoTipoProduto.HasValue is false)
                return null;

            var caracteristicasGrupoTipoProduto =
                _tipoProdutoRepository
                    .GetEntity()
                        .Include(tipoProduto => tipoProduto.CaracteristicasTipoProduto)
                        .Where(tipoProduto => tipoProduto.CodigoGrupoTipoProduto == codigoGrupoTipoProduto.Value)
                        .Select(tipoProduto => new
                        {
                            Caracteristicas = tipoProduto.CaracteristicasTipoProduto.Select(caracteristica => caracteristica.Nome)
                        })
                        .ToList();

            foreach (var caracteristicas in caracteristicasGrupoTipoProduto)
                if (TipoProdutoQueries.CaracteristicasIguaisExistentes(
                    nomesCaracteristicasExistentes: caracteristicas.Caracteristicas,
                    nomesCaracteristicasComparacao: nomesCaracteristicasParaCadastro)
                )
                    return ReturnNotification(nameof(caracteristicasTipoProduto), MensagensTipoProduto.TipoProduto_Cadastro_CaracteristicasExistentesNoSistema);

            return null;
        }

        private async Task<Entities.TipoProduto> SalvarTipoProduto(
            CadastroTipoProdutoRequest request,
            Guid? codigoGrupoTipoProduto,
            int? numeroAtualOrdemHierarquiaGrupo)
        {
            var tipoProduto = new Entities.TipoProduto(
                nome: request.NomeTipoProduto,
                idTipoProdutoSuperior: request.IdTipoProdutoSuperior,
                codigoGrupoTipoProduto: codigoGrupoTipoProduto,
                numeroAtualOrdemHierarquiaGrupo: numeroAtualOrdemHierarquiaGrupo,
                caracteristicasTipoProduto: request.CaracteristicasTipoProduto,
                idUsuario: request.IdUsuario);

            if (tipoProduto.IsValid is false)
                return tipoProduto;

            await _tipoProdutoRepository.AddEntityAsync(tipoProduto);

            if (await CommitAsync())
            {
                await PublishEventAsync(
                    @event: new TipoProdutoCadastradoEvent(
                        idTipoProduto: tipoProduto.Id,
                        nomeTipoProduto: tipoProduto.Nome,
                        idTipoProdutoSuperior: tipoProduto.TipoProdutoSuperiorId,
                        caracteristicasTipoProduto:
                            tipoProduto
                            .CaracteristicasTipoProduto
                            .Select(tipoProduto =>
                                (tipoProduto.Id, tipoProduto.Nome, tipoProduto.Observacao)
                             )
                    ),
                    aggregateRoot: tipoProduto
                );

                return tipoProduto;
            }

            return null;
        }

        private CadastroTipoProdutoDataResponse ConfigurarRetorno(Entities.TipoProduto tipoProduto)
        {
            var retorno = new CadastroTipoProdutoDataResponse
            {
                IdTipoProduto = tipoProduto.Id,
                NomeTipoProduto = tipoProduto.Nome,
                IdTipoProdutoSuperior = tipoProduto.TipoProdutoSuperiorId
            };

            foreach (TipoProdutoCaracteristica caracteristica in tipoProduto.CaracteristicasTipoProduto)
                retorno.CaracteristicasTipoProduto.Add(new DataResponse.CaracteristicaTipoProduto
                {
                    Id = caracteristica.Id,
                    Nome = caracteristica.Nome,
                    Observacao = caracteristica.Observacao
                });
            return retorno;
        }
    }
}
