using Microsoft.EntityFrameworkCore;
using BancoDigitalAPI.Models;
using static System.Net.Mime.MediaTypeNames;

public class BancoContext : DbContext
{
    public BancoContext(DbContextOptions<BancoContext> options) : base(options) { }

    public DbSet<Conta> Contas { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conta>().HasIndex(c => c.CPF).IsUnique();
    }
}
