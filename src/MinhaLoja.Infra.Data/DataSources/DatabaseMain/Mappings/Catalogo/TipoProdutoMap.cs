using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaLoja.Domain.Catalogo.Entities;

namespace MinhaLoja.Infra.Data.DataSources.DatabaseMain.Mappings.Catalogo
{
    internal class TipoProdutoMap : MappingBase<TipoProduto>
    {
        public override void Configure(EntityTypeBuilder<TipoProduto> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(TipoProduto));

            builder
                .Property(tipoProduto => tipoProduto.Nome)
                .HasMaxLength(65);

            builder
                .HasOne(tipoProduto => tipoProduto.TipoProdutoSuperior)
                .WithMany(tipoProduto => tipoProduto.TiposProdutosInferiores)
                .HasForeignKey(tipoProduto => tipoProduto.TipoProdutoSuperiorId);
        }
    }
}
