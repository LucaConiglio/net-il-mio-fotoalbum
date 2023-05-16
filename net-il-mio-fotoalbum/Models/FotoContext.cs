using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace net_il_mio_fotoalbum.Models
{
    public class FotoContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Photo> Photos { get; set; }
        
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Photo;Integrated Security=True; TrustServerCertificate = true");
        }
    }
}
