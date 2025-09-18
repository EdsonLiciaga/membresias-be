using membresias.be.Configurations;
using membresias.be.Models;
using Microsoft.EntityFrameworkCore;

namespace membresias.be.Db
{
    public class MembresiasDbContext : DbContext
    {
        public MembresiasDbContext(DbContextOptions<MembresiasDbContext> options)
            : base(options)
        {}

        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Fallecimiento> Fallecimientos { get; set; }
        public DbSet<Miembro> Miembros { get; set; }
        public DbSet<Tarifa> Tarifas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CargoConfiguration());
            modelBuilder.ApplyConfiguration(new MiembroConfiguration());
            modelBuilder.ApplyConfiguration(new FallecimientoConfiguration());
            modelBuilder.ApplyConfiguration(new TarifaConfiguration()); 
        }
    }
}
