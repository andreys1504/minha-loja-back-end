using MinhaLoja.Core.Infra.Data;
using MinhaLoja.Core.Mediator;

namespace MinhaLoja.Core.Domain.ApplicationServices.Service
{
    public class DependenciesAppService
    {
        public DependenciesAppService(
            IApplicationTransaction applicationTransaction,
            IMediatorHandler mediatorHandler)
        {
            ApplicationTransaction = applicationTransaction;
            MediatorHandler = mediatorHandler;
        }

        public IApplicationTransaction ApplicationTransaction { get; private set; }
        public IMediatorHandler MediatorHandler { get; private set; }
    }
}
