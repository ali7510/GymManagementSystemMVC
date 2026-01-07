using GymManagementBL.ViewModel.PlanViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Interface
{
    internal interface IPlanService
    {
        IQueryable<GetAllPlansViewModel> GetAllPlans();

        GetAllPlansViewModel GetPlanById(int id);

        UpdatePlanViewModel GetUpdatePlan(int planId);

        bool UpdatePlan(int planId, UpdatePlanViewModel updatePlan);

        bool ToggleStatus(int planId);

    }
}
