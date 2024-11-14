using System.Collections.Immutable;
using Attelas.Models;
using Attelas.Utility;
using Microsoft.EntityFrameworkCore;

namespace Attelas.DbContex;

public class AttelasDbContext : DbContext
{
    public DbSet<InvoiceModel> Invoices { get; set; }
    public DbSet<ClientModel> Clients { get; set; } 
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = GetConfigurations.GetConfiguration("ConnectionStrings:AttelasDataBase");//"Server=.;Database=AttelasWebApi;";
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        // only for unit test
        // optionsBuilder.UseInMemoryDatabase("AttelasDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        modelBuilder.Entity<InvoiceModel>(
            entity => entity.HasKey(x => x.InvoiceNumber));
        modelBuilder.Entity<ClientModel>(
            entity => entity.HasKey(x => x.ClientId));
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}