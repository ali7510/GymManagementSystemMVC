using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel;
using GymManagementBL.Service.Class;
using Microsoft.AspNetCore.Mvc;
using GymManagementBL.ViewModel.TrainerViewModels;

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
                TempData["ErrorMessage"] = "No Trainers Available";

            }
            return View(Trainers);
        }

        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer Id";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "No Trainer with this Id";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer Id";
                return RedirectToAction(nameof(Index));
            }
            var trainerToUpdate = _trainerService.GetTrainerToUpdate(id);
            if (trainerToUpdate is null)
            {
                TempData["ErrorMessage"] = "No Trainer with this Id";
                return RedirectToAction(nameof(Index));
            }
            return View(trainerToUpdate);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, TrainerToUpdateViewModel updatedTrainer)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer Id";
                return RedirectToAction(nameof(Index));
            }
            bool isUpdated = _trainerService.UpdateTrainerDetails(updatedTrainer, id);
            if (!isUpdated)
            {
                TempData["ErrorMessage"] = "Trainer Details Updation Failed";
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = "Trainer Details Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View(nameof(Create));
        }

        [HttpPost]
        public IActionResult CreateTrainer(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check missing field!");
                return View(nameof(Create), model);
            }


            bool result = _trainerService.CreateTrainer(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Trainer";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer Id";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var trainer = _trainerService.GetTrainerDetails(id);
                if (trainer is null)
                {
                    TempData["ErrorMessage"] = "No member with this Id";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.TrainerId = id;
                return View(nameof(Delete));
            }
        }

        [HttpPost]
        public IActionResult DeleteConfirm([FromForm]int id)
        {
            bool deleted = _trainerService.RemoveTrainer(id);
            if (deleted)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Delete Trainer";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
