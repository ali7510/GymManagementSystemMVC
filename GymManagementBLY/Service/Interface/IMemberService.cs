using GymManagementBL.ViewModel.HealthRecordViewModels;
using GymManagementBL.ViewModel.MemberViewModel;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Interface
{
    internal interface IMemberService
    {
        IQueryable<GetAllMembersViewModel> GetAllMembers();

        bool CreateMember(CreateMemberViewModel member);

        GetMemberDetailsViewModel? GetMemberDetails(int memberId);

        HealthRecordViewModel? GetMemberHealthDetails(int memberId);
    }
}
