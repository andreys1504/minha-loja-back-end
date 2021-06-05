using MinhaLoja.Core.Domain.Entities.EntityBase;
using System;

namespace MinhaLoja.Core.Domain.Entities.AggregateRootBase
{
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        public AggregateRoot(Guid? idUsuario) : base(idUsuario)
        {
        }
    }
}
