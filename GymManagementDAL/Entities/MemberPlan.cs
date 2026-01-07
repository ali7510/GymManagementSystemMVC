using GymManagementDAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class MemberPlan : BaseEntity
    {
        #region Members
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        #endregion
        #region Plans
        public int PlanId { get; set; }
        public Plan? Plan { get; set; }
        #endregion
        public DateTime EndDate { get; set; }

        // started at instead of created at

        public int Status
        {
            get
            {
                if (EndDate > DateTime.Now)
                {
                    return (int)MembershipStatus.Active;
                }
                else
                {
                    return (int)MembershipStatus.Expired;
                }
            }
        }
    }
}
