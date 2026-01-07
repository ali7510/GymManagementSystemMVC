using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel.MemberViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }

        // BaseURL/Member/memberDetails?id
        public IActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member Id";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if(member is null)
            {
                TempData["ErrorMessage"] = "No member with this Id";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        public IActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var healthRecord = _memberService.GetMemberHealthDetails(id);
            if (healthRecord is null)
            {
                return Content("No member with this Id exists");
            }
            return View(healthRecord);
        }

        public IActionResult Create()
        {
            return View(nameof(Create));
        }

        [HttpPost]
        public IActionResult CreateMember(CreateMemberViewModel createMemberViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check missing field!");
                return View(nameof(Create), createMemberViewModel);
            }
            bool result = _memberService.CreateMember(createMemberViewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Member";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member Id";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetUpdateMember(id);
            if(member is null)
            {
                TempData["ErrorMessage"] = "No member with this Id";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public IActionResult MemberEdit([FromRoute]int id, UpdateMemberViewModel updateMemberViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check missing field!");
                return View(nameof(MemberEdit), updateMemberViewModel);
            }
            bool result = _memberService.UpdateMember(id, updateMemberViewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Update Member";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member Id";
            }
            else
            {
                var member = _memberService.GetMemberDetails(id);
                if (member is null)
                {
                    TempData["ErrorMessage"] = "No member with this Id";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.MemberId = id;
            return View(nameof(Delete));
        }

        [HttpPost]
        public IActionResult DeleteConfirm([FromForm] int id)
        {
            bool result = _memberService.DeleteMember(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Delete Member";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
