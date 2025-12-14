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
    internal class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(a => a.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Property(a=>a.Description).HasColumnType("varchar").HasMaxLength(200);
            builder.Property(a=>a.Price).HasColumnType("decimal(10,2)").IsRequired();
            
            builder.ToTable(tb=>tb.HasCheckConstraint("PlanDurationWithinRange", "DurationDays > 0 AND DurationDays < 361"));
        }
    }
}
