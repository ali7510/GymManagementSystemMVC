using AutoMapper;
using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel.MemberPlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Class;
using GymManagementDAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Class
{
    public class MemberPlanService : IMemberPlanService
    {
        private readonly IMapper _mapping;
        private readonly IGenericRepository<MemberPlan> _membershipRepositories;
        private readonly IUnitOfWork _unitOfWork;

        public MemberPlanService(IMapper mapping, IGenericRepository<MemberPlan> membershipRepositories, IUnitOfWork unitOfWork)
        {
            _mapping = mapping;
            _membershipRepositories = membershipRepositories;
            _unitOfWork = unitOfWork;
        }
        public IQueryable<GetAllMembershipsViewModel>? GetAllMemberships()
        {
            var memberPlans = _membershipRepositories.GetAll().Include(s => s.Member).Include(p => p.Plan);
            if (memberPlans == null) return null;
            var memberships = new List<GetAllMembershipsViewModel>();

            memberships = _mapping.Map<List<GetAllMembershipsViewModel>>(memberPlans.ToList());
            return memberships.AsQueryable();
        }

        public bool CreateMemberPlan(CreateMembershipViewModel memberPlan)
        {
            try
            {
                if (memberPlan == null)
                {
                    return false;
                }
                var newMemberPlan = _mapping.Map<CreateMembershipViewModel, MemberPlan>(memberPlan);
                _membershipRepositories.Create(newMemberPlan);
                var isCreated = _unitOfWork.SaveChange() > 0;
                return isCreated;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) if necessary
                return false;
            }
        }

        public bool DeleteMemberPlan(int memberId, int planId)
        {
            try
            {
                var memberPlan = _membershipRepositories.GetAll(X => X.MemberId == memberId && X.PlanId == planId).FirstOrDefault();
                if (memberPlan == null)
                {
                    return false;
                }
                if (memberPlan.EndDate > DateTime.Now)
                {
                    return false;
                }
                _membershipRepositories.Delete(memberPlan);
                var isDeleted = _unitOfWork.SaveChange() > 0;
                return isDeleted;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) if necessary
                return false;
            }
        }

        public bool GetMembershipsOfMember(int memberId)
        {
            try
            {
                if (memberId <= 0)
                {
                    return false;
                }
                var isMemberPlan = _membershipRepositories.GetAll(X => X.MemberId == memberId).Any();
                return isMemberPlan;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) if necessary
                return false;

            }
        }
        public bool GetAllMembersWithActiveMembership(int memberId)
        {
            var activeMembers = _membershipRepositories.GetAll(x=>x.Status == 1);
            return activeMembers.Any(x => x.MemberId == memberId);
        }
    }
}
