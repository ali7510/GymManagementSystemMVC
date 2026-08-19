using GymManagementBL.ViewModel.MemberPlanViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Interface
{
    public interface IMemberPlanService
    {
        public IQueryable<GetAllMembershipsViewModel>? GetAllMemberships();

        public bool CreateMemberPlan(CreateMembershipViewModel memberPlan);

        public bool DeleteMemberPlan(int memberId, int planId);

        public bool GetMembershipsOfMember(int memberId);
        public bool GetAllMembersWithActiveMembership(int memberId);
    }
}
