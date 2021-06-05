namespace MinhaLoja.Core.Domain.Events.EventHandler
{
    public abstract class DomainEventHandler
    {
        private readonly DependenciesEventHandler _dependenciesEventHandler;

        public DomainEventHandler(DependenciesEventHandler dependenciesEventHandler)
        {
            _dependenciesEventHandler = dependenciesEventHandler;
        }
    }
}
