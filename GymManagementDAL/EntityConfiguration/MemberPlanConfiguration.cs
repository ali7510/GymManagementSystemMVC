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
    internal class MemberPlanConfiguration : IEntityTypeConfiguration<MemberPlan>
    {
        public void Configure(EntityTypeBuilder<MemberPlan> builder)
        {
            builder.HasKey(a=> new { a.MemberId, a.PlanId });
            builder.Property(a => a.CreatedAt).HasColumnName("StartDate").HasDefaultValueSql("GETDATE()");
            builder.Property(a => a.Updated_At).HasDefaultValueSql("GETDATE()");
            builder.Ignore(a => a.Id);
        }
    }
}
