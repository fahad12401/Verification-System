using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class TenantVerification
    {
        public long TenantVerificationId { get; set; }
        public long InquiryId { get; set; }

        public string PersonType { get; set; }

        public string PersonName { get; set; }
        public string PersonContactNo { get; set; }
        
        public string TenantAddress { get; set; }
        public string NearestLandMark { get; set; }



        public string TenantName { get; set; }
        public string TenantContactNo { get; set; }
        public string TenantCNIC { get; set; }
        public string TenancyPeriod { get; set; }
        public double TenantRent { get; set; }

        public string Status { get; set; }
        public DateTime StatusDate { get; set; }

        public string GeneralComments { get; set; }
        public string OutComeVerification { get; set; }

        public string GpsLocation { get; set; }
        public string GpsURL { get; set; }

        //Quality check fields
        public double? Price { get; set; }
        public string QCComments { get; set; }
        public string VerifiedBy { get; set; }

        public virtual Inquiry Inquiry { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public virtual List<InquiryApplicationUser> InquiryApplicationUser
        {
            get
            {
                if (Inquiry != null)
                {
                    return Inquiry.InquiryApplicationUsers.Where(x => x.PersonType == this.PersonType && x.VerificationType == constant.VerificationType.Tenant).ToList();
                }

                return null;
            }
        }

        [NotMapped]
        private List<InquiryImage> images;
        [NotMapped]
        public List<InquiryImage> Images
        {
            get
            {
                if (images != null)
                    return images;

                if (Inquiry == null)
                    return null;

                images = Inquiry.Images.Where(x => x.VerificationType == constant.VerificationType.Tenant
                && x.PersonType == this.PersonType && x.VerificationId == TenantVerificationId).ToList();
                return images;
            }
        }

        [NotMapped]
        private List<Status> statuses;

        [NotMapped]
        public List<Status> Statuses
        {
            get
            {
                if (statuses != null) return statuses;

                //fetch statuses from inquiry..
                if (this.Inquiry != null)
                {

                    statuses = Inquiry.Statuses.Where(x => x.PersonType == PersonType &&
                                  x.VerificationType == constant.VerificationType.Tenant &&
                                   x.VerificationId == TenantVerificationId).ToList();
 
                    return statuses;

                }
                return new List<DB.Status>();
            }
        }
    }
}