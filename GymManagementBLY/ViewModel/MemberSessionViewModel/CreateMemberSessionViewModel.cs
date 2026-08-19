using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.ViewModel.MemberSessionViewModel
{
    public class CreateMemberSessionViewModel
    {
        public int MemberId { get; set; }
        public int SessionId { get; set; }
        public bool isAttended { get; set; } = false;
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public DateTime Updated_At { get; set; } = DateTime.Now;

    }
}
