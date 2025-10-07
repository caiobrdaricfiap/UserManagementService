using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementService.Entities;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("UserEntitys");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name)
                 .IsRequired()
                 .HasMaxLength(100);
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(e => e.PasswordHash)
            .IsRequired();
        builder.Property(e => e.Salt)
            .IsRequired();
        builder.Property(e => e.CreatedAt)
            .HasColumnType("DATETIME2");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("DATETIME2");
        builder.Property(e => e.IsActive)
            .HasColumnType("BIT");
    }
}
