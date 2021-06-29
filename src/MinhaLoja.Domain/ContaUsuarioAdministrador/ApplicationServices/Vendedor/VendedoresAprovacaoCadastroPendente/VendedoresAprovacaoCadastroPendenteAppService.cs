using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Queries;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.VendedoresAprovacaoCadastroPendente
{
    public class VendedoresAprovacaoCadastroPendenteAppService : AppService<List<VendedoresAprovacaoCadastroPendenteDataResponse>>,
        IRequestHandler<VendedoresAprovacaoCadastroPendenteRequest, IResponseAppService<List<VendedoresAprovacaoCadastroPendenteDataResponse>>>
    {
        private readonly IVendedorRepository _vendedorRepository;

        public VendedoresAprovacaoCadastroPendenteAppService(
            IVendedorRepository vendedorRepository,
            DependenciesAppService dependenciesAppService) : base(dependenciesAppService)
        {
            _vendedorRepository = vendedorRepository;
        }

        public async Task<IResponseAppService<List<VendedoresAprovacaoCadastroPendenteDataResponse>>> 
            Handle(VendedoresAprovacaoCadastroPendenteRequest request, CancellationToken cancellationToken)
        {
            if (request.Validate() == false)
            {
                return ReturnNotifications(request.Notifications);
            }

            List<VendedoresAprovacaoCadastroPendenteDataResponse> vendedores = 
                await _vendedorRepository
                    .GetEntity()
                    .Include(vendedor => vendedor.Usuario)
                    .Where(VendedorQueries.CadastroUsuarioAprovacaoPendente())
                    .Select(vendedor => new VendedoresAprovacaoCadastroPendenteDataResponse
                    {
                        Email = vendedor.Email,
                        Cnpj = vendedor.Cnpj,
                        Nome = vendedor.Usuario.Nome,
                        Username = vendedor.Usuario.Username
                    })
                    .ToListAsync();

            return ReturnData(vendedores);
        }
    }
}
