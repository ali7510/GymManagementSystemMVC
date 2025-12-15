using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Booking : BaseEntity
    {
        #region Members
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        #endregion

        #region Sessions
        public int SessionId { get; set; }
        public Session? Session { get; set; }
        #endregion
        // Booking date instead of created at

        public bool isAttended { get; set; }
    }
}
