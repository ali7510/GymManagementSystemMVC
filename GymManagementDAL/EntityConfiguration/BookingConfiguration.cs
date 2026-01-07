using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.EntityConfiguration
{
    internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(a => new { a.MemberId, a.SessionId });
            builder.Property(a => a.CreatedAt).HasColumnName("BookingDate").HasDefaultValueSql("GETDATE()");
            builder.Property(a => a.Updated_At).HasDefaultValueSql("GETDATE()");
            builder.Ignore(a => a.Id);
        }
    }
}
