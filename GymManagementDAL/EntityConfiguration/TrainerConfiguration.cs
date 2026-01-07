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
    internal class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(a=>a.CreatedAt).HasColumnName("HireDate").HasDefaultValueSql("GETDATE()");
            builder.Property(a=>a.Updated_At).HasDefaultValueSql("GETDATE()");
        }
    }
}
