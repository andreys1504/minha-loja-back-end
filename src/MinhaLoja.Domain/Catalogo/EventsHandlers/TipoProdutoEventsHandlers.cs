using MediatR;
using MinhaLoja.Core.Domain.Events.EventHandler;
using MinhaLoja.Domain.Catalogo.Events.TipoProduto.Cadastro;
using System.Threading;
using System.Threading.Tasks;

namespace MinhaLoja.Domain.Catalogo.EventsHandlers
{
    public class TipoProdutoEventsHandlers : DomainEventHandler,
        INotificationHandler<TipoProdutoCadastradoEvent>
    {
        public TipoProdutoEventsHandlers(DependenciesEventHandler dependenciesEventHandler)
            : base(dependenciesEventHandler)
        {
        }

        public Task Handle(TipoProdutoCadastradoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
