using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Entities;

namespace MinhaLoja.Infra.Data.DataSources.DatabaseMain.Mappings.ContaUsuarioAdministrador
{
    internal class VendedorMap : MappingBase<Vendedor>
    {
        public override void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            base.Configure(builder);

            base.RequiredFalseIdUsuarioUltimaAtualizacao(builder);

            builder.ToTable(nameof(Vendedor));

            builder
                .Property(vendedor => vendedor.Email)
                .HasMaxLength(45);

            builder
                .Property(vendedor => vendedor.Cnpj)
                .HasMaxLength(14);

            builder
                .Property(vendedor => vendedor.CodigoValidacaoEmail)
                .HasMaxLength(350);

            builder
                .Property(vendedor => vendedor.DataMaximaCodigoValidacaoEmail)
                .HasColumnType("DATETIME2(0)");

            builder
                .HasOne(vendedor => vendedor.Usuario)
                .WithOne(usuario => usuario.Vendedor)
                .HasForeignKey<Vendedor>(vendedor => vendedor.UsuarioId);
        }
    }
}
