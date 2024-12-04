using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieService.Core.Entities;

namespace MovieService.DataAccess.Persistence.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.DurationInMinutes)
                .IsRequired();

            builder.Property(m => m.Genre)
                .IsRequired();
        }
    }
}
