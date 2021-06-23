using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Domain.Catalogo.ApplicationServices.Produto.Cadastro;
using MinhaLoja.Domain.Catalogo.Entities;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Entities;
using MinhaLoja.Tests.Fakes.Domain.ApplicationServices.Service;
using MinhaLoja.Tests.Fakes.Infra.Data.Repositories.Catalogo;
using MinhaLoja.Tests.Fakes.Infra.Data.Repositories.ContaUsuarioAdministrador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace MinhaLoja.Domain.Tests.Catalogo.ApplicationServices.Produto.Cadastro
{
    public class CadastroProdutoAppServiceTests
    {
        [Fact(DisplayName = "Sucesso cadastro produto")]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        public void RetornarSucesso()
        {
            //arrange
            int idTipoProduto = 2;
            var caracteristicaTempMaxima = new TipoProdutoCaracteristica(
                    nome: "Temperatura Máxima (°C)",
                    observacao: null,
                    idTipoProduto: idTipoProduto,
                    idUsuario: Guid.NewGuid()
            );
            caracteristicaTempMaxima.AlterarId(1);

            var caracteristicaTensao = new TipoProdutoCaracteristica(
                    nome: "Tensão/Voltagem",
                    observacao: null,
                    idTipoProduto: idTipoProduto,
                    idUsuario: Guid.NewGuid()
            );
            caracteristicaTensao.AlterarId(5);


            var request = new CadastroProdutoRequest(
                nomeProduto: "Fritadeira Elétrica Sem Óleo Air Fryer Mondial",
                valor: (decimal)189.90,
                idMarca: 1,
                descricaoProduto: "Faça pratos suculentos e sem sujar a cozinha com a Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF-31. Com capacidade para até 3,5 litros , você prepara batatas palitos congeladas em até 12 minutos, livre de óleo e muito mais crocantes.",
                idExterno: null,
                idTipoProduto: idTipoProduto,
                caracteristicaProduto: new List<(int idCaracteristicaProduto, string descricao)>()
                {
                    (caracteristicaTempMaxima.Id, "200º C"),
                    (caracteristicaTensao.Id, "220V")
                },
                idVendedor: 1,
                idUsuario: Guid.NewGuid()
            );


            #region Marca

            var marcas = new List<Domain.Catalogo.Entities.Marca>();
            var marcaMondial = new Domain.Catalogo.Entities.Marca(
                nome: "Mondial",
                idUsuario: Guid.NewGuid()
            );
            marcaMondial.AlterarId(request.IdMarca);
            marcas.Add(marcaMondial);

            #endregion


            #region TipoProduto

            Guid codigoGrupoTipoProduto = Guid.NewGuid();
            var tiposProdutos = new List<Domain.Catalogo.Entities.TipoProduto>();
            var tipoProdutoFritadeira = new Domain.Catalogo.Entities.TipoProduto(
                nome: "Fritadeira Elétrica",
                idTipoProdutoSuperior: null,
                codigoGrupoTipoProduto: codigoGrupoTipoProduto,
                numeroAtualOrdemHierarquiaGrupo: 1,
                caracteristicasTipoProduto: new List<(string, string)>(),
                idUsuario: Guid.NewGuid()
            );
            tipoProdutoFritadeira.AlterarId(request.IdTipoProduto);

            caracteristicaTempMaxima.VincularTipoProduto(tipoProdutoFritadeira);
            caracteristicaTensao.VincularTipoProduto(tipoProdutoFritadeira);

            tipoProdutoFritadeira.CaracteristicasTipoProduto.Add(caracteristicaTempMaxima);
            tipoProdutoFritadeira.CaracteristicasTipoProduto.Add(caracteristicaTensao);
            tiposProdutos.Add(tipoProdutoFritadeira);

            var tiposProdutosCaracteristicas = new List<TipoProdutoCaracteristica>()
            {
                caracteristicaTempMaxima,
                caracteristicaTensao
            };

            #endregion


            #region Vendedor

            var vendedores = new List<Vendedor>();
            var vendedor01 = new Vendedor(
                email: "andreys1504",
                cnpj: "45.379.876/0001-29",
                idUsuario: 2
            );
            vendedor01.AlterarId(1);
            vendedores.Add(vendedor01);

            #endregion


            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(new List<Domain.Catalogo.Entities.Produto>()),
                    marcaRepository: new MarcaRepositoryFake(marcas),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(tiposProdutos, tiposProdutosCaracteristicas.AsQueryable()),
                    vendedorRepository: new VendedorRepositoryFake(vendedores),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.True(response.Success);
            Assert.Equal(request.NomeProduto, response.Data.NomeProduto);
            Assert.Equal(request.Valor, response.Data.ValorAtual);
            Assert.Equal(request.IdMarca, response.Data.IdMarca);
            Assert.Equal(request.DescricaoProduto, response.Data.DescricaoProduto);
            Assert.Equal(request.IdExterno, response.Data.IdExterno);
            Assert.Equal(request.IdTipoProduto, response.Data.IdTipoProduto);

            bool caracteristicaInexistente = false;
            foreach (string caracteristica in response.Data.CaracteristicasProduto)
                if (request.CaracteristicaProduto.Any(c => c.descricao == caracteristica) == false)
                {
                    caracteristicaInexistente = true;
                    break;
                }

            Assert.False(caracteristicaInexistente);
        }


        [Fact(DisplayName = "Cadastrar Produto Já Existente")]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        public void RetornarNotificacaoQuandoProdutoJaExistenteTentarSerCadastrado()
        {
            //arrange
            string descricao = @"
                Faça pratos suculentos e sem sujar a cozinha com a Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF-31. Com capacidade para até 3,5 litros , você prepara batatas palitos congeladas em até 12 minutos, livre de óleo e muito mais crocantes.
            ";

            int idTipoProduto = 2;
            var caracteristicaTempMaxima = new TipoProdutoCaracteristica(
                    nome: "Temperatura Máxima (°C)",
                    observacao: null,
                    idTipoProduto: idTipoProduto,
                    idUsuario: Guid.NewGuid());
            caracteristicaTempMaxima.AlterarId(1);

            var caracteristicaTensao = new TipoProdutoCaracteristica(
                    nome: "Tensão/Voltagem",
                    observacao: null,
                    idTipoProduto: idTipoProduto,
                    idUsuario: Guid.NewGuid());
            caracteristicaTensao.AlterarId(5);


            var request = new CadastroProdutoRequest(
                nomeProduto: "Fritadeira Elétrica Sem Óleo Air Fryer Mondial",
                valor: (decimal)89.90,
                idMarca: 1,
                descricaoProduto: descricao,
                idExterno: null,
                idTipoProduto: idTipoProduto,
                caracteristicaProduto: new List<(int idCaracteristicaProduto, string descricao)>()
                {
                    (caracteristicaTempMaxima.Id, "200º C"),
                    (caracteristicaTensao.Id, "220V")
                },
                idVendedor: 1,
                idUsuario: Guid.NewGuid()
            );

            var marcas = new List<Domain.Catalogo.Entities.Marca>();
            var marcaMondial = new Domain.Catalogo.Entities.Marca(
                nome: "Mondial",
                idUsuario: Guid.NewGuid());
            marcaMondial.AlterarId(request.IdMarca);
            marcas.Add(marcaMondial);

            var tiposProdutos = new List<Domain.Catalogo.Entities.TipoProduto>();
            var tipoProdutoFritadeira = new Domain.Catalogo.Entities.TipoProduto(
                nome: "Fritadeira Elétrica",
                idTipoProdutoSuperior: null,
                codigoGrupoTipoProduto: null,
                numeroAtualOrdemHierarquiaGrupo: null,
                caracteristicasTipoProduto: new List<(string, string)>(),
                idUsuario: Guid.NewGuid());
            tipoProdutoFritadeira.AlterarId(request.IdTipoProduto);
            tiposProdutos.Add(tipoProdutoFritadeira);

            var tiposProdutosCaracteristicas = new List<TipoProdutoCaracteristica>()
            {
                caracteristicaTempMaxima,
                caracteristicaTensao
            };

            var produtos = new List<Domain.Catalogo.Entities.Produto>();
            var produto01 = new Domain.Catalogo.Entities.Produto(
                nome: request.NomeProduto,
                valor: (decimal)151.25,
                idMarca: marcaMondial.Id,
                descricao: descricao,
                idExterno: null,
                caracteristicasProduto: new List<(int idCaracteristicaProduto, string descricao)>()
                {
                    (caracteristicaTempMaxima.Id, "200º C")
                },
                idTipoProduto: idTipoProduto,
                idVendedor: request.IdVendedor,
                idUsuario: new Guid()
            );
            produtos.Add(produto01);

            var vendedores = new List<Vendedor>();
            var vendedor01 = new Vendedor(
                email: "andreys1504",
                cnpj: "45.379.876/0001-29",
                idUsuario: 2
            );
            vendedor01.AlterarId(produto01.VendedorId.Value);
            vendedores.Add(vendedor01);


            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(produtos),
                    marcaRepository: new MarcaRepositoryFake(marcas),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(tiposProdutos, tiposProdutosCaracteristicas.AsQueryable()),
                    vendedorRepository: new VendedorRepositoryFake(vendedores),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == nameof(request.NomeProduto));
        }


        [Fact(DisplayName = "Marca inexistente no sistema")]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        public void RetornarNotificacaoQuandoIdMarcaNaoCadastradaNoSistemaForInformado()
        {
            //arrange
            string descricao = @"
                Faça pratos suculentos e sem sujar a cozinha com a Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF-31. Com capacidade para até 3,5 litros , você prepara batatas palitos congeladas em até 12 minutos, livre de óleo e muito mais crocantes.
            ";

            var caracteristicas = new List<(int idCaracteristicaProduto, string descricao)>()
            {
                (1, "200º C"),
                (5, "220V")
            };

            var request = new CadastroProdutoRequest(
                nomeProduto: "Fritadeira Elétrica Sem Óleo Air Fryer Mondial",
                valor: (decimal)89.90,
                idMarca: 1,
                descricaoProduto: descricao,
                idExterno: null,
                idTipoProduto: 2,
                caracteristicaProduto: caracteristicas,
                idVendedor: null,
                idUsuario: Guid.NewGuid()
            );

            var marcas = new List<Domain.Catalogo.Entities.Marca>();
            var marcaMizuno = new Domain.Catalogo.Entities.Marca(
                nome: "Electrolux",
                idUsuario: Guid.NewGuid());
            marcaMizuno.AlterarId(2);
            marcas.Add(marcaMizuno);

            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(new List<Domain.Catalogo.Entities.Produto>()),
                    marcaRepository: new MarcaRepositoryFake(marcas),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(null, null),
                    vendedorRepository: new VendedorRepositoryFake(null),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == nameof(request.IdMarca));
        }


        [Fact(DisplayName = "Tipo produto inexistente no sistema")]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        public void RetornarNotificacaoQuandoIdTipoProdutoNaoCadastradaNoSistemaForInformado()
        {
            //arrange
            string descricao = @"
                Faça pratos suculentos e sem sujar a cozinha com a Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF-31. Com capacidade para até 3,5 litros , você prepara batatas palitos congeladas em até 12 minutos, livre de óleo e muito mais crocantes.
            ";

            var caracteristicas = new List<(int idCaracteristicaProduto, string descricao)>()
            {
                (1, "200º C"),
                (5, "220V")
            };

            var request = new CadastroProdutoRequest(
                nomeProduto: "Fritadeira Elétrica Sem Óleo Air Fryer Mondial",
                valor: (decimal)89.90,
                idMarca: 1,
                descricaoProduto: descricao,
                idExterno: null,
                idTipoProduto: 2,
                caracteristicaProduto: caracteristicas,
                idVendedor: null,
                idUsuario: Guid.NewGuid());

            var marcas = new List<Domain.Catalogo.Entities.Marca>();
            var marcaMondial = new Domain.Catalogo.Entities.Marca(
                 nome: "Mondial",
                 idUsuario: Guid.NewGuid()
            );
            marcaMondial.AlterarId(1);
            marcas.Add(marcaMondial);

            var tiposProduto = new List<Domain.Catalogo.Entities.TipoProduto>();
            var tiposProdutoFritadeira = new Domain.Catalogo.Entities.TipoProduto(
                nome: "Fritadeira",
                idTipoProdutoSuperior: null,
                codigoGrupoTipoProduto: null,
                numeroAtualOrdemHierarquiaGrupo: null,
                caracteristicasTipoProduto: new List<(string, string)>(),
                idUsuario: Guid.NewGuid()
            );
            tiposProdutoFritadeira.AlterarId(1);
            tiposProduto.Add(tiposProdutoFritadeira);

            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(new List<Domain.Catalogo.Entities.Produto>()),
                    marcaRepository: new MarcaRepositoryFake(marcas),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(tiposProduto),
                    vendedorRepository: new VendedorRepositoryFake(null),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == nameof(request.IdTipoProduto));
        }


        [Fact(DisplayName = "Caracteristicas Inexistentes Informadas")]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        public void RetornarNotificacaoQuandoCaracteristicasInexistentesForemInformadas()
        {
            //arrange
            string descricao = @"
                Faça pratos suculentos e sem sujar a cozinha com a Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF-31. Com capacidade para até 3,5 litros , você prepara batatas palitos congeladas em até 12 minutos, livre de óleo e muito mais crocantes.
            ";

            var caracteristicas = new List<(int idCaracteristicaProduto, string descricao)>()
            {
                (1, "200º C"),
                (5, "220V")
            };

            int idTipoProduto = 1;

            var request = new CadastroProdutoRequest(
                nomeProduto: "Fritadeira Elétrica Sem Óleo Air Fryer Mondial",
                valor: (decimal)89.90,
                idMarca: 1,
                descricaoProduto: descricao,
                idExterno: null,
                idTipoProduto: idTipoProduto,
                caracteristicaProduto: caracteristicas,
                idVendedor: null,
                idUsuario: Guid.NewGuid()
            );

            var marcas = new List<Domain.Catalogo.Entities.Marca>();
            var marcaMondial = new Domain.Catalogo.Entities.Marca(
                 nome: "Mondial",
                 idUsuario: Guid.NewGuid()
            );
            marcaMondial.AlterarId(request.IdMarca);
            marcas.Add(marcaMondial);

            var tiposProduto = new List<Domain.Catalogo.Entities.TipoProduto>();
            var tiposProdutoFritadeira = new Domain.Catalogo.Entities.TipoProduto(
                nome: "Fritadeira",
                idTipoProdutoSuperior: null,
                codigoGrupoTipoProduto: null,
                numeroAtualOrdemHierarquiaGrupo: null,
                caracteristicasTipoProduto: new List<(string, string)>(),
                idUsuario: Guid.NewGuid()
            );
            tiposProdutoFritadeira.AlterarId(idTipoProduto);
            var caracteristicaVoltagem = new TipoProdutoCaracteristica(
                nome: "Voltagem",
                observacao: null,
                idTipoProduto: idTipoProduto,
                idUsuario: Guid.NewGuid()
            );
            caracteristicaVoltagem.AlterarId(2);
            tiposProdutoFritadeira.CaracteristicasTipoProduto.Add(caracteristicaVoltagem);
            tiposProduto.Add(tiposProdutoFritadeira);

            //caracteristicas inexistentes informadas acima
            var tipoProdutoCaracteristicas = new List<TipoProdutoCaracteristica>()
            {
                caracteristicaVoltagem
            };

            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(new List<Domain.Catalogo.Entities.Produto>()),
                    marcaRepository: new MarcaRepositoryFake(marcas),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(tiposProduto, tipoProdutoCaracteristicas.AsQueryable()),
                    vendedorRepository: new VendedorRepositoryFake(null),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == nameof(request.CaracteristicaProduto));
        }


        #region Validar Request 

        [Theory(DisplayName = "Request: " + nameof(CadastroProdutoRequest.NomeProduto))]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        [InlineData(null, nameof(CadastroProdutoRequest.NomeProduto))]
        [InlineData("", nameof(CadastroProdutoRequest.NomeProduto))]
        [InlineData("Fritadeira", nameof(CadastroProdutoRequest.NomeProduto))] //mínimo de 15 características
        [InlineData("Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF31 3,5 L – Preto Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF31 3,5 L – Preto",
            nameof(CadastroProdutoRequest.NomeProduto))] //máximo de 150 características
        public void RetornarNotificacoesQuandoNomeProdutoInvalidoForInformado(string nomeProduto, string notificacaoEsperada)
        {
            //arrange
            string descricao = @"
                Faça pratos suculentos e sem sujar a cozinha com a Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF-31. Com capacidade para até 3,5 litros , você prepara batatas palitos congeladas em até 12 minutos, livre de óleo e muito mais crocantes.
            ";

            var caracteristicas = new List<(int idCaracteristicaProduto, string descricao)>()
            {
                (1, "200º C"),
                (5, "220V")
            };

            var request = new CadastroProdutoRequest(
                nomeProduto: nomeProduto,
                valor: (decimal)89.90,
                idMarca: 15662,
                descricaoProduto: descricao,
                idExterno: null,
                idTipoProduto: 2,
                caracteristicaProduto: caracteristicas,
                idVendedor: null,
                idUsuario: Guid.NewGuid()
            );

            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(null),
                    marcaRepository: new MarcaRepositoryFake(null),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(null),
                    vendedorRepository: new VendedorRepositoryFake(null),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == notificacaoEsperada);
        }


        [Theory(DisplayName = "Request: " + nameof(CadastroProdutoRequest.Valor))]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        [InlineData(0, nameof(CadastroProdutoRequest.Valor))]
        [InlineData(-1, nameof(CadastroProdutoRequest.Valor))]
        public void RetornarNotificacoesQuandoValorInformadoForInvalido(decimal valor, string notificacaoEsperada)
        {
            //arrange
            string descricao = @"
                Faça pratos suculentos e sem sujar a cozinha com a Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF-31. Com capacidade para até 3,5 litros , você prepara batatas palitos congeladas em até 12 minutos, livre de óleo e muito mais crocantes.
            ";

            var caracteristicas = new List<(int idCaracteristicaProduto, string descricao)>()
            {
                (1, "200º C"),
                (5, "220V")
            };

            var request = new CadastroProdutoRequest(
                nomeProduto: "Fritadeira Elétrica Sem Óleo Air Fryer Mondial",
                valor: valor,
                idMarca: 15662,
                descricaoProduto: descricao,
                idExterno: null,
                idTipoProduto: 2,
                caracteristicaProduto: caracteristicas,
                idVendedor: null,
                idUsuario: Guid.NewGuid());

            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(null),
                    marcaRepository: new MarcaRepositoryFake(null),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(null),
                    vendedorRepository: new VendedorRepositoryFake(null),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == notificacaoEsperada);
        }


        [Theory(DisplayName = "Request: " + nameof(CadastroProdutoRequest.IdMarca))]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        [InlineData(0, nameof(CadastroProdutoRequest.IdMarca))]
        [InlineData(-1, nameof(CadastroProdutoRequest.IdMarca))]
        public void RetornarNotificacoesQuandoIdMarcaInvalidaForInformado(int idMarca, string notificacaoEsperada)
        {
            //arrange
            string descricao = @"
                Faça pratos suculentos e sem sujar a cozinha com a Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF-31. Com capacidade para até 3,5 litros , você prepara batatas palitos congeladas em até 12 minutos, livre de óleo e muito mais crocantes.
            ";

            var caracteristicas = new List<(int idCaracteristicaProduto, string descricao)>()
            {
                (1, "200º C"),
                (5, "220V")
            };

            var request = new CadastroProdutoRequest(
                nomeProduto: "Fritadeira Elétrica Sem Óleo Air Fryer Mondial",
                valor: (decimal)89.90,
                idMarca: idMarca,
                descricaoProduto: descricao,
                idExterno: null,
                idTipoProduto: 2,
                caracteristicaProduto: caracteristicas,
                idVendedor: null,
                idUsuario: Guid.NewGuid());

            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(null),
                    marcaRepository: new MarcaRepositoryFake(null),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(null),
                    vendedorRepository: new VendedorRepositoryFake(null),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == notificacaoEsperada);
        }


        [Theory(DisplayName = "Request: " + nameof(CadastroProdutoRequest.DescricaoProduto))]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        [InlineData(null, nameof(CadastroProdutoRequest.DescricaoProduto))]
        [InlineData("", nameof(CadastroProdutoRequest.DescricaoProduto))]
        [InlineData("Produto revolucionário", nameof(CadastroProdutoRequest.DescricaoProduto))] //mínimo de 30 características
                                                                                                //máximo de 100.000 características
        public void RetornarNotificacoesQuandoDescricaoProdutoInvalidaForInformado(string descricaoProduto, string notificacaoEsperada)
        {
            //arrange
            var caracteristicas = new List<(int idCaracteristicaProduto, string descricao)>()
            {
                (1, "200º C"),
                (5, "220V")
            };

            var request = new CadastroProdutoRequest(
                nomeProduto: "Fritadeira Elétrica Sem Óleo Air Fryer Mondial",
                valor: (decimal)89.90,
                idMarca: 15662,
                descricaoProduto: descricaoProduto,
                idExterno: null,
                idTipoProduto: 2,
                caracteristicaProduto: caracteristicas,
                idVendedor: null,
                idUsuario: Guid.NewGuid());

            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(null),
                    marcaRepository: new MarcaRepositoryFake(null),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(null),
                    vendedorRepository: new VendedorRepositoryFake(null),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == notificacaoEsperada);
        }


        [Theory(DisplayName = "Request: " + nameof(CadastroProdutoRequest.IdExterno))]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        [InlineData("332e597ebf-cff2-45dc-948e-c34724d68f192e597ebf-cff2-45dc-948e-c34724d68f19 2e597ebf-cff2-45dc-948e-c34724d68f192e597ebf-cff2-45dc-948e-c34724d68f192e597ebf-cff2-45dc-948e-c34724d68f192e597ebf-cff2-45dc-948e-c34724d68f192e597ebf-cff2-45dc-948e-c34724d68f192e597ebf-cff2-45dc-948e-c34724d68f192e597ebf-cff2-45dc-948e-c34724d68f192e597ebf-cff2-45dc-948e-c34724d68f192e597ebf-cff2-45dc-948e-c34724d68f192e597ebf-cff2-45dc-948e-c34724d68f192e597ebf-cff2-45dc-948e-c34724d68f192e597ebf-cff2-45dc-948e-c34724d",
            nameof(CadastroProdutoRequest.IdExterno))] //máximo de 500 caracteristicas
        public void RetornarNotificacoesQuandoIdExternoInvalidoForInformado(string idExterno, string notificacaoEsperada)
        {
            //arrange
            string descricao = @"
                Faça pratos suculentos e sem sujar a cozinha com a Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF-31. Com capacidade para até 3,5 litros , você prepara batatas palitos congeladas em até 12 minutos, livre de óleo e muito mais crocantes.
            ";

            var caracteristicas = new List<(int idCaracteristicaProduto, string descricao)>()
            {
                (1, "200º C"),
                (5, "220V")
            };

            var request = new CadastroProdutoRequest(
                nomeProduto: "Fritadeira Elétrica Sem Óleo Air Fryer Mondial",
                valor: (decimal)89.90,
                idMarca: 2,
                descricaoProduto: descricao,
                idExterno: idExterno,
                idTipoProduto: 2,
                caracteristicaProduto: caracteristicas,
                idVendedor: null,
                idUsuario: Guid.NewGuid());

            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(null),
                    marcaRepository: new MarcaRepositoryFake(null),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(null),
                    vendedorRepository: new VendedorRepositoryFake(null),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == notificacaoEsperada);
        }


        [Theory(DisplayName = "Request: " + nameof(CadastroProdutoRequest.IdTipoProduto))]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        [InlineData(0, nameof(CadastroProdutoRequest.IdTipoProduto))]
        [InlineData(-1, nameof(CadastroProdutoRequest.IdTipoProduto))]
        public void RetornarNotificacoesQuandoIdTipoProdutoInvalidaForInformado(int idTipoProduto, string notificacaoEsperada)
        {
            //arrange
            string descricao = @"
                Faça pratos suculentos e sem sujar a cozinha com a Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF-31. Com capacidade para até 3,5 litros , você prepara batatas palitos congeladas em até 12 minutos, livre de óleo e muito mais crocantes.
            ";

            var caracteristicas = new List<(int idCaracteristicaProduto, string descricao)>()
            {
                (1, "200º C"),
                (5, "220V")
            };

            var request = new CadastroProdutoRequest(
                nomeProduto: "Fritadeira Elétrica Sem Óleo Air Fryer Mondial",
                valor: (decimal)89.90,
                idMarca: 1,
                descricaoProduto: descricao,
                idExterno: null,
                idTipoProduto: idTipoProduto,
                caracteristicaProduto: caracteristicas,
                idVendedor: null,
                idUsuario: Guid.NewGuid());

            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(null),
                    marcaRepository: new MarcaRepositoryFake(null),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(null),
                    vendedorRepository: new VendedorRepositoryFake(null),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == notificacaoEsperada);
        }


        [Theory(DisplayName = "Request: " + nameof(CadastroProdutoRequest.CaracteristicaProduto))]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        [InlineData(null, nameof(CadastroProdutoRequest.CaracteristicaProduto))]
        public void RetornarNotificacoesQuandoCaracteristicaProdutoInvalidaForInformado(
            IList<(int idCaracteristicaProduto, string descricao)> caracteristicaProduto,
            string notificacaoEsperada)
        {
            //arrange
            string descricao = @"
                Faça pratos suculentos e sem sujar a cozinha com a Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF-31. Com capacidade para até 3,5 litros , você prepara batatas palitos congeladas em até 12 minutos, livre de óleo e muito mais crocantes.
            ";

            var request = new CadastroProdutoRequest(
                nomeProduto: "Fritadeira Elétrica Sem Óleo Air Fryer Mondial",
                valor: (decimal)89.90,
                idMarca: 1,
                descricaoProduto: descricao,
                idExterno: null,
                idTipoProduto: 1,
                caracteristicaProduto: caracteristicaProduto,
                idVendedor: null,
                idUsuario: Guid.NewGuid());

            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(null),
                    marcaRepository: new MarcaRepositoryFake(null),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(null),
                    vendedorRepository: new VendedorRepositoryFake(null),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == notificacaoEsperada);
        }


        [Fact(DisplayName = "Request: " + nameof(CadastroProdutoRequest.CaracteristicaProduto))]
        [Trait("Domain", nameof(CadastroProdutoAppService))]
        public void RetornarNotificacoesQuandoNenhumaCaracteristicaProdutoForInformada()
        {
            //arrange
            string descricao = @"
                Faça pratos suculentos e sem sujar a cozinha com a Fritadeira Elétrica Sem Óleo Air Fryer Mondial New Pratic AF-31. Com capacidade para até 3,5 litros , você prepara batatas palitos congeladas em até 12 minutos, livre de óleo e muito mais crocantes.
            ";

            var request = new CadastroProdutoRequest(
                nomeProduto: "Fritadeira Elétrica Sem Óleo Air Fryer Mondial",
                valor: (decimal)89.90,
                idMarca: 1,
                descricaoProduto: descricao,
                idExterno: null,
                idTipoProduto: 1,
                caracteristicaProduto: new List<(int idCaracteristicaProduto, string descricao)>(),
                idVendedor: null,
                idUsuario: Guid.NewGuid()
            );

            var appService =
                new CadastroProdutoAppService(
                    produtoRepository: new ProdutoRepositoryFake(null),
                    marcaRepository: new MarcaRepositoryFake(null),
                    tipoProdutoRepository: new TipoProdutoRepositoryFake(null),
                    vendedorRepository: new VendedorRepositoryFake(null),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroProdutoDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == nameof(CadastroProdutoRequest.CaracteristicaProduto));
        }

        #endregion
    }
}
