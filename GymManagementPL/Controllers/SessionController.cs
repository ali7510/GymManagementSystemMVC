using GymManagementBL.Service.Class;
using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionservice;

        public SessionController(ISessionService sessionservice)
        {
            _sessionservice = sessionservice;
        }
        public IActionResult Index()
        {
            var sessions = _sessionservice.GetAllSessions();
            if (sessions is null || !sessions.Any())
            {
                return View("Index");
            }
            return View(sessions);
        }

        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction("Index");
            }
            var session = _sessionservice.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
            return View(session);
        }
        public IActionResult Create()
        {
            LoadDropDownCategory();
            LoadDropDownTrainer();
            return View(nameof(Create));
        }

        [HttpPost]
        public IActionResult CreateSession(CreateSessionViewModel session)
        {
            if (session == null)
            {
                TempData["ErrorMessage"] = "Invalid session data.";
                return RedirectToAction("Index");
            }
            var isCreated = _sessionservice.CreateSession(session);
            if (!isCreated)
            {
                LoadDropDownCategory();
                LoadDropDownTrainer();
                TempData["ErrorMessage"] = "Failed to create session. Please check the details and try again.";
                return RedirectToAction("Create", session);
            }

            TempData["SuccessMessage"] = "Session created successfully.";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction("Index");
            }
            var session = _sessionservice.GetUpdateSession(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
            LoadDropDownTrainer();
            return View(session);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute]int id, UpdateSessionViewModel session)
        {
            if (session == null)
            {
                TempData["ErrorMessage"] = "Invalid session data.";
                return RedirectToAction("Index");
            }
            var isUpdated = _sessionservice.UpdateSession(id, session);
            if (!isUpdated)
            {
                LoadDropDownTrainer();
                TempData["ErrorMessage"] = "Failed to update session. Please check the details and try again.";
                return RedirectToAction("Edit", session);
            }
            TempData["SuccessMessage"] = "Session updated successfully.";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction("Index");
            }
            var isDeleted = _sessionservice.GetSessionById(id);
            if (isDeleted is null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction("Index");
            }
            ViewBag.SessionId = id;
            return View(nameof(Delete));
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var isDeleted = _sessionservice.DeleteSession(id);
            if (!isDeleted)
            {
                TempData["ErrorMessage"] = "Failed to delete session. Please try again.";
                return RedirectToAction("Index");
            }
            TempData["SuccessMessage"] = "Session deleted successfully.";
            return RedirectToAction("Index");
        }

        private void LoadDropDownCategory()
        {
            var category = _sessionservice.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(category, "Id", "Name");
        }
        private void LoadDropDownTrainer()
        {

            var trainers = _sessionservice.GetTrainerForDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }
    }
}
