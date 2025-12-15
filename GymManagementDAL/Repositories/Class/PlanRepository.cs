using GymManagementDAL.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Class
{
    public class PlanRepository : IPlanRepsitory
    {
        private readonly GymContext _context;
        public PlanRepository(GymContext context)
        {
            _context = context;
        }
        public IQueryable<Plan> GetAll()
        {
            return (IQueryable<Plan>) _context.Plans.ToList();
        }

        public Plan? GetById(int id)
        {
            return _context.Plans.Find(id);
        }

        public bool update(Plan plan)
        {
            _context.Plans.Update(plan);
            return _context.SaveChanges() > 0;
        }
    }
}
