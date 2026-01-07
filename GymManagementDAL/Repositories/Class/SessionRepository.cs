using GymManagementDAL.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Class
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymContext _context;

        public SessionRepository(GymContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return _context.Sessions.Include(x => x.Trainer).Include(x => x.Category).ToList().AsQueryable();
        }

        public int GetBookedSlotsCount(int sessionId)
        {
            return _context.MemberSessions.Where(s => s.SessionId == s.SessionId).Count();
        }

        public Session? GetSessionsWithTrainerAndCategory(int sessionId)
        {
            return _context.Sessions.Include(x => x.Trainer).Include(x => x.Category).FirstOrDefault(x=>x.Id == sessionId);
        }
    }
}
