using MinhaLoja.Core.Domain.ApplicationServices.Service;
using MinhaLoja.Tests.Fakes.Infra.Data;
using MinhaLoja.Tests.Fakes.Mediator;

namespace MinhaLoja.Tests.Fakes.Domain.ApplicationServices.Service
{
    public class DependenciesAppServiceFake : DependenciesAppService
    {
        public DependenciesAppServiceFake() 
            : base(new ApplicationTransactionFake(), new MediatorHandlerFake())
        {
        }
    }
}
