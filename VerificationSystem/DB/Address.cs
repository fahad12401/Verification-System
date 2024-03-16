using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class Address
    {
        public long AddressId { get; set; }
        public long InquiryId { get; set; }

        public string VerificationType { get; set; }
        public string PersonType { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }
        

        [Required]
        public string Address3 { get; set; }

        public string Descriptions { get; set; }



        public string City { get; set; }
        public string Province { get; set; }

        public string RecordedBy { get; set; }
        public DateTime RecordAt { get; set; }

        [NotMapped]
        public string FullAddress { get; set; }


        [ForeignKey("InquiryId")]
        public Inquiry Inquiry { get; set; }

    }
}