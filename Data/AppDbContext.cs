using Microsoft.EntityFrameworkCore;
using SolicitacoesInternas.Models;

namespace SolicitacoesInternas.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Solicitacao> Solicitacoes => Set<Solicitacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Solicitacao>(entity =>
        {
            entity.ToTable("solicitacoes");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Solicitante)
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Tipo)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Descricao)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.DataSolicitacao)
                .IsRequired();
        });
    }
}
