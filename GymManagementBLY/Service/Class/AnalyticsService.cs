using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel.AnalyticsViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Class
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AnalyticsViewModel GetAnalyticsData()
        {
            var sessions = _unitOfWork.SessionRepository.GetAll();
            return new AnalyticsViewModel()
            {
                ActiveMembers = _unitOfWork.GetRepository<MemberPlan>().GetAll(x => x.Status == 1).Count(),
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = _unitOfWork.SessionRepository.GetAll(x=>x.StartDate > DateTime.Now).Count(),
                OnGoingSessions = _unitOfWork.SessionRepository.GetAll(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).Count(),
                CompleteSessions = _unitOfWork.SessionRepository.GetAll(x=>x.EndDate < DateTime.Now).Count(),
            };
        }
    }
}
