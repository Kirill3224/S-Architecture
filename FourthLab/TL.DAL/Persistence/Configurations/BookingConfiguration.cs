using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TL.DAL.Entities;

namespace TL.DAL.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.CreatedAt).IsRequired();
        builder.Property(b => b.GuestName).IsRequired().HasMaxLength(250);
        builder.Property(b => b.StartDate).IsRequired();
        builder.Property(b => b.EndDate).IsRequired();
    }
}