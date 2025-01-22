using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieService.Core.Entities;

namespace MovieService.DataAccess.Persistence.Configurations
{
    public class HallConfiguration : IEntityTypeConfiguration<Hall>
    {
        public void Configure(EntityTypeBuilder<Hall> builder)
        {
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(h => h.TotalSeats)
                .IsRequired();

            builder.Property(h => h.SeatLayoutJson)
                .IsRequired();

            builder.ToTable("Halls");
        }
    }
}
