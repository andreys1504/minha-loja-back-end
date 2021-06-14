using Flunt.Notifications;
using System;

namespace MinhaLoja.Core.Domain.Entities.EntityBase
{
    public abstract class Entity : Notifiable<Notification>, IEntity
    {
        public Entity(Guid? idUsuario)
        {
            Id2 = Guid.NewGuid();
            DataCadastro = DateTime.Now;
            DataUltimaAtualizacao = DateTime.Now;
            IdUsuarioUltimaAtualizacao = idUsuario;
        }

        public int Id { get; private set; }
        public Guid Id2 { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataUltimaAtualizacao { get; private set; }
        public Guid? IdUsuarioUltimaAtualizacao { get; private set; }

        protected void AtualizarDataUltimaAtualizacao()
        {
            DataUltimaAtualizacao = DateTime.Now;
        }

        //Para testes unitários
        public void AlterarId(int valor)
        {
            Id = valor;
        }
    }
}
