using Microsoft.EntityFrameworkCore;
using TestTask.Configurations;
using TestTask.Models;
namespace TestTask.Data

{
    public class Context(DbContextOptions<Context> options)
        :DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}