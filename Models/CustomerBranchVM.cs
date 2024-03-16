using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VerificationSystem.Models
{
    public class CustomerBranchCreateVM
    {
        [Display(Name = "Company")]
        public long CompanyId { get; set; }
        
        [Display(Name= "Branch Name"), Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }


        public string Province { get; set; }

        [Display(Name = "Phone 1")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only Numbers.")]
        public string Phone1 { get; set; }

        [Display(Name = "Phone 2")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only Numbers.")]
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public bool Disabled { get; set; }


    }

    public class CustomerBranchEditVM
    {

        public long CustomerBranchId { get; set; }
        [Display(Name = "Company")]
        public long CompanyId { get; set; }

        [Display(Name = "Branch Name")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }


        public string Province { get; set; }
        [Display(Name = "Phone 1")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only Numbers.")]
        public string Phone1 { get; set; }

        [Display(Name = "Phone 2")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only Numbers.")]
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public bool Disabled { get; set; }


    }

}