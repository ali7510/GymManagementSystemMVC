using GymManagementDAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    internal class Trainer : GymUser
    {
        public Speciality Speciality{ get; set; }

        // created at will be Hire_Date using fluent api

        public ICollection<Session> Trainer_Sessions { get; set; } = null!;
    }
}
