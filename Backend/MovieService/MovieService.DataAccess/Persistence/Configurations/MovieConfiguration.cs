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
                .HasMaxLength(200);

            builder.Property(m => m.Description)
                .HasMaxLength(1000);

            builder.Property(m => m.Rating)
                .HasPrecision(3, 2);

            builder.Property(m => m.ImageUrl)
                .HasMaxLength(500);

            builder.Property(m => m.TrailerUrl)
                .HasMaxLength(500);

            builder.HasMany(m => m.MovieGenres)
                .WithOne(mg => mg.Movie)
                .HasForeignKey(mg => mg.MovieId);

            builder.ToTable("Movies");
        }
    }
}
