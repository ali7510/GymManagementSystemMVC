using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel;
using GymManagementBL.Service.Class;
using Microsoft.AspNetCore.Mvc;

//using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        public IActionResult Index()
        {
            var Trainers = _trainerService.GetAllTrainers();
            if (Trainers is null || !Trainers.Any())
            {
                TempData["InfoMessage"] = "No Trainers Available";
                return Content("No Trainers Available");
            }
            return View(Trainers);
        }

        //public IActionResult About() {
    }
}
