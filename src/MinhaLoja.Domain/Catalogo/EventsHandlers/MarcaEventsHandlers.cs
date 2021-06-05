using MediatR;
using MinhaLoja.Core.Domain.Events.EventHandler;
using MinhaLoja.Domain.Catalogo.Events.Marca.Cadastro;
using System.Threading;
using System.Threading.Tasks;

namespace MinhaLoja.Domain.Catalogo.EventsHandlers
{
    public class MarcaEventsHandlers : DomainEventHandler,
        INotificationHandler<MarcaCadastradaEvent>
    {
        public MarcaEventsHandlers(DependenciesEventHandler dependenciesEventHandler) 
            : base(dependenciesEventHandler)
        {
        }

        public Task Handle(MarcaCadastradaEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
