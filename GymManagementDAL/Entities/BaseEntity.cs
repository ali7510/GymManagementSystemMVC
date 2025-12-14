using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    internal class BaseEntity
    {
        public int Id { get; set; }
        public DateOnly Created_At { get; set; }
        public DateOnly Updated_At { get; set; }
    }
}
