using Microsoft.EntityFrameworkCore;
using RotaViagem.Domain;

namespace RotaViagem.Infra;

public class RotaDbContext : DbContext
{
    public RotaDbContext(DbContextOptions<RotaDbContext> options) : base(options) { }

    public DbSet<Rota> Rotas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rota>().HasKey(r => new { r.Origem, r.Destino });

        modelBuilder.Entity<Rota>()
            .Property(r => r.Origem).HasMaxLength(3).IsRequired();

        modelBuilder.Entity<Rota>()
            .Property(r => r.Destino).HasMaxLength(3).IsRequired();

        modelBuilder.Entity<Rota>()
            .Property(r => r.Custo).IsRequired();
    }
}
