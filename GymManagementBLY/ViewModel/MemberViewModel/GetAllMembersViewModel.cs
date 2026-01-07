using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBL.ViewModel.MemberViewModel
{
    public class GetAllMembersViewModel
    {
        public int Id { get; set; }
        public string? Photo { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string Gender { get; set; } = null!;
        public string Phone { get; set; } = null!;

    }
}
