using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class Branch
    {
        public Branch()
        {
            this.AppUsers = new HashSet<ApplicationUser>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long BranchId { get; set; }

        public string Name { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public Nullable<System.DateTime> DisableDate { get; set; }
        public bool IsActive { get; set; }
        
        public virtual ICollection<ApplicationUser> AppUsers { get; set; }
    }

}