using Microsoft.EntityFrameworkCore;
using TelephoneDiary.Models;

namespace TelephoneDiary.Data
{
    public class AgendaContext(DbContextOptions<AgendaContext> options) : DbContext(options)
    {
        public DbSet<Contatos> Contatos { get; set; }
        public DbSet<Telefones> Telefones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Telefones>()
                .HasOne<Contatos>()
                .WithMany(c => c.Telefones)
                .HasForeignKey(t => t.IDContato)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}