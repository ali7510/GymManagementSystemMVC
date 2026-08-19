using AutoMapper;
using AutoMapper.QueryableExtensions;
using GymManagementBL.Service.Interface;
using GymManagementBL.ViewModel.SessionViewModels;
using GymManagementBL.ViewModel.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interface;
using GymManagmentBLL.ViewModels.SessionViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Class
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel session)
        {
            try
            {
                if (!CategoryIsExist(session.CategoryId))
                    throw new Exception("Category does not exist");

                if (!TrainerIsExist(session.TrainerId))
                    throw new Exception("Trainer does not exist");

                if (!IsDateValid(session))
                    throw new Exception("Invalid session date");
                if (session.Capacity > 25 || session.Capacity < 0) return false;

                var sessionEntity = _mapper.Map<CreateSessionViewModel, Session>(session);
                if(sessionEntity is null) throw new Exception("Session Mapping failed");
                _unitOfWork.GetRepository<Session>().Create(sessionEntity);
                return _unitOfWork.SaveChange() > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public IQueryable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (sessions is null) return Enumerable.Empty<SessionViewModel>().AsQueryable();
            //return sessions.Select(s => new SessionViewModel
            //{
            //    Id = s.Id,
            //    CategoryName = s.Category.CategoryName,
            //    Description = s.Description,
            //    TrainerName = s.Trainer.Name,
            //    Capacity = s.Capacity,
            //    CreatedAt = s.CreatedAt,
            //    EndDate = s.EndDate,
            //    AvailableSlots = s.Capacity - _unitOfWork.SessionRepository.GetBookedSlotsCount(s.Id)
            //});

            var mappedSessions = sessions.ProjectTo<SessionViewModel>(_mapper.ConfigurationProvider);

            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetBookedSlotsCount(session.Id);
            }
            return mappedSessions;
        }

        public IQueryable<TrainerSelectViewModel> GetTrainerForDropDown()
        {
            var query = _unitOfWork.GetRepository<Trainer>().GetAll();

            return query.ProjectTo<TrainerSelectViewModel>(_mapper.ConfigurationProvider).AsQueryable();
        }

        public IQueryable<CategorySelectViewModel> GetCategoryForDropDown()
        {
            var query = _unitOfWork.GetRepository<Category>().GetAll();

            return query.ProjectTo<CategorySelectViewModel>(_mapper.ConfigurationProvider).AsQueryable();
        }

        public SessionViewModel? GetSessionById(int sessionid)
        {
            var session = _unitOfWork.SessionRepository.GetSessionsWithTrainerAndCategory(sessionid);
            if (session is null) return null!;
            //SessionViewModel sessionView = new SessionViewModel()
            //{
            //    Id = sessionid,
            //    CategoryName = session.Category.CategoryName,
            //    Description = session.Description,
            //    TrainerName = session.Trainer.Name,
            //    Capacity = session.Capacity,
            //    CreatedAt = session.CreatedAt,
            //    EndDate = session.EndDate,
            //    AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetBookedSlotsCount(sessionid)
            //};
            //return sessionView;

            var mappedSession = _mapper.Map<Session, SessionViewModel>(session);
            mappedSession.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetBookedSlotsCount(mappedSession.Id);
            return mappedSession;
        }

        public bool DeleteSession(int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (session is null) return false;
                if (!isSessionAvailableToDelete(sessionId)) return false;
                _unitOfWork.SessionRepository.Delete(session);
                return _unitOfWork.SaveChange() > 0;
            }
            catch { return false; }
        }

        public UpdateSessionViewModel? GetUpdateSession(int sessionId)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (session is null || !IsSessionAvailableToUpdate(session)) return null!;

            var mappedSession = _mapper.Map<UpdateSessionViewModel>(session);
            return mappedSession;
        }

        public bool UpdateSession(int sessionId, UpdateSessionViewModel session)
        {
            try
            {
                var updatedSession = _unitOfWork.SessionRepository.GetById(sessionId);
                if (updatedSession is null || !IsSessionAvailableToUpdate(updatedSession)) return false;
                if (!TrainerIsExist(updatedSession.Trainer_Id) || !CategoryIsExist(updatedSession.Category_Id)) return false;
                _mapper.Map(session, updatedSession);
                updatedSession.Updated_At = DateTime.Now;
                _unitOfWork.GetRepository<Session>().Update(updatedSession);
                return _unitOfWork.SaveChange() > 0;
            }
            catch
            {
                return false;
            }
        }

        #region HelperMethods
        private bool CategoryIsExist(int CategoryId)
        {
            var isExist = _unitOfWork.GetRepository<Category>().GetById(CategoryId);
            if (isExist is null) return false;
            return true;
        }
        private bool TrainerIsExist(int TrainerId)
        {
            var isExist = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (isExist is null) return false;
            return true;
        }

        private bool IsDateValid(CreateSessionViewModel session)
        {
            return session.EndDate > session.StartDate;
        }

        private bool IsSessionAvailableToUpdate(Session session)
        {
            if (session is null) return false;
            // if session is complete
            if (session.EndDate < DateTime.Now) return false;
            // if sessin doesn't have booking slots
            var hasActiveBooking = _unitOfWork.SessionRepository.GetBookedSlotsCount(session.Id) > 0;
            if (hasActiveBooking) return true;
            return false;
        }

        private bool isSessionAvailableToDelete(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (session is null) return false;
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
            if (session.StartDate > DateTime.Now) return false;
            var hasActiveBooking = _unitOfWork.SessionRepository.GetBookedSlotsCount(sessionId) > 0;
            return hasActiveBooking;
        }



        #endregion
    }
}
