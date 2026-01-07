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
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Ignore(x=>x.CreatedAt);
            builder.Property(a=>a.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Property(a => a.Email).HasColumnType("varcahr").HasMaxLength(100);
            builder.Property(a => a.Phone).HasColumnType("varchar").HasMaxLength(11);
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("GymUserValidEmailCheck", "Email LIKE '%_@__%.__%' OR Email IS NULL");
                tb.HasCheckConstraint("GymUserValidPhoneCheck", "Phone LIKE '01%' AND Phone NOT LIKE '%[^0-9]%'");
            });
            builder.HasIndex(a=>a.Email).IsUnique();
            builder.HasIndex(a => a.Phone).IsUnique();

            builder.OwnsOne(a => a.Address, addressbuilder =>
            {
                addressbuilder.Property(b => b.Street).HasColumnType("varchar").HasMaxLength(30);
                addressbuilder.Property(b => b.City).HasColumnType("varchar").HasMaxLength(30);
            });
        }
    }
}
