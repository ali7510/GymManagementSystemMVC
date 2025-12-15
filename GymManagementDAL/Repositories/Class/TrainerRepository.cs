using GymManagementDAL.Context;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Class
{
    public class TrainerRepository : GenericRepository<Trainer>
    {
        public TrainerRepository(GymContext context) : base(context)
        {
        }
    }
}
