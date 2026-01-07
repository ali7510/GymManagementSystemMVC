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
    internal class BookingRepository : GenericRepository<Booking>
    {
        public BookingRepository(GymContext context) : base(context)
        {
        }
    }
}
