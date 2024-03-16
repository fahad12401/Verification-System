using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VerificationSystem.Models
{
    public class CompanyVM
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }



        [Required]
        [Display(Name = "Description")]
        [StringLength(100, ErrorMessage = "The {0} must be between {1} to {2} characters long.", MinimumLength = 2)]
        public string Description { get; set; }


        [Required]
        [Display(Name = "Address")]
        [StringLength(100, ErrorMessage = "The {0} must be between {1} to {2} characters long.", MinimumLength = 2)]
        public string Address { get; set; }

        [Display(Name = "Phone")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only Numbers.")]
        public string Phone { get; set; }


        [Display(Name = "Phone2")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only Numbers.")]
        public string Phone2 { get; set; }




        public string RecordBy { get; set; }
        public DateTime RecordAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }


        [Display(Name = "Is Disable ?")]
        public bool Disabled { get; set; }

        public DateTime? DisableDate { get; set; }
    }
}