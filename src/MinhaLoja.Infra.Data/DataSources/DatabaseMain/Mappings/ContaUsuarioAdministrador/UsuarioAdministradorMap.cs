using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Entities;

namespace MinhaLoja.Infra.Data.DataSources.DatabaseMain.Mappings.ContaUsuarioAdministrador
{
    internal class UsuarioAdministradorMap : MappingBase<UsuarioAdministrador>
    {
        public override void Configure(EntityTypeBuilder<UsuarioAdministrador> builder)
        {
            base.Configure(builder);

            base.RequiredFalseIdUsuarioUltimaAtualizacao(builder);

            builder.ToTable(nameof(UsuarioAdministrador));

            builder
                .Property(usuarioProfissional => usuarioProfissional.Nome)
                .HasMaxLength(45);

            builder
                .Property(usuarioProfissional => usuarioProfissional.Username)
                .HasMaxLength(45);

            builder
                .Property(usuarioProfissional => usuarioProfissional.Senha)
                .HasMaxLength(350);
        }
    }
}
