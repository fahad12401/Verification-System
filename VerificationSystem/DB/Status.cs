using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class Status
    {
        
        public long StatusId { get; set; }
        
        public string UserId { get; set; }
        public long InquiryId { get; set; }
        public string StatusMessage { get; set; }
        public DateTime StatusDate { get; set; }
        public string VerificationType { get; set; }
        public string PersonType { get; set; }

        public string Descriptions { get; set; }

        public long VerificationId { get; set; }

        public virtual Inquiry Inquiry { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUsers { get; set; }

    }
    
}