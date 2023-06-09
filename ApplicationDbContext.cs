using AdminPolizasAPI.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace AdminPolizasAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PolizasCoberturas>().Property(p => p.MontoAsegurado)
                .HasPrecision(14, 2);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Cobertura> Coberturas { get; set; }
        public DbSet<Poliza> Polizas { get; set; }
        public DbSet<PolizasCoberturas> PolizasCoberturas { get; set; }
    }
}
