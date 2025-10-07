using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTask.Models;
namespace TestTask.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(c => c.Id);
            builder.
                HasOne(c => c.Authors)
                .WithMany(c => c.Books)
                .HasForeignKey(c => c.AuthorId);
        }
    }
}
