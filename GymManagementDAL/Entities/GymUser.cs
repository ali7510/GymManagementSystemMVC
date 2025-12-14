using GymManagementDAL.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    internal class GymUser : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        // join date which is created at from base entity, you should handle it in fluent api
        public Gender Gender { get; set; }
        public Address? Address { get; set; }


    }

    [Owned]
    internal class Address
    {
        public int BuildingNo { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
