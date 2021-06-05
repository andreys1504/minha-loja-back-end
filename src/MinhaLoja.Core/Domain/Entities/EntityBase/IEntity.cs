using System;

namespace MinhaLoja.Core.Domain.Entities.EntityBase
{
    public interface IEntity
    {
        int Id { get; }
        Guid Id2 { get; }
        DateTime DataCadastro { get; }
        DateTime DataUltimaAtualizacao { get; }
        Guid? IdUsuarioUltimaAtualizacao { get; }

        //Para testes unitários
        void AlterarId(int valor);
    }
}
