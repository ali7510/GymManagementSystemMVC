using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.EntityConfiguration
{
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("SessionCapacityInRange", "Capacity > 0 AND Capacity < 26");
                tb.HasCheckConstraint("SessionCapacityEndDateConstraint", "EndDate > StartDate");
            });

            builder.HasOne(a=>a.Category).WithMany(a=>a.Category_Sessions).HasForeignKey(a => a.Category_Id);
            builder.Property(a => a.Created_At).HasColumnName("StartDate").HasDefaultValueSql("GETDATE()");
            builder.HasOne(a=>a.Trainer).WithMany(a=>a.Trainer_Sessions).HasForeignKey(a => a.Trainer_Id);
        }
    }
}
