using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.JwtSigningCredentials;
using NetDevPack.Security.JwtSigningCredentials.Store.EntityFrameworkCore;

namespace MinhaLoja.Infra.Identity.IdentityStorage
{
    public class IdentityMinhaLojaContext : DbContext, ISecurityKeyContext
    {
        public IdentityMinhaLojaContext(
            DbContextOptions<IdentityMinhaLojaContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<SecurityKeyWithPrivate> SecurityKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //SecurityKeys
            modelBuilder.Entity<SecurityKeyWithPrivate>()
                .ToTable("SecurityKeys");

            modelBuilder.Entity<SecurityKeyWithPrivate>()
                .HasKey(securityKey => securityKey.Id);
        }
    }
}
