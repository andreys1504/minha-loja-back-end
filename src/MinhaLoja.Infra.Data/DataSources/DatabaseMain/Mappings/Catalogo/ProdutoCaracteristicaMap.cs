using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaLoja.Domain.Catalogo.Entities;

namespace MinhaLoja.Infra.Data.DataSources.DatabaseMain.Mappings.Catalogo
{
    internal class ProdutoCaracteristicaMap : MappingBase<ProdutoCaracteristica>
    {
        public override void Configure(EntityTypeBuilder<ProdutoCaracteristica> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(ProdutoCaracteristica));

            builder
                .Property(produtoCaracteristica => produtoCaracteristica.Descricao)
                .HasMaxLength(150);

            builder
                .HasOne(produtoCaracteristica => produtoCaracteristica.Produto)
                .WithMany(produto => produto.CaracteristicasProduto)
                .HasForeignKey(produtoCaracteristica => produtoCaracteristica.ProdutoId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(produtoCaracteristica => produtoCaracteristica.CaracteristicasTipoProduto)
                .WithMany(tpProdutoCaracteristicas => tpProdutoCaracteristicas.CaracteristicasProdutos)
                .HasForeignKey(produtoCaracteristica => produtoCaracteristica.TipoProdutoCaracteristicasId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
