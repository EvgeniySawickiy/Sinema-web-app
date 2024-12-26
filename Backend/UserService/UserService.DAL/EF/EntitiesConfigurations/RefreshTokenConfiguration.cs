using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.DAL.Entities;

namespace UserService.DAL.EF.EntitiesConfigurations
{
    internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                   .HasMaxLength(500);

            builder.Property(rt => rt.LifeTime);

            builder.HasOne(rt => rt.Account)
                   .WithOne(a => a.RefreshToken)
                   .HasForeignKey<RefreshToken>(rt => rt.Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
