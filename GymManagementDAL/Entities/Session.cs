using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Session : BaseEntity
    {
        public string? Description { get; set; }
        public int Capacity { get; set; }

        // start date instead of Created at, using fluent api
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }

        #region Category Relationship
        public int Category_Id { get; set; }
        public Category Category { get; set; } = null!;
        #endregion

        #region Trainer Relationship
        public int Trainer_Id { get; set; }
        public Trainer Trainer { get; set; } = null!;
        #endregion

        public ICollection<Booking> Members { get; set; } = null!;
    }
}
