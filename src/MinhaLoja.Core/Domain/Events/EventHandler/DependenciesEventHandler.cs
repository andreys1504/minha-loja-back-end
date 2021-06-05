using MinhaLoja.Core.Infra.Data;
using MinhaLoja.Core.Mediator;

namespace MinhaLoja.Core.Domain.Events.EventHandler
{
    public class DependenciesEventHandler
    {
        public DependenciesEventHandler(
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
