using GymManagementDAL.Context;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Class
{
    public class SessionRepository : GenericRepository<Session>
    {
        public SessionRepository(GymContext context) : base(context)
        {
        }
    }
}
