using GymManagementDAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }
        public HealthRecord HealthRecord { get; set; } = null!;

        // Joind at instead of created at

        public ICollection<MemberPlan> Plans { get; set; } = null!;
        public ICollection<Booking> Sessions { get; set; } = null!;


    }
}
