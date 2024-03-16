using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VerificationSystem.Models
{
    public class ProductCreateVM
    {

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage ="Company is required")]
        public long CompanyId { get; set; }
        public bool SMSApplicable { get; set; }

    }
    public class ProductEditVM
    {
        public long ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public long CompanyId { get; set; }
        public bool SMSApplicable { get; set; }
    }
}