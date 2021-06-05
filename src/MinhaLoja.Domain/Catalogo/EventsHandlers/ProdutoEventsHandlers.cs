using MediatR;
using MinhaLoja.Core.Domain.Events.EventHandler;
using MinhaLoja.Domain.Catalogo.Events.Produto.Cadastro;
using System.Threading;
using System.Threading.Tasks;

namespace MinhaLoja.Domain.Catalogo.EventsHandlers
{
    public class ProdutoEventsHandlers : DomainEventHandler,
        INotificationHandler<ProdutoCadastradoEvent>
    {
        public ProdutoEventsHandlers(DependenciesEventHandler dependenciesEventHandler)
            : base(dependenciesEventHandler)
        {
        }

        public Task Handle(ProdutoCadastradoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
