using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    internal class Session : BaseEntity
    {
        public string? Description { get; set; }
        public int Capacity { get; set; }

        // start date instead of Created at, using fluent api
        public DateOnly? EndDate { get; set; }

        #region Category Relationship
        public int Category_Id { get; set; }
        public Category? Category { get; set; }
        #endregion

        #region Trainer Relationship
        public int Trainer_Id { get; set; }
        public Trainer? Trainer { get; set; }
        #endregion

        public ICollection<Booking> Members { get; set; } = null!;
    }
}
