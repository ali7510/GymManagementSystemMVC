using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel.MemberPlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMemberPlanService _memberPlanService;
        private readonly IMemberService _memberservice;
        private readonly IPlanService _planservice;

        public MembershipController(IMemberPlanService memberPlanService, IMemberService memberservice, IPlanService planservice)
        {
            _memberPlanService = memberPlanService;
            _memberservice = memberservice;
            _planservice = planservice;
        }
        public IActionResult Index()
        {
            var memberships = _memberPlanService.GetAllMemberships();
            if (memberships is null)
            {
                TempData["ErrorMessage"] = "No memberships found.";
            }
            else
            {
                TempData["SuccessMessage"] = "Memberships loaded successfully.";
            }
            return View(memberships);
        }

        public IActionResult Create()
        {
            var Members = _memberservice.GetAllMembers();
            var Plans = _planservice.GetAllPlans();
            if(Members is null)
            {
                TempData["ErrorMessage"] = "Unable to load members.";
                return RedirectToAction("Index");
            }
            else if (Plans is null)
            {
                TempData["ErrorMessage"] = "Unable to load plans.";
                return RedirectToAction("Index");
            }
            ViewBag.Members = Members;
            ViewBag.Plans = Plans;
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm]int memberId, [FromForm]int planId)
        {
            if(memberId <= 0 || planId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member or plan selection.";
                return RedirectToAction("Create");
            }
            var member = _memberservice.GetAllMembers().FirstOrDefault(m => m.Id == memberId);
            var plan = _planservice.GetPlanById(planId);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Selected member does not exist.";
                return RedirectToAction("Create");
            }
            else if(plan is null)
            {
                TempData["ErrorMessage"] = "Selected plan does not exist.";
                return RedirectToAction("Create");
            }
            var isThereMembership = _memberPlanService.GetMembershipsOfMember(memberId);
            if (isThereMembership)
            {
                TempData["ErrorMessage"] = "Member already has an active membership.";
                return RedirectToAction("Create");
            }
            var newMembership = new CreateMembershipViewModel
            {
                MemberId = memberId,
                PlanId = planId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(plan.DurationDays)
            };
            var isCreated = _memberPlanService.CreateMemberPlan(newMembership);
            if (isCreated)
            {
                TempData["SuccessMessage"] = "Membership created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create membership.";
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public IActionResult Cancel([FromForm]int memberId, [FromForm]int planId)
        {
            var isCancelled = _memberPlanService.DeleteMemberPlan(memberId, planId);
            if (isCancelled)
            {
                TempData["SuccessMessage"] = "Membership cancelled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to cancel membership.";
            }
            return RedirectToAction("Index");
        }
    }
}
