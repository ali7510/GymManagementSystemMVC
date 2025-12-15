using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel.HealthRecordViewModels;
using GymManagementBL.ViewModel.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Class;
using GymManagementDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Class
{
    internal class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<MemberPlan> _memberPlanRepository;
        private readonly IPlanRepsitory _planRepository;

        public MemberService(IGenericRepository<Member> memberRepository, IGenericRepository<MemberPlan> memberPlanRepository, IPlanRepsitory planRepository)
        {
            _memberRepository = memberRepository;
            _memberPlanRepository = memberPlanRepository;
            _planRepository = planRepository;
        }

        public bool CreateMember(CreateMemberViewModel member)
        {
            // if this member already exist
            var mem = _memberRepository.GetAll(m => m.Phone == member.Phone);
            var mem2 = _memberRepository.GetAll(m => m.Email == member.Email);

            if (mem is null || mem2 is null)
            {
                Member newMember = new Member()
                {
                    Phone = member.Phone,
                    Email = member.Email,
                    Name = member.Name,
                    DateOfBirth = member.DateOfBirth,
                    Gender = member.Gender,
                    Address = new Address()
                    {
                        City = member.City,
                        Street = member.Street,
                        BuildingNo = member.BuildingNumber
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Weight = member.HealthRecord.Weight,
                        Height = member.HealthRecord.Height,
                        Note = member.HealthRecord.Note,
                        BloodType = member.HealthRecord.BloodType,
                    }

                };
                return _memberRepository.Create(newMember);
            }
            else
            {
                return false;
            }
              
        }

        public IQueryable<GetAllMembersViewModel> GetAllMembers()
        {
            var members = _memberRepository.GetAll();
            if (members is null) return Enumerable.Empty<GetAllMembersViewModel>().AsQueryable();

            // manual mapping from Member to MemberViewModel way(1)
            var memberviewmodels = new List<GetAllMembersViewModel>();
            //foreach(var member in members)
            //{
            //    var memberviewmodel = new GetAllMembersViewModel()
            //    {
            //        Id = member.Id,
            //        Name = member.Name,
            //        Email = member.Email,
            //        Phone = member.Phone,
            //        Photo = member.Photo,
            //        Gender = member.Gender.ToString(),
            //    };
            //    memberviewmodels.Add(memberviewmodel);
            //}

            // way number 2
            memberviewmodels = members.Select(m => new GetAllMembersViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString()
            }
            ).ToList();
            return memberviewmodels.AsQueryable();
        }

        public GetMemberDetailsViewModel? GetMemberDetails(int memberId)
        {
            Member? member = _memberRepository.GetById(memberId);
            if (member is null) return null;
            GetMemberDetailsViewModel viewMember = new GetMemberDetailsViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                DateOfBirth = member.DateOfBirth,
                Gender = member.Gender.ToString(),
                BuildinhNo = member.Address?.BuildingNo??0,
                Street = member.Address?.Street??"No Street",
                City = member.Address?.City??"No City",
            };
            var ActiveMembership = _memberPlanRepository.GetAll(X => X.MemberId == memberId && X.Status == "Active").FirstOrDefault();
            if (ActiveMembership is null) return null;
            viewMember.MembershipStartDate = ActiveMembership.Created_At;
            viewMember.MembershipEndDate = ActiveMembership.EndDate;

            var plan = _planRepository.GetById(ActiveMembership.PlanId);
            if(plan is null) return null;
            viewMember.PlanName = plan.Name;
            return viewMember;
        }

        public HealthRecordViewModel? GetMemberHealthDetails(int memberId)
        {
            Member? member = _memberRepository.GetById(memberId);
            if (member is null) return null;
            var HealthRecordViewModel = new HealthRecordViewModel()
            {
                Height = member.HealthRecord.Height,
                Weight = member.HealthRecord.Weight,
                BloodType = member.HealthRecord.BloodType,
                Note = member.HealthRecord.Note,
            };
            return HealthRecordViewModel;
        }
    }
}
