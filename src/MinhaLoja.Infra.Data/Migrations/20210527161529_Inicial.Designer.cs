// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MinhaLoja.Infra.Data.DataSources.DatabaseMain;

namespace MinhaLoja.Infra.Data.Migrations
{
    [DbContext(typeof(MinhaLojaContext))]
    [Migration("20210527161529_Inicial")]
    partial class Inicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.Marca", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<DateTime>("DataUltimaAtualizacao")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<Guid>("Id2")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdUsuarioUltimaAtualizacao")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nome")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Marca");
                });

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<DateTime>("DataUltimaAtualizacao")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Id2")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("IdExterno")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid?>("IdUsuarioUltimaAtualizacao")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MarcaId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int>("TipoProdutoId")
                        .HasColumnType("int");

                    b.Property<decimal?>("ValorAnterior")
                        .HasColumnType("SMALLMONEY");

                    b.Property<decimal>("ValorAtual")
                        .HasColumnType("SMALLMONEY");

                    b.Property<int?>("VendedorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MarcaId");

                    b.HasIndex("TipoProdutoId");

                    b.HasIndex("VendedorId");

                    b.ToTable("Produto");
                });

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.ProdutoCaracteristica", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<DateTime>("DataUltimaAtualizacao")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<string>("Descricao")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<Guid>("Id2")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdUsuarioUltimaAtualizacao")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("TipoProdutoCaracteristicasId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.HasIndex("TipoProdutoCaracteristicasId");

                    b.ToTable("ProdutoCaracteristica");
                });

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.TipoProduto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<Guid>("CodigoGrupoTipoProduto")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<DateTime>("DataUltimaAtualizacao")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<Guid>("Id2")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdUsuarioUltimaAtualizacao")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nome")
                        .HasMaxLength(65)
                        .HasColumnType("nvarchar(65)");

                    b.Property<int>("NumeroOrdemHierarquiaGrupo")
                        .HasColumnType("int");

                    b.Property<int?>("TipoProdutoSuperiorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TipoProdutoSuperiorId");

                    b.ToTable("TipoProduto");
                });

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.TipoProdutoCaracteristica", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<DateTime>("DataUltimaAtualizacao")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<Guid>("Id2")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdUsuarioUltimaAtualizacao")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nome")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Observacao")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TipoProdutoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TipoProdutoId");

                    b.ToTable("TipoProdutoCaracteristica");
                });

            modelBuilder.Entity("MinhaLoja.Domain.ContaUsuarioAdministrador.Entities.UsuarioAdministrador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<DateTime>("DataUltimaAtualizacao")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<Guid>("Id2")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdUsuarioUltimaAtualizacao")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nome")
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.Property<string>("Senha")
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.Property<string>("Username")
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.Property<bool>("UsuarioMaster")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("UsuarioAdministrador");
                });

            modelBuilder.Entity("MinhaLoja.Domain.ContaUsuarioAdministrador.Entities.Vendedor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("CadastroAprovado")
                        .HasColumnType("bit");

                    b.Property<string>("Cnpj")
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<string>("CodigoValidacaoEmail")
                        .HasMaxLength(350)
                        .HasColumnType("nvarchar(350)");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<DateTime?>("DataMaximaCodigoValidacaoEmail")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<DateTime>("DataUltimaAtualizacao")
                        .HasColumnType("DATETIME2(0)");

                    b.Property<string>("Email")
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.Property<bool>("EmailValidado")
                        .HasColumnType("bit");

                    b.Property<Guid>("Id2")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdUsuarioUltimaAtualizacao")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId")
                        .IsUnique();

                    b.ToTable("Vendedor");
                });

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.Produto", b =>
                {
                    b.HasOne("MinhaLoja.Domain.Catalogo.Entities.Marca", "Marca")
                        .WithMany("Produtos")
                        .HasForeignKey("MarcaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MinhaLoja.Domain.Catalogo.Entities.TipoProduto", "TipoProduto")
                        .WithMany("Produtos")
                        .HasForeignKey("TipoProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MinhaLoja.Domain.ContaUsuarioAdministrador.Entities.Vendedor", "Vendedor")
                        .WithMany("Produtos")
                        .HasForeignKey("VendedorId");

                    b.Navigation("Marca");

                    b.Navigation("TipoProduto");

                    b.Navigation("Vendedor");
                });

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.ProdutoCaracteristica", b =>
                {
                    b.HasOne("MinhaLoja.Domain.Catalogo.Entities.Produto", "Produto")
                        .WithMany("CaracteristicasProduto")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MinhaLoja.Domain.Catalogo.Entities.TipoProdutoCaracteristica", "CaracteristicasTipoProduto")
                        .WithMany("CaracteristicasProdutos")
                        .HasForeignKey("TipoProdutoCaracteristicasId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CaracteristicasTipoProduto");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.TipoProduto", b =>
                {
                    b.HasOne("MinhaLoja.Domain.Catalogo.Entities.TipoProduto", "TipoProdutoSuperior")
                        .WithMany("TiposProdutosInferiores")
                        .HasForeignKey("TipoProdutoSuperiorId");

                    b.Navigation("TipoProdutoSuperior");
                });

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.TipoProdutoCaracteristica", b =>
                {
                    b.HasOne("MinhaLoja.Domain.Catalogo.Entities.TipoProduto", "TipoProduto")
                        .WithMany("CaracteristicasTipoProduto")
                        .HasForeignKey("TipoProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TipoProduto");
                });

            modelBuilder.Entity("MinhaLoja.Domain.ContaUsuarioAdministrador.Entities.Vendedor", b =>
                {
                    b.HasOne("MinhaLoja.Domain.ContaUsuarioAdministrador.Entities.UsuarioAdministrador", "Usuario")
                        .WithOne("Vendedor")
                        .HasForeignKey("MinhaLoja.Domain.ContaUsuarioAdministrador.Entities.Vendedor", "UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.Marca", b =>
                {
                    b.Navigation("Produtos");
                });

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.Produto", b =>
                {
                    b.Navigation("CaracteristicasProduto");
                });

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.TipoProduto", b =>
                {
                    b.Navigation("CaracteristicasTipoProduto");

                    b.Navigation("Produtos");

                    b.Navigation("TiposProdutosInferiores");
                });

            modelBuilder.Entity("MinhaLoja.Domain.Catalogo.Entities.TipoProdutoCaracteristica", b =>
                {
                    b.Navigation("CaracteristicasProdutos");
                });

            modelBuilder.Entity("MinhaLoja.Domain.ContaUsuarioAdministrador.Entities.UsuarioAdministrador", b =>
                {
                    b.Navigation("Vendedor");
                });

            modelBuilder.Entity("MinhaLoja.Domain.ContaUsuarioAdministrador.Entities.Vendedor", b =>
                {
                    b.Navigation("Produtos");
                });
#pragma warning restore 612, 618
        }
    }
}
