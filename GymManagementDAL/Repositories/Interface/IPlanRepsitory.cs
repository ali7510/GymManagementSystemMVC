using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interface
{
    public interface IPlanRepsitory
    {
        IQueryable<Plan> GetAll();

        Plan? GetById(int id);

        bool update(Plan plan); 
    }
}
