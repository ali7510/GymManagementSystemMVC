using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.ViewModel.MemberSessionViewModel
{
    public class GetAllMembersInBooking
    {
        public int MemberId { get; set; }
        public int SessionId { get; set; }
        public string MemberName { get; set; } = null!;
        public bool isAttended { get; set; }
    }
}
