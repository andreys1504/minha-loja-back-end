using MinhaLoja.Core.Domain.Entities.AggregateRootBase;
using MinhaLoja.Core.Domain.Entities.EntityBase;
using System;

namespace MinhaLoja.Core.Domain.Entities.AggregateBase
{
    public abstract class Aggregate<TAggregateRoot> : Entity, IAggregate
        where TAggregateRoot : AggregateRoot
    {
        public Aggregate(Guid? idUsuario) : base(idUsuario)
        {
        }
    }
}
