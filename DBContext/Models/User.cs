using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SharedHardware.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser<Guid>
    {
        public bool IsOrganization { get; set; }
        public ICollection<Credit> ToReceiveCredits { get; set; }
        public ICollection<Credit> ToPayCredits { get; set; }
        public bool BlockedForPaidResources { get; set;  } //if user has huge credit value - do not allow him to use paid hardware
        //in general users should deal with each other about payment and confirm that their deal was successful at web site
    }
}
