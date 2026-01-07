using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel.MemberViewModel;
using GymManagementBL.ViewModel.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Class;
using GymManagementDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Class
{
    internal class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<GetAllPlansViewModel> GetAllPlans()
        {
            return _unitOfWork.GetRepository<Plan>()
            .GetAll()
            .Select(p => new GetAllPlansViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                Price = p.Price,
                IsActive = p.IsActive
            });
        }

        public GetAllPlansViewModel GetPlanById(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan is null) return null!;
            return new GetAllPlansViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive,
            };
        }

        public UpdatePlanViewModel GetUpdatePlan(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || hasActiveMembership(planId)) return null!;

            return new UpdatePlanViewModel()
            {
                PlanName = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
            };
        }

        public bool ToggleStatus(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || hasActiveMembership(planId)) return false;

            plan.IsActive = !plan.IsActive;
            plan.Updated_At = DateTime.UtcNow;
            try
            {
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChange() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePlan(int planId, UpdatePlanViewModel updatePlan)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || hasActiveMembership(planId)) return false;

            plan.Description = updatePlan.Description;
            plan.Price = updatePlan.Price;
            plan.DurationDays = updatePlan.DurationDays;
            plan.Name = updatePlan.PlanName;

            _unitOfWork.GetRepository<Plan>().Update(plan);
            return _unitOfWork.SaveChange()>0;
        }

        private bool hasActiveMembership(int planId)
        {
            var plan = _unitOfWork.GetRepository<MemberPlan>().GetAll(m=>m.PlanId == planId && m.Status == 1).Any();
            return plan;

        }
    }
}
