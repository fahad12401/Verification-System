using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class SalarySlipVerification
    {
        public long SalarySlipVerificationId { get; set; }
        public long InquiryId { get; set; }


        public string PersonType { get; set; }
        public string PersonName { get; set; }
        public string PersonContactNo { get; set; }


        public string OfficeName { get; set; }
        public string OfficeAddress { get; set; }
        public string NearestLandMark { get; set; }

        public string  PaySlipPath { get; set; }


        public string GeneralComments { get; set; }
        public string OutComeVerification { get; set; }
        public string GpsLocation { get; set; }

        public string GpsURL { get; set; }

        public string Status { get; set; }
        public DateTime StatusDate { get; set; }

        //Quality Check
        public string QCComments { get; set; }
        public double? Price { get; set; }
        public string VerifiedBy { get; set; }


        public virtual Inquiry Inquiry { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public virtual List<InquiryApplicationUser> InquiryApplicationUser
        {
            get
            {
                if (Inquiry != null)
                {
                    return Inquiry.InquiryApplicationUsers.Where(x => x.PersonType == this.PersonType && x.VerificationType == constant.VerificationType.SalarySlip).ToList();
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

                images = Inquiry.Images.Where(x => x.VerificationType == constant.VerificationType.SalarySlip
                && x.PersonType == this.PersonType && x.VerificationId == SalarySlipVerificationId).ToList();
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
                                  x.VerificationType == constant.VerificationType.SalarySlip &&
                                   x.VerificationId == SalarySlipVerificationId).ToList();
 
                    return statuses;

                }
                return new List<DB.Status>();
            }
        }
    }

}