using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TL.DAL.Entities;

namespace TL.DAL.Configurations;

public class RoomCategoryConfiguration : IEntityTypeConfiguration<RoomCategory>
{
    public void Configure(EntityTypeBuilder<RoomCategory> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.PricePerNight).IsRequired().HasColumnType("decimal(18,2)");

        builder.HasMany(c => c.Rooms)
                .WithOne(r => r.Category)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}