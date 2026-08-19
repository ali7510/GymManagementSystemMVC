using AutoMapper;
using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel.MemberSessionViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Class
{
    public class MemberSessionService : IMemberSessionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Session> _Sessions;
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<MemberPlan> _memberPlanRepository;
        private readonly ISessionRepository _sessionRepository;

        public MemberSessionService(IMapper mapper, IUnitOfWork unitOfWork, IGenericRepository<Session> Sessions, IGenericRepository<Booking> booking, ISessionRepository sessionRepository, IGenericRepository<MemberPlan> memberPlanRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _Sessions = Sessions;
            _bookingRepository = booking;
            _sessionRepository = sessionRepository;
            _memberPlanRepository = memberPlanRepository;
        }
        public IQueryable<GetAllMemberSessionsViewModel>? GetAllMemberSessions()
        {
            var memberSessions = _Sessions.GetAll().Include(x => x.Trainer);
            if (memberSessions == null) return null;
            var sessions = new List<GetAllMemberSessionsViewModel>();
            sessions = _mapper.Map<List<GetAllMemberSessionsViewModel>>(memberSessions.ToList());
            foreach(var session in sessions)
            {
                session.FreeCapacity = GetFreeCapacityCount(session.SessionId)-_sessionRepository.GetBookedSlotsCount(session.SessionId);
            }
            return sessions.AsQueryable();
        }

        public IQueryable<GetAllMembersInBooking>? GetAllMembersInOngoingBookings()
        {
            var membersInBooking = _bookingRepository.GetAll().Include(b => b.Member).Include(b=>b.Session).Where(b => b.Session.StartDate <= DateTime.Now && b.Session.EndDate >= DateTime.Now);
            if (membersInBooking == null) return null!;
            var members = new List<GetAllMembersInBooking>();
            members = _mapper.Map<List<GetAllMembersInBooking>>(membersInBooking.ToList());
            return members.AsQueryable();
        }

        public IQueryable<GetAllMembersInBooking>? GetAllMembersInUpcomingBookings()
        {
            var membersInBooking = _bookingRepository.GetAll().Include(b => b.Member).Include(b => b.Session).Where(b => b.Session.StartDate > DateTime.Now);
            if (membersInBooking == null) return null!;
            var members = new List<GetAllMembersInBooking>();
            members = _mapper.Map<List<GetAllMembersInBooking>>(membersInBooking.ToList());
            return members.AsQueryable();
        }



        public int GetFreeCapacityCount(int sessionId)
        {
            int members = _bookingRepository.GetAll(b => b.SessionId == sessionId).Count();
            int capacity = _Sessions.GetAll(s => s.Id == sessionId).Select(s => s.Capacity).FirstOrDefault();
            int freeCapacity = capacity - members;
            return freeCapacity;
        }

        public IQueryable<GetAllMembersInBooking> GetMembersInSpecificBooking(int sessionId)
        {
            var membersInBooking = _bookingRepository.GetAll(b => b.SessionId == sessionId).Include(b => b.Member);
            if (membersInBooking == null) return null!;
            var members = new List<GetAllMembersInBooking>();
            members = _mapper.Map<List<GetAllMembersInBooking>>(membersInBooking.ToList());
            return members.AsQueryable();
        }



        public bool CreateBooking(CreateMemberSessionViewModel booking)
        {
            try
            {
                var isExisted = _bookingRepository.GetAll(b => b.MemberId == booking.MemberId).Any();
                if (isExisted) return false;

                var hasMemberPlan = _memberPlanRepository.GetAll(mp => mp.MemberId == booking.MemberId && mp.EndDate < DateTime.Now).Any();
                if (hasMemberPlan) return false;

                if (booking is null)
                {
                    return false;
                }
                Booking newBooking = _mapper.Map<Booking>(booking);
                var session = _Sessions.GetById(booking.SessionId);
                if (session != null)
                {
                    newBooking.BookingStatus = session.StartDate > DateTime.Now ? BookingStatus.Upcoming : BookingStatus.Ongoing;
                }
                _bookingRepository.Create(newBooking);
                bool isCreated = _unitOfWork.SaveChange() > 0;
                return isCreated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool MarkMemberAsAttended(int memberId, int sessionId)
        {
            var booking = _bookingRepository.GetAll().Where(b=>b.MemberId == memberId && b.SessionId == sessionId).FirstOrDefault();
            if (booking is null)
            {
                return false;
            }
            if (booking.isAttended == true)
            {
                return false;
            }
            booking.isAttended = true;
            _bookingRepository.Update(booking);
            bool isUpdated = _unitOfWork.SaveChange() > 0;
            return isUpdated;
        }
    }
}
