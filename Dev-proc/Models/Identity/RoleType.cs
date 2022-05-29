using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dev_proc.Models.Identity
{
    public enum RoleType
    {
        [Display(Name = "Administrator")]
        Administrator = 0,
        [Display(Name = "Dean")]
        Dean = 1,
        [Display(Name = "Company")]
        Company = 2
    }
}
