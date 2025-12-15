using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.ViewModel.MemberViewModel
{
    internal class GetMemberDetailsViewModel
    {
        public string? Photo { get; set; }
        public string Name { get; set; } = null!;
        public string? PlanName { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public DateOnly? MembershipStartDate { get; set; }
        public DateTime? MembershipEndDate { get; set; }
        public int BuildinhNo { get; set; } = default!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
