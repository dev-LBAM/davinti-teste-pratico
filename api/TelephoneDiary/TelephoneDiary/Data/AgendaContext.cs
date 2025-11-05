using Microsoft.EntityFrameworkCore;
using TelephoneDiary.Models;

namespace TelephoneDiary.Data
{
    public class AgendaContext(DbContextOptions<AgendaContext> options) : DbContext(options)
    {
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Telefone> Telefones { get; set; }
    }
}