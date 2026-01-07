using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.EntityConfiguration
{
    internal class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(X => X.CreatedAt).HasColumnName("JoinDate")
                .HasDefaultValueSql("GETDATE()");

            builder.OwnsOne(m => m.HealthRecord, hr =>
            {
                hr.Property(h => h.Weight);
                hr.Property(h => h.Height);
                hr.Property(h => h.BloodType);
                hr.Property(h => h.Note);
            });
        }
    }
}
