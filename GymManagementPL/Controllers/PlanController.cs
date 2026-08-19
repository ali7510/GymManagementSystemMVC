using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planservice;

        public PlanController(IPlanService planservice)
        {
            _planservice = planservice;
        }
        public IActionResult Index()
        {
            var plans = _planservice.GetAllPlans();
            if (plans is null || !plans.Any())
            {
                return View("Empty");
            }
            return View(plans);
        }
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planservice.GetPlanById(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "No Plan with this Id";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var planToUpdate = _planservice.GetUpdatePlan(id);
            if (planToUpdate is null)
            {
                TempData["ErrorMessage"] = "No Plan with this Id or Plan has active memberships";
                return RedirectToAction(nameof(Index));
            }
            return View(planToUpdate);
        }

        [HttpPost]
        public IActionResult Edit(int id, UpdatePlanViewModel updatePlan)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                return View(nameof(Edit), updatePlan);
            }
            var isUpdated = _planservice.UpdatePlan(id, updatePlan);
            if (!isUpdated)
            {
                TempData["ErrorMessage"] = "Failed to update Plan";
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = "Plan updated successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Activate(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var isToggled = _planservice.ToggleStatus(id);
            if (!isToggled)
            {
                TempData["ErrorMessage"] = "Failed to toggle Plan status or Plan has active memberships";
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = "Plan status toggled successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
