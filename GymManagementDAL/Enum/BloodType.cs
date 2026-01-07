using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Enum
{
    public enum BloodType
    {
        [Display(Name = "A+")]
        Aplus = 1,
        [Display(Name = "A-")]
        Aminus = 2,
        [Display(Name = "B+")]
        Bplus = 3,
        [Display(Name = "B-")]
        Bminus = 4,
        [Display(Name = "AB+")]
        ABplus = 5,
        [Display(Name = "AB-")]
        ABminus = 6,
        [Display(Name = "O+")]
        Oplus = 7,
        [Display(Name = "O-")]
        Ominus = 8,
    }
}
