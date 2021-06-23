using MediatR;
using MinhaLoja.Core.Domain.ApplicationServices.Response;
using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Queries;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MinhaLoja.Domain.ContaUsuarioAdministrador.ApplicationServices.Vendedor.VendedoresValidacaoCadastroPendente
{
    public class VendedoresValidacaoCadastroPendenteAppService : AppService<List<VendedoresValidacaoCadastroPendenteDataResponse>>,
        IRequestHandler<VendedoresValidacaoCadastroPendenteRequest, IResponseAppService<List<VendedoresValidacaoCadastroPendenteDataResponse>>>
    {
        private readonly IVendedorRepository _vendedorRepository;

        public VendedoresValidacaoCadastroPendenteAppService(
            IVendedorRepository vendedorRepository,
            DependenciesAppService dependenciesAppService) : base(dependenciesAppService)
        {
            _vendedorRepository = vendedorRepository;
        }

        public async Task<IResponseAppService<List<VendedoresValidacaoCadastroPendenteDataResponse>>> 
            Handle(VendedoresValidacaoCadastroPendenteRequest request, CancellationToken cancellationToken)
        {
            if (request.Validate() == false)
            {
                return ReturnNotifications(request.Notifications);
            }

            List<VendedoresValidacaoCadastroPendenteDataResponse> vendedores = 
                await _vendedorRepository
                    .GetEntity()
                    .Include(vendedor => vendedor.Usuario)
                    .Where(VendedorQueries.CadastroUsuarioAprovacaoPendente())
                    .Select(vendedor => new VendedoresValidacaoCadastroPendenteDataResponse
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
