using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class InquiryApplicationUser
    {

        [Key, Column(Order = 0)]
        public long InquiryId { get; set; }

        [Key, Column(Order = 1)]
        public string UserId { get; set; }

        [Key, Column(Order =2)]
        public string VerificationType { get; set; }

        [Key, Column(Order = 3)]
        public string PersonType { get; set; }

        [Key, Column(Order = 4)]
        public long VerificationId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Inquiry Inquiry { get; set; }
    }
}