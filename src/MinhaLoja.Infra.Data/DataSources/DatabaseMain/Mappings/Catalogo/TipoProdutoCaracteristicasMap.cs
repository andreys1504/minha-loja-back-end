using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaLoja.Domain.Catalogo.Entities;

namespace MinhaLoja.Infra.Data.DataSources.DatabaseMain.Mappings.Catalogo
{
    internal class TipoProdutoCaracteristicasMap : MappingBase<TipoProdutoCaracteristica>
    {
        public override void Configure(EntityTypeBuilder<TipoProdutoCaracteristica> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(TipoProdutoCaracteristica));

            builder
                .Property(tpProdutoCaracteristicas => tpProdutoCaracteristicas.Nome)
                .HasMaxLength(150);

            builder
                .Property(tpProdutoCaracteristicas => tpProdutoCaracteristicas.Observacao)
                .HasMaxLength(100);

            builder
                .HasOne(tpProdutoCaracteristicas => tpProdutoCaracteristicas.TipoProduto)
                .WithMany(tipoProduto => tipoProduto.CaracteristicasTipoProduto)
                .HasForeignKey(tpProdutoCaracteristicas => tpProdutoCaracteristicas.TipoProdutoId);
        }
    }
}
