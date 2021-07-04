using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Domain.Catalogo.ApplicationServices.Marca.Cadastro;
using MinhaLoja.Tests.Fakes.Domain.ApplicationServices.Service;
using MinhaLoja.Tests.Fakes.Infra.Data.Repositories.Catalogo;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace MinhaLoja.Domain.Tests.Catalogo.ApplicationServices.Marca.Cadastro
{
    public class CadastroMarcaAppServiceTests
    {
        [Fact(DisplayName = "Sucesso Cadastro Marca")]
        [Trait("Domain", nameof(CadastroMarcaAppService))]
        public void RetornarSucesso()
        {
            //arrange
            var request = new CadastroMarcaRequest(
                nomeMarca: "Nike",
                idUsuario: Guid.NewGuid()
            );

            var marcas = new List<Domain.Catalogo.Entities.Marca>();

            var appService = new CadastroMarcaAppService(
                marcaRepository: new MarcaRepositoryFake(marcas),
                dependenciesAppService: new DependenciesAppServiceFake());

            //act
            IResponseAppService<CadastroMarcaDataResponse> response = 
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.True(response.Success);
            Assert.Equal(request.NomeMarca, response.Data.NomeMarca);
        }


        [Fact(DisplayName = "Marca Existente no Sistema")]
        [Trait("Domain", nameof(CadastroMarcaAppService))]
        public void RetornarNotificacaoQuandoMarcaExistenteNoSistemaForInformada()
        {
            //arrange
            string nomeMarca = "Nike";
            var request = new CadastroMarcaRequest(
                nomeMarca: nomeMarca,
                idUsuario: Guid.NewGuid()
            );

            var marcas = new List<Domain.Catalogo.Entities.Marca>();
            marcas.Add(new Domain.Catalogo.Entities.Marca(
                nome: nomeMarca,
                idUsuario: Guid.NewGuid()
              )
            );

            var appService = new CadastroMarcaAppService(
                marcaRepository: new MarcaRepositoryFake(marcas),
                dependenciesAppService: new DependenciesAppServiceFake()
            );

            //act
            IResponseAppService<CadastroMarcaDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == nameof(request.NomeMarca));
        }


        #region Validar Request 

        [Theory(DisplayName = "Request: " + nameof(CadastroMarcaRequest.NomeMarca))]
        [Trait("Domain", nameof(CadastroMarcaAppService))]
        [InlineData(null, nameof(CadastroMarcaRequest.NomeMarca))] //Nulo
        [InlineData("", nameof(CadastroMarcaRequest.NomeMarca))] //Vazio
        [InlineData("N", nameof(CadastroMarcaRequest.NomeMarca))] //mínimo de 2 características
        [InlineData("--Nike Nike Nike Nike Nike Nike Nike Nike ",
            nameof(CadastroMarcaRequest.NomeMarca))] //máximo de 40 características
        public void RetornarNotificacoesQuandoNomeMarcaInvalidoForInformado(string nomeMarca, string notificacaoEsperada)
        {
            //arrange
            var request = new CadastroMarcaRequest(
                nomeMarca: nomeMarca,
                idUsuario: Guid.NewGuid()
            );

            var appService =
                new CadastroMarcaAppService(
                    marcaRepository: new MarcaRepositoryFake(null),
                    dependenciesAppService: new DependenciesAppServiceFake()
                );

            //act
            IResponseAppService<CadastroMarcaDataResponse> response =
                appService.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.False(response.Success);
            Assert.Contains(response.Notifications, notification => notification.Key == notificacaoEsperada);
        }

        #endregion
    }
}
