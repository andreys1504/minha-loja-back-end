using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Core.Domain.Exceptions;
using MinhaLoja.Domain.Catalogo.Events.Marca.Cadastro;
using MinhaLoja.Domain.Catalogo.Queries;
using MinhaLoja.Domain.Catalogo.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MarcaMensagens = MinhaLoja.Domain.MessagesDomain.Catalogo;

namespace MinhaLoja.Domain.Catalogo.ApplicationServices.Marca.Cadastro
{
    public class CadastroMarcaAppService : AppService<CadastroMarcaDataResponse>,
        IRequestHandler<CadastroMarcaRequest, IResponseAppService<CadastroMarcaDataResponse>>
    {
        private readonly IMarcaRepository _marcaRepository;

        public CadastroMarcaAppService(
            IMarcaRepository marcaRepository,
            DependenciesAppService dependenciesAppService)
            : base(dependenciesAppService)
        {
            _marcaRepository = marcaRepository;
        }

        public async Task<IResponseAppService<CadastroMarcaDataResponse>> Handle(
            CadastroMarcaRequest request,
            CancellationToken cancellationToken)
        {
            if (!request.Validate())
                return ReturnNotifications(request.Notifications);

            bool marcaExistente = _marcaRepository
                .GetEntity()
                .Any(MarcaQueries.MarcaExistenteSistema(request.NomeMarca));

            if (marcaExistente)
                return ReturnNotification(nameof(request.NomeMarca), MarcaMensagens.Marca_Cadastro_MarcaJaCadastradaSistema);

            var marca = new Entities.Marca(
                nome: request.NomeMarca,
                idUsuario: request.IdUsuario);

            if (marca.IsValid == false)
                return ReturnNotifications(marca.Notifications);

            await _marcaRepository.AddEntityAsync(marca);

            if (await CommitAsync())
            {
                await PublishEventAsync(
                    @event: new MarcaCadastradaEvent(
                        idMarca: marca.Id,
                        nomeMarca: marca.Nome
                    ),
                    aggregateRoot: marca
                );

                return ReturnData(new CadastroMarcaDataResponse()
                {
                    IdMarca = marca.Id,
                    NomeMarca = marca.Nome
                });
            }

            throw new DomainException("erro na realização do cadastro da Marca");
        }
    }
}
