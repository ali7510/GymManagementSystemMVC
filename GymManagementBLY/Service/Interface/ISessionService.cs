using GymManagementBL.ViewModel.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.Service.Interface
{
    internal interface ISessionService
    {
        IQueryable<SessionViewModel> GetAllSessions();

        SessionViewModel? GetSessionById(int sessionid);

        bool CreateSession(CreateSessionViewModel session);

        UpdateSessionViewModel? GetUpdateSession(int sessionId);

        bool UpdateSession(int sessionId, UpdateSessionViewModel session);

        bool DeleteSession(int sessionId);



    }
}
