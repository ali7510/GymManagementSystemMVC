using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.ViewModel.MemberSessionViewModel
{
    public class GetAllMemberSessionsViewModel
    {
        public int MemberId { get; set; }
        public int SessionId { get; set; }
        public int TrainerId { get; set; }
        public string TrainerSpeciality { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int Capacity { get; set; }
        public int FreeCapacity { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan Duration { get; set; }
        public string TrainerName { get; set; } = null!;
    }
}
