using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace VerificationSystem.DB
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters.")]
        [StringLength(50, ErrorMessage = "The {0} must be between {1} to {2} characters long.", MinimumLength = 3)]
        [Column(TypeName = "varchar")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters.")]
        [StringLength(50, ErrorMessage = "The {0} must be between {1} to {2} characters long.", MinimumLength = 3)]
        [Column(TypeName = "varchar")]
        public string LastName { get; set; }

        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }
        public long? CustomerBranchId { get; set; }

        public string Address { get; set; }
        public string CustomerCode { get; set; }
        
        public bool IsHead { get; set; }
        public long? ParentCode { get; set; }


        [Required]
        public string Code { get; set; }
        public Nullable<System.DateTime> DisableDate { get; set; }

        public string RecordBy { get; set; }

        [Required]
        public System.DateTime RecordAt { get; set; }

        public string UpdateBy { get; set; }

        public System.DateTime? UpdateAt { get; set; }




        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
        [ForeignKey("CustomerBranchId")]
        public virtual CustomerBranch CustomerBranch { get; set;    }

        public virtual ICollection<InquiryApplicationUser> InquiryApplicationUsers { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaim(new Claim(ClaimTypes.Name, this.UserName));
            //userIdentity.AddClaim(new Claim("CompanyId", this.CompanyId.ToString()));

            if (this.CustomerBranchId != null)
            {
                userIdentity.AddClaim(new Claim("CustomerBranchId", this.CustomerBranchId.ToString()));
                if (!string.IsNullOrEmpty(this.CustomerBranch.Name))
                {
                    userIdentity.AddClaim(new Claim("CustomerBranch", this.CustomerBranch.Name));
                }
            }



            if (this.BranchId != null)
            {
                userIdentity.AddClaim(new Claim("BranchId", this.BranchId.ToString()));
                if (!string.IsNullOrEmpty(this.Branch.Name))
                {
                    userIdentity.AddClaim(new Claim("Branch", this.Branch.Name));
                }
            }



            if (!string.IsNullOrEmpty(this.CustomerCode))
            {
                userIdentity.AddClaim(new Claim("CustomerId", this.CustomerCode));
            }


            

            if (this.CompanyId != null )
            {
                userIdentity.AddClaim(new Claim("CompanyId", this.CompanyId.ToString()));
                if (!string.IsNullOrEmpty(this.Company.Name))
                {
                    userIdentity.AddClaim(new Claim("CompanyName", this.Company.Name));
                }
            }


            // Add custom user claims here
            return userIdentity;
        }






    }

}