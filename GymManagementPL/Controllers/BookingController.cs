using AutoMapper;
using GymManagementBL.Service.Class;
using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel.MemberSessionViewModel;
using GymManagementBL.ViewModel.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace GymManagementPL.Controllers
{
    public class BookingController : Controller
    {
        private readonly IMemberSessionService _memberSessionService;
        private readonly IMemberService _memberService;
        private readonly ITrainerService _trainerService;
        private readonly IMemberPlanService _memberPlanService;
        private readonly ISessionService _sessionService;
        private readonly IMapper _mapper;

        public BookingController(IMemberSessionService memberSessionService, IMemberService memberService, ITrainerService trainerService, IMemberPlanService memberPlanService, ISessionService sessionService, IMapper mapper)
        {
            _memberSessionService = memberSessionService;
            _memberService = memberService;
            _trainerService = trainerService;
            _memberPlanService = memberPlanService;
            _sessionService = sessionService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var memberSessions = _memberSessionService.GetAllMemberSessions();
            return View(memberSessions);
        }

        public IActionResult Create()
        {
            var members = _memberService.GetAllMembers();
            var sessions = _sessionService.GetAllSessions();
            ViewBag.Sessions = sessions;
            return View(members);
        }

        [HttpPost]
        public IActionResult Create([FromForm] int memberId, [FromForm] int sessionId)
        {
            CreateMemberSessionViewModel booking = new CreateMemberSessionViewModel
            {
                MemberId = memberId,
                SessionId = sessionId,
                isAttended = false,
                BookingDate = DateTime.Now,
                Updated_At = DateTime.Now
            };
            bool isCreated = _memberSessionService.CreateBooking(booking);
            if (isCreated)
            {
                TempData["SuccessMessage"] = "Booking created successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create booking. Please try again.";
            }
            return RedirectToAction("Index");
        }


        //public IActionResult ViewUpcomingMembers(int sessionId)
        //{
        //    //IQueryable? membersSessions = _memberSessionService.GetAllMemberSessions().Where(m=>m.SessionId == sessionId && m.Date > DateOnly.FromDateTime(DateTime.Now)).ToList().AsQueryable();
        //}

        [HttpGet]
        public IActionResult ViewOngoingMembers(int sessionId)
        {
            var memberSessions = _memberSessionService?.GetAllMembersInOngoingBookings();
            if (memberSessions is null)
            {
                TempData["ErrorMessage"] = "No members found for this session.";
                return RedirectToAction("Index");
            }
            return View(memberSessions);
        }

        [HttpGet]
        public IActionResult ViewUpcomingMembers(int sessionId)
        {
            var memberSessions = _memberSessionService?.GetAllMembersInUpcomingBookings();
            if (memberSessions is null)
            {
                TempData["ErrorMessage"] = "No members found for this session.";
                return RedirectToAction("Index");
            }
            return View(memberSessions);
        }

        [HttpPost]
        public IActionResult MarkMemberAttendance(int memberId, int sessionId, string distinction)
        {
            bool result = _memberSessionService.MarkMemberAsAttended(memberId, sessionId);
            if (distinction == "ongoing")
            {
                return RedirectToAction("ViewOngoingMembers", new { sessionId });
            }
            else
            {
                return RedirectToAction("ViewUpcomingMembers", new { sessionId });
            }

        }
    }
}
