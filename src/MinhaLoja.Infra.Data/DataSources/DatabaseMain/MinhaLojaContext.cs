using Microsoft.EntityFrameworkCore;
using MinhaLoja.Domain.Catalogo.Entities;
using MinhaLoja.Domain.ContaUsuarioAdministrador.Entities;
using System.Reflection;

namespace MinhaLoja.Infra.Data.DataSources.DatabaseMain
{
    public class MinhaLojaContext : DbContext
    {
        public MinhaLojaContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
            //this.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        //Catalogo
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProdutoCaracteristica> ProdutosCaracteristicas { get; set; }
        public DbSet<TipoProduto> TiposProdutos { get; set; }
        public DbSet<TipoProdutoCaracteristica> TiposProdutosCaracteristicas { get; set; }

        //ContaUsuarioAdministrador
        public DbSet<UsuarioAdministrador> UsuariosAdministradores { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
