using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Domain.Catalogo.ApplicationServices.TipoProduto.Cadastro;
using MinhaLoja.Domain.Catalogo.Entities;
using MinhaLoja.Tests.Fakes.Domain.ApplicationServices.Service;
using MinhaLoja.Tests.Fakes.Infra.Data.Repositories.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace MinhaLoja.Domain.Tests.Catalogo.ApplicationServices.TipoProduto.Cadastro
{
    public class CadastroTipoProdutoAppServiceTests
    {
        [Fact(DisplayName = "Sucesso Cadastro Tipo Produto")]
        [Trait("Domain", nameof(CadastroTipoProdutoAppService))]
        public void RetornarSucesso()
        {
            //arrange
            var caracteristicas = new List<(string nome, string observacao)>
            {
                ("Quantidade de Portas", null)
            };
            var request = new CadastroTipoProdutoRequest(
                nomeTipoProduto: "Carro",
                idTipoProdutoSuperior: 5,
                caracteristicasTipoProduto: caracteristicas,
                idUsuario: Guid.NewGuid()
            );

            var tiposProduto = new List<Domain.Catalogo.Entities.TipoProduto>();
            var tipoProdutoVeiculo = new Domain.Catalogo.Entities.TipoProduto(
                nome: "Veiculo",
                idTipoProdutoSuperior: null,
                codigoGrupoTipoProduto: Guid.NewGuid(),
                numeroAtualOrdemHierarquiaGrupo: 1,
                caracteristicasTipoProduto: new List<(string, string)>() { ("Cor", null) },
                idUsuario: new Guid()
            );
            tipoProdutoVeiculo.AlterarId(5);
            tiposProduto.Add(tipoProdutoVeiculo);

            var caracteristicasTipoProduto = new List<TipoProdutoCaracteristica>();

            var appService = new CadastroTipoProdutoAppService(
                tipoProdutoRepository: new TipoProdutoRepositoryFake(
                    entities: tiposProduto,
                    entitiesAggregate: caracteristicasTipoProduto.AsQueryable()),
                dependenciesAppService: new DependenciesAppServiceFake());

            //act
            IResponseAppService<CadastroTipoProdutoDataResponse> response = 
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.True(response.Success);
            Assert.Equal(request.NomeTipoProduto, response.Data.NomeTipoProduto);
            Assert.Equal(request.IdTipoProdutoSuperior, response.Data.IdTipoProdutoSuperior);
            Assert.Equal(request.CaracteristicasTipoProduto.Count, response.Data.CaracteristicasTipoProduto.Count);

            bool caracteristicaInexistente = false;
            foreach (var caracteristica in response.Data.CaracteristicasTipoProduto)
                if (request.CaracteristicasTipoProduto.Any(c => c.nome == caracteristica.Nome) is false)
                {
                    caracteristicaInexistente = true;
                    break;
                }

            Assert.False(caracteristicaInexistente);
        }


        [Fact(DisplayName = "Caracteristicas Enviadas Já Existirem No Grupo do Tipo de Produto")]
        [Trait("Domain", nameof(CadastroTipoProdutoAppService))]
        public void RetornarNotificacaoQuandoAsCaracteristicasEnviadasJaExistiremNoGrupoTipoProduto()
        {
            //arrange
            var caracteristicas = new List<(string nome, string observacao)>
            {
                ("Cor", null) //caracteristica já existente
            };
            var request = new CadastroTipoProdutoRequest(
                nomeTipoProduto: "Carro",
                idTipoProdutoSuperior: 5,
                caracteristicasTipoProduto: caracteristicas,
                idUsuario: Guid.NewGuid()
            );

            var tiposProduto = new List<Domain.Catalogo.Entities.TipoProduto>();
            var tipoProdutoVeiculo = new Domain.Catalogo.Entities.TipoProduto(
                nome: "Veiculo",
                idTipoProdutoSuperior: null,
                codigoGrupoTipoProduto: Guid.NewGuid(),
                numeroAtualOrdemHierarquiaGrupo: 1,
                caracteristicasTipoProduto: new List<(string, string)>() { ("Cor", null) },
                idUsuario: new Guid()
            );
            tipoProdutoVeiculo.AlterarId(5);
            tiposProduto.Add(tipoProdutoVeiculo);

            var caracteristicasTipoProduto = new List<TipoProdutoCaracteristica>();

            var appService = new CadastroTipoProdutoAppService(
                tipoProdutoRepository: new TipoProdutoRepositoryFake(
                    entities: tiposProduto,
                    entitiesAggregate: caracteristicasTipoProduto.AsQueryable()),
                dependenciesAppService: new DependenciesAppServiceFake());

            //act
            IResponseAppService<CadastroTipoProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
        }


        [Fact(DisplayName = "IdTipoProdutoSuperior inexistente")]
        [Trait("Domain", nameof(CadastroTipoProdutoAppService))]
        public void RetornarNotificacaoQuandoIdTipoProdutoSuperiorNaoExistir()
        {
            //arrange
            var caracteristicas = new List<(string nome, string observacao)>
            {
                ("Quantidade de portas", null)
            };
            var request = new CadastroTipoProdutoRequest(
                nomeTipoProduto: "Carro",
                idTipoProdutoSuperior: 6,
                caracteristicasTipoProduto: caracteristicas,
                idUsuario: Guid.NewGuid()
            );

            var tiposProduto = new List<Domain.Catalogo.Entities.TipoProduto>();
            var tipoProdutoVeiculo = new Domain.Catalogo.Entities.TipoProduto(
                nome: "Veiculo",
                idTipoProdutoSuperior: null,
                codigoGrupoTipoProduto: Guid.NewGuid(),
                numeroAtualOrdemHierarquiaGrupo: 1,
                caracteristicasTipoProduto: new List<(string, string)>() { ("Cor", null) },
                idUsuario: new Guid()
            );
            tipoProdutoVeiculo.AlterarId(1);
            tiposProduto.Add(tipoProdutoVeiculo);

            var caracteristicasTipoProduto = new List<TipoProdutoCaracteristica>();

            var appService = new CadastroTipoProdutoAppService(
                tipoProdutoRepository: new TipoProdutoRepositoryFake(
                    entities: tiposProduto,
                    entitiesAggregate: caracteristicasTipoProduto.AsQueryable()),
                dependenciesAppService: new DependenciesAppServiceFake());

            //act
            IResponseAppService<CadastroTipoProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
        }


        #region Validar Request 

        [Theory(DisplayName = "Request: " + nameof(CadastroTipoProdutoRequest.NomeTipoProduto))]
        [Trait("Domain", nameof(CadastroTipoProdutoAppService))]
        [InlineData(null, nameof(CadastroTipoProdutoRequest.NomeTipoProduto))]
        [InlineData("", nameof(CadastroTipoProdutoRequest.NomeTipoProduto))]
        [InlineData("Ca", nameof(CadastroTipoProdutoRequest.NomeTipoProduto))] //mínimo de 3 características
        [InlineData("Carro carro carro carro Carro carro carro carro Carro carro carro1",
            nameof(CadastroTipoProdutoRequest.NomeTipoProduto))] //máximo de 65 características
        public void RetornarNotificacoesQuandoNomeTipoProdutoInvalidoForInformado(string nomeTipoProduto, string notificacaoEsperada)
        {
            //arrange
            var caracteristicas = new List<(string nome, string observacao)>
            {
                ("Quantidade de Portas", null)
            };
            var request = new CadastroTipoProdutoRequest(
                nomeTipoProduto: nomeTipoProduto,
                idTipoProdutoSuperior: null,
                caracteristicasTipoProduto: caracteristicas,
                idUsuario: Guid.NewGuid()
            );

            var tiposProduto = new List<Domain.Catalogo.Entities.TipoProduto>();
            var caracteristicasTipoProduto = new List<TipoProdutoCaracteristica>();

            var appService = new CadastroTipoProdutoAppService(
                tipoProdutoRepository: new TipoProdutoRepositoryFake(
                    entities: tiposProduto,
                    entitiesAggregate: caracteristicasTipoProduto.AsQueryable()),
                dependenciesAppService: new DependenciesAppServiceFake());

            //act
            IResponseAppService<CadastroTipoProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == notificacaoEsperada);
        }


        [Theory(DisplayName = "Request: " + nameof(CadastroTipoProdutoRequest.IdTipoProdutoSuperior))]
        [Trait("Domain", nameof(CadastroTipoProdutoAppService))]
        [InlineData(0, nameof(CadastroTipoProdutoRequest.IdTipoProdutoSuperior))]
        [InlineData(-1, nameof(CadastroTipoProdutoRequest.IdTipoProdutoSuperior))]
        public void RetornarNotificacoesQuandoIdTipoProdutoSuperiorInvalidoForInformado(int idTipoProdutoSuperior, string notificacaoEsperada)
        {
            //arrange
            var caracteristicas = new List<(string nome, string observacao)>
            {
                ("Quantidade de Portas", null)
            };
            var request = new CadastroTipoProdutoRequest(
                nomeTipoProduto: "Carro",
                idTipoProdutoSuperior: idTipoProdutoSuperior,
                caracteristicasTipoProduto: caracteristicas,
                idUsuario: Guid.NewGuid()
            );

            var tiposProduto = new List<Domain.Catalogo.Entities.TipoProduto>();
            var caracteristicasTipoProduto = new List<TipoProdutoCaracteristica>();

            var appService = new CadastroTipoProdutoAppService(
                tipoProdutoRepository: new TipoProdutoRepositoryFake(
                    entities: tiposProduto,
                    entitiesAggregate: caracteristicasTipoProduto.AsQueryable()),
                dependenciesAppService: new DependenciesAppServiceFake());

            //act
            IResponseAppService<CadastroTipoProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == notificacaoEsperada);
        }


        [Theory(DisplayName = "Request: " + nameof(CadastroTipoProdutoRequest.CaracteristicasTipoProduto))]
        [Trait("Domain", nameof(CadastroTipoProdutoAppService))]
        [InlineData(null, nameof(CadastroTipoProdutoRequest.CaracteristicasTipoProduto))]
        public void RetornarNotificacoesQuandoCaracteristicasTipoProdutoNaoForInformado(
            IList<(string nome, string observacao)> caracteristicasTipoProduto, 
            string notificacaoEsperada)
        {
            //arrange
            var request = new CadastroTipoProdutoRequest(
                nomeTipoProduto: "Carro",
                idTipoProdutoSuperior: null,
                caracteristicasTipoProduto: caracteristicasTipoProduto,
                idUsuario: Guid.NewGuid()
            );

            var appService = new CadastroTipoProdutoAppService(
                tipoProdutoRepository: new TipoProdutoRepositoryFake(
                    entities: new List<Domain.Catalogo.Entities.TipoProduto>(),
                    entitiesAggregate: new List<TipoProdutoCaracteristica>().AsQueryable()),
                dependenciesAppService: new DependenciesAppServiceFake());

            //act
            IResponseAppService<CadastroTipoProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == notificacaoEsperada);
        }


        [Fact(DisplayName = "Request: " + nameof(CadastroTipoProdutoRequest.CaracteristicasTipoProduto))]
        [Trait("Domain", nameof(CadastroTipoProdutoAppService))]
        public void RetornarNotificacoesQuandoNenhumaCaracteristicaForInformada()
        {
            //arrange
            var request = new CadastroTipoProdutoRequest(
                nomeTipoProduto: "Carro",
                idTipoProdutoSuperior: null,
                caracteristicasTipoProduto: new List<(string nome, string observacao)>(),
                idUsuario: Guid.NewGuid()
            );

            var appService = new CadastroTipoProdutoAppService(
                tipoProdutoRepository: new TipoProdutoRepositoryFake(
                    entities: new List<Domain.Catalogo.Entities.TipoProduto>(),
                    entitiesAggregate: new List<TipoProdutoCaracteristica>().AsQueryable()),
                dependenciesAppService: new DependenciesAppServiceFake());

            //act
            IResponseAppService<CadastroTipoProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == nameof(CadastroTipoProdutoRequest.CaracteristicasTipoProduto));
        }

        #endregion
    }
}
