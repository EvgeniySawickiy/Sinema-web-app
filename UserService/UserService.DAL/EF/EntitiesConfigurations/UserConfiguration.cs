using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.DAL.Entities;

namespace UserService.DAL.EF.EntitiesConfigurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
              .IsRequired()
              .HasMaxLength(100);

            builder.Property(u => u.PhoneNumber)
                   .HasMaxLength(15);

            builder.Property(u => u.Name)
                   .HasMaxLength(50);

            builder.Property(u => u.Surname)
                   .HasMaxLength(50);

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            builder.HasOne(u => u.Account)
                   .WithOne(a => a.User)
                   .HasForeignKey<Account>(a => a.Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
