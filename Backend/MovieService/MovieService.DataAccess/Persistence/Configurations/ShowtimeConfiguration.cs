using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieService.Core.Entities;

namespace MovieService.DataAccess.Persistence.Configurations
{
    public class ShowtimeConfiguration : IEntityTypeConfiguration<Showtime>
    {
        public void Configure(EntityTypeBuilder<Showtime> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.StartTime)
                .IsRequired();

            builder.HasOne(s => s.Movie)
                .WithMany()
                .HasForeignKey(s => s.MovieId);

            builder.HasOne(s => s.Hall)
                .WithMany()
                .HasForeignKey(s => s.HallId);
        }
    }
}
