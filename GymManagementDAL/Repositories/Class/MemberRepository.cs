using GymManagementDAL.Context;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Class
{
    public class MemberRepository : GenericRepository<Member>
    {
        public MemberRepository(GymContext _context) : base(_context)
        {
            
        }
    }
}
