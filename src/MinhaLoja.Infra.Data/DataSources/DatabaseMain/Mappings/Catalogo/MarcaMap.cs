using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaLoja.Domain.Catalogo.Entities;

namespace MinhaLoja.Infra.Data.DataSources.DatabaseMain.Mappings.Catalogo
{
    internal class MarcaMap : MappingBase<Marca>
    {
        public override void Configure(EntityTypeBuilder<Marca> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(Marca));

            builder
                .Property(marca => marca.Nome)
                .HasMaxLength(40);
        }
    }
}
