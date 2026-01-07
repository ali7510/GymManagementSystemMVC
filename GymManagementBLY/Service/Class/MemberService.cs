using AutoMapper;
using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel.HealthRecordViewModels;
using GymManagementBL.ViewModel.MemberViewModel;
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
    public class MemberService : IMemberService
    {
        //private readonly IGenericRepository<Member> _memberRepository;
        //private readonly IGenericRepository<MemberPlan> _memberPlanRepository;
        //private readonly IPlanRepsitory _planRepository;
        //private readonly IGenericRepository<Session> _sessionRepository;
        //private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            //_memberPlanRepository = memberPlanRepository;
            //_planRepository = planRepository;
            //_sessionRepository = sessionRepository;
            //_bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateMember(CreateMemberViewModel member)
        {
            // if this member already exist
            var mem = _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == member.Phone);
            var mem2 = _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == member.Email);

            if (mem is not null || mem2 is not null)
            {
                //Member newMember = new Member()
                //{
                //    Phone = member.Phone,
                //    Email = member.Email,
                //    Name = member.Name,
                //    DateOfBirth = member.DateOfBirth,
                //    Gender = member.Gender,
                //    Address = new Address()
                //    {
                //        City = member.City,
                //        Street = member.Street,
                //        BuildingNo = member.BuildingNumber
                //    },
                //    HealthRecord = new HealthRecord()
                //    {
                //        Weight = member.HealthRecord.Weight,
                //        Height = member.HealthRecord.Height,
                //        Note = member.HealthRecord.Note,
                //        BloodType = member.HealthRecord.BloodType,
                //    }
                //};
                Member newMember = _mapper.Map<CreateMemberViewModel, Member>(member);
                _unitOfWork.GetRepository<Member>().Create(newMember);
                return _unitOfWork.SaveChange()>0;
            }
            else
            {
                return false;
            }
              
        }

        public bool DeleteMember(int memberId)
        {
            Member? member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return false;
            var booking = _unitOfWork.GetRepository<Booking>().GetAll(m=>m.MemberId== memberId).FirstOrDefault();
            if (booking is null) return false;
            int sessionId = booking.SessionId;
            var session  = _unitOfWork.GetRepository<Session>().GetById(sessionId);
            if(session is null) return false;
            var memberships = _unitOfWork.GetRepository<MemberPlan>().GetAll(m => m.MemberId == memberId);
            if (session.EndDate > DateTime.Now)
            {
                return false;
            }
            else
            {
                try
                {
                    if (memberships.Any())
                    {
                        foreach(var membership in memberships)
                        {
                            _unitOfWork.GetRepository<MemberPlan>().Delete(membership);
                        }
                    }
                    _unitOfWork.GetRepository<Member>().Delete(member);
                    return _unitOfWork.SaveChange() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public IQueryable<GetAllMembersViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members is null) return Enumerable.Empty<GetAllMembersViewModel>().AsQueryable();

            // manual mapping from Member to MemberViewModel way(1)
            var memberviewmodels = new List<GetAllMembersViewModel>();
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
            Member? member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;
            GetMemberDetailsViewModel viewMember = _mapper.Map<Member, GetMemberDetailsViewModel>(member);
            var ActiveMembership = _unitOfWork.GetRepository<MemberPlan>().GetAll(X => X.MemberId == memberId).FirstOrDefault();
            if (ActiveMembership is null) return null;
            viewMember.MembershipStartDate = ActiveMembership.CreatedAt;
            viewMember.MembershipEndDate = ActiveMembership.EndDate;

            var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMembership.PlanId);
            if(plan is null) return null;
            viewMember.PlanName = plan.Name;
            return viewMember;
        }

        public HealthRecordViewModel? GetMemberHealthDetails(int memberId)
        {
            Member? member = _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
            {
                Console.WriteLine("NULL, check get repository method");
                return null;
            }
            HealthRecordViewModel healthRecord = _mapper.Map<Member, HealthRecordViewModel>(member);
            if (healthRecord is null) Console.WriteLine("RETURNED HEALTH RECORD IS NULL");
            return healthRecord;
        }

        public UpdateMemberViewModel? GetUpdateMember(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId); if (member is null) return null;
            //var UpdateMemberViewModel = new UpdateMemberViewModel()
            //{
            //    Name = member.Name,
            //    Photo = member.Photo,
            //    Email = member?.Email ?? "NoEmail",
            //    Phone = member?.Phone ?? "NoPhone",
            //    BuildingNumber = member?.Address?.BuildingNo ?? 012,
            //    Street = member?.Address?.Street ?? "NoStreet",
            //    City = member?.Address?.City ?? "Nocity",
            //};
            //return UpdateMemberViewModel;
            return _mapper.Map<Member, UpdateMemberViewModel>(member);
        }

        public bool UpdateMember(int memberId, UpdateMemberViewModel member)
        {
            var storedMember = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (storedMember is null) return false;

            try
            {
                if (storedMember is null || storedMember.Name != member.Name || storedMember.Photo != member.Photo) return false;
                storedMember.Email = member.Email;
                storedMember.Phone = member.Phone;
                storedMember.Address.BuildingNo = member.BuildingNumber;
                storedMember.Address.Street = member?.Street ?? "";
                storedMember.Address.City = member?.City ?? "";
                storedMember.Updated_At = DateTime.Now;
                //_mapper.Map<UpdateMemberViewModel, Member>(member);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            _unitOfWork.GetRepository<Member>().Update(storedMember);
            return _unitOfWork.SaveChange() > 0;
        }
    }
}
