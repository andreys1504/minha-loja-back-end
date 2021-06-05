using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaLoja.Domain.Catalogo.Entities;

namespace MinhaLoja.Infra.Data.DataSources.DatabaseMain.Mappings.Catalogo
{
    internal class ProdutoMap : MappingBase<Produto>
    {
        public override void Configure(EntityTypeBuilder<Produto> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(Produto));

            builder
                .Property(produto => produto.Nome)
                .HasMaxLength(150);

            builder
                .Property(produto => produto.ValorAnterior)
                .HasColumnType("SMALLMONEY");

            builder
                .Property(produto => produto.ValorAtual)
                .HasColumnType("SMALLMONEY");

            builder
                .HasOne(produto => produto.Marca)
                .WithMany(marca => marca.Produtos)
                .HasForeignKey(produto => produto.MarcaId);

            builder
                .Property(produto => produto.Descricao)
                .HasColumnType("TEXT");

            builder
                .Property(produto => produto.IdExterno)
                .HasMaxLength(500);

            builder
                .HasOne(produto => produto.TipoProduto)
                .WithMany(tipoProduto => tipoProduto.Produtos)
                .HasForeignKey(produto => produto.TipoProdutoId);

            builder
                .HasOne(produto => produto.Vendedor)
                .WithMany(vendedor => vendedor.Produtos)
                .HasForeignKey(produto => produto.VendedorId);
        }
    }
}
