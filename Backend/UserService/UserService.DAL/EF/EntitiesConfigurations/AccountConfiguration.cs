using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.DAL.Entities;

namespace UserService.DAL.EF.EntitiesConfigurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Login)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.PasswordHash)
                   .IsRequired();

            builder.HasOne(a => a.User)
                   .WithOne(u => u.Account)
                   .HasForeignKey<User>(u => u.Id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.RefreshToken)
                   .WithOne(rt => rt.Account)
                   .HasForeignKey<RefreshToken>(rt => rt.Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
