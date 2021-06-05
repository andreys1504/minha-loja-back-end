using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaLoja.Core.Domain.Entities.EntityBase;

namespace MinhaLoja.Infra.Data.DataSources.DatabaseMain.Mappings
{
    internal abstract class MappingBase<TEntity> : IEntityTypeConfiguration<TEntity> 
        where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Ignore(entity => entity.Notifications);
            builder.Ignore(entity => entity.IsValid);


            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(entity => entity.DataCadastro)
                .HasColumnType("DATETIME2(0)");

            builder
                .Property(entity => entity.DataUltimaAtualizacao)
                .HasColumnType("DATETIME2(0)");

            builder
                .Property(entity => entity.IdUsuarioUltimaAtualizacao)
                .IsRequired();
        }

        protected void RequiredFalseIdUsuarioUltimaAtualizacao(EntityTypeBuilder<TEntity> builder)
        {
            builder
                .Property(entity => entity.IdUsuarioUltimaAtualizacao)
                .IsRequired(false);
        }
    }
}
