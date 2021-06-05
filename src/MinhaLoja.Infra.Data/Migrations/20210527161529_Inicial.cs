using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MinhaLoja.Infra.Data.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Marca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Id2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    IdUsuarioUltimaAtualizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marca", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoProduto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    TipoProdutoSuperiorId = table.Column<int>(type: "int", nullable: true),
                    CodigoGrupoTipoProduto = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroOrdemHierarquiaGrupo = table.Column<int>(type: "int", nullable: false),
                    Id2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    IdUsuarioUltimaAtualizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoProduto_TipoProduto_TipoProdutoSuperiorId",
                        column: x => x.TipoProdutoSuperiorId,
                        principalTable: "TipoProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioAdministrador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    Senha = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    UsuarioMaster = table.Column<bool>(type: "bit", nullable: false),
                    Id2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    IdUsuarioUltimaAtualizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioAdministrador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoProdutoCaracteristica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TipoProdutoId = table.Column<int>(type: "int", nullable: false),
                    Id2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    IdUsuarioUltimaAtualizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoProdutoCaracteristica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoProdutoCaracteristica_TipoProduto_TipoProdutoId",
                        column: x => x.TipoProdutoId,
                        principalTable: "TipoProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vendedor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    Cnpj = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    CadastroAprovado = table.Column<bool>(type: "bit", nullable: true),
                    EmailValidado = table.Column<bool>(type: "bit", nullable: false),
                    CodigoValidacaoEmail = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    DataMaximaCodigoValidacaoEmail = table.Column<DateTime>(type: "DATETIME2(0)", nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Id2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    IdUsuarioUltimaAtualizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendedor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendedor_UsuarioAdministrador_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "UsuarioAdministrador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ValorAnterior = table.Column<decimal>(type: "SMALLMONEY", nullable: true),
                    ValorAtual = table.Column<decimal>(type: "SMALLMONEY", nullable: false),
                    MarcaId = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    IdExterno = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TipoProdutoId = table.Column<int>(type: "int", nullable: false),
                    VendedorId = table.Column<int>(type: "int", nullable: true),
                    Id2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    IdUsuarioUltimaAtualizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_Marca_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "Marca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Produto_TipoProduto_TipoProdutoId",
                        column: x => x.TipoProdutoId,
                        principalTable: "TipoProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Produto_Vendedor_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Vendedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoCaracteristica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    TipoProdutoCaracteristicasId = table.Column<int>(type: "int", nullable: false),
                    Id2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "DATETIME2(0)", nullable: false),
                    IdUsuarioUltimaAtualizacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoCaracteristica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoCaracteristica_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProdutoCaracteristica_TipoProdutoCaracteristica_TipoProdutoCaracteristicasId",
                        column: x => x.TipoProdutoCaracteristicasId,
                        principalTable: "TipoProdutoCaracteristica",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produto_MarcaId",
                table: "Produto",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_TipoProdutoId",
                table: "Produto",
                column: "TipoProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_VendedorId",
                table: "Produto",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoCaracteristica_ProdutoId",
                table: "ProdutoCaracteristica",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoCaracteristica_TipoProdutoCaracteristicasId",
                table: "ProdutoCaracteristica",
                column: "TipoProdutoCaracteristicasId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoProduto_TipoProdutoSuperiorId",
                table: "TipoProduto",
                column: "TipoProdutoSuperiorId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoProdutoCaracteristica_TipoProdutoId",
                table: "TipoProdutoCaracteristica",
                column: "TipoProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendedor_UsuarioId",
                table: "Vendedor",
                column: "UsuarioId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutoCaracteristica");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "TipoProdutoCaracteristica");

            migrationBuilder.DropTable(
                name: "Marca");

            migrationBuilder.DropTable(
                name: "Vendedor");

            migrationBuilder.DropTable(
                name: "TipoProduto");

            migrationBuilder.DropTable(
                name: "UsuarioAdministrador");
        }
    }
}
