using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.ViewModel.MemberSessionViewModel
{
    public class ViewMembersViewModel
    {
        public int MemberId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BookingDate { get; set; }

    }
}
