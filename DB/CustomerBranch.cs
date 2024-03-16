using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class CustomerBranch
    {
        public CustomerBranch()
        {
            ConsumerUsers = new HashSet<ApplicationUser>();
        }

        public long CustomerBranchId { get; set; }
        public long CompanyId { get; set; }

        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public DateTime? DisableDate { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<ApplicationUser> ConsumerUsers { get; set; }
    }
}