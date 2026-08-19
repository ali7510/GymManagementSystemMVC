using GymManagementBL.ViewModel.MemberSessionViewModel;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Interface
{
    public interface IMemberSessionService
    {
        public IQueryable<GetAllMemberSessionsViewModel>? GetAllMemberSessions();

        public int GetFreeCapacityCount(int sessionId);

        public bool CreateBooking(CreateMemberSessionViewModel booking);
        public IQueryable<GetAllMembersInBooking>? GetAllMembersInOngoingBookings();
        public IQueryable<GetAllMembersInBooking>? GetAllMembersInUpcomingBookings();
        public bool MarkMemberAsAttended(int memberId, int sessionId);
    }
}
