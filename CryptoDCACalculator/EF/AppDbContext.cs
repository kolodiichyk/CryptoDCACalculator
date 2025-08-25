using CryptoDCACalculator.EF.Data;
using CryptoDCACalculator.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoDCACalculator.EF;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }
    public DbSet<PriceHistory> PriceHistory { get; set; }
    public DbSet<Investment> Investments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CryptoCurrency>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<PriceHistory>()
            .HasOne(p => p.CryptoCurrency)
            .WithMany(c => c.PriceHistory)
            .HasForeignKey(p => p.CryptoCurrencyId);
        modelBuilder.Entity<PriceHistory>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Investment>()
            .HasOne(i => i.CryptoCurrency)
            .WithMany()
            .HasForeignKey(i => i.CryptoCurrencyId);
        modelBuilder.Entity<Investment>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<InvestmentCalculation>()
            .HasOne(i => i.Investment)
            .WithMany()
            .HasForeignKey(i => i.InvestmentId);
        modelBuilder.Entity<InvestmentCalculation>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        // Seed data for popular cryptocurrencies
        modelBuilder.Entity<CryptoCurrency>().HasData(
            CryptoCurrencyData.Seed()
        );

        // Seed data for investments
        modelBuilder.Entity<Investment>().HasData(
            InvestmentData.Seed()
        );
    }
}
