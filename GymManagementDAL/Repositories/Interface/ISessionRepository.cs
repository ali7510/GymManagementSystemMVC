using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interface
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IQueryable<Session> GetAllSessionsWithTrainerAndCategory();
        Session? GetSessionsWithTrainerAndCategory(int sessionId);

        int GetBookedSlotsCount(int sessionId);
    }
}
