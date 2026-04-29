using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TL.DAL.Entities;

namespace TL.DAL.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.CreatedAt).IsRequired();
        builder.Property(r => r.Number).IsRequired().HasMaxLength(100);
        builder.Property(r => r.Status).IsRequired().HasConversion<string>().HasMaxLength(20);

        builder.HasMany(r => r.Bookings)
                .WithOne(b => b.Room)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}