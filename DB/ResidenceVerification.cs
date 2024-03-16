using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class ResidenceVerification
    {

        public ResidenceVerification()
        {
        }
        public long ResidenceVerificationId { get; set; }
        public long InquiryId { get; set; }// for different persons type thats y i took it like master detail otherwise i toook it like one to one 
        public string PersonType { get; set; }


        public string PersonName { get; set; }
        public string PersonCNIC { get; set; }
        public string PersonContactNo { get; set; }
        public string ResidenceAddress { get; set; }
        public string NearestLandMark { get; set; }




        public string GeneralComments { get; set; }
        public string OutComeVerification { get; set; }
        public string GpsLocation { get; set; }


        public string GpsURL { get; set; }

        public string Status { get; set; }
        public DateTime StatusDate { get; set; }


        //Quality Check
        public double? Price { get; set; }
        public string QCComments { get; set; }
        public string VerifiedBy { get; set; }

        //public long? BeforeQCId { get; set; }
        //public ResidenceVerification BeforeQCVerification { get; set; }


        public virtual Inquiry Inquiry { get; set; }

        public virtual ResidenceDetail ResidenceDetail {get;set;}
        public virtual ResidenceProfile  ResidenceProfile { get; set; }
        public virtual NeighbourCheck NeighbourCheck { get; set; }
        
        [NotMapped]
        public virtual List<InquiryApplicationUser> InquiryApplicationUser
        {
            get
            {
                if (Inquiry != null)
                {
                    return Inquiry.InquiryApplicationUsers.Where(x => x.PersonType == this.PersonType && x.VerificationType == constant.VerificationType.Residence).ToList();
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

                images = Inquiry.Images.Where(x => x.VerificationType == constant.VerificationType.Residence 
                && x.PersonType == this.PersonType && x.VerificationId == ResidenceVerificationId).ToList();
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
                if (statuses != null ) return statuses;

                //fetch statuses from inquiry..
                if (this.Inquiry != null)
                {
                    
                        statuses = Inquiry.Statuses.Where(x => x.PersonType == PersonType &&
                                      x.VerificationType == constant.VerificationType.Residence &&
                                       x.VerificationId == ResidenceVerificationId).ToList();

                    return statuses;
                    
                }
                return new List<DB.Status>();
            }
        }

    }
    
    public class ResidenceDetail
    {
        [Key, ForeignKey("ResidenceVerification")]
        public long ResidenceDetailId { get; set; }
        public bool IsApplicantAvailable { get; set; }//Did you meet the applicant

        public string NameOfPersonToMet { get; set; } //if applicant not available.
        public string RelationWithApplicant { get; set; }//RelatioinWithApplicant if applicant not available.
        public bool WasActualAddressSame { get; set; }//Was actual address same as above?
        public string CorrectAddress { get; set; }//Give correct addres if address not same ..

        public string PhoneNo { get; set; }

        public bool LivesAtGivenAddress { get; set; }//does applicant lives at the given address
        public string PermanentAddress { get; set; }//Permanent address if not lives at the given address
        public string SinceHowLongLiving { get; set; }//Since how long applicant is living on the same address.
        public string CNICNo { get; set; }//applicant cnic no.

        public string ReferenceRelation { get; set; }
        public string SinceHowLongReferenceKnows { get; set; }



        public virtual ResidenceVerification ResidenceVerification { get; set; }
    }
    public class ResidenceProfile
    {

        [Key, ForeignKey("ResidenceVerification")]
        public long ResidenceVerificationId { get; set; }
        public string TypeOfResidence { get; set; }//House - Portion - Apartment
        public string ApplicantIsA { get; set; } //Owner - Tenant - Other
        public string MentionOther { get; set; }


        public double? MentionRent { get; set; } // if Tenant mention rent
        public string SizeApproxArea { get; set; }
        public string UtilizationOfResidence { get; set; }// Residential - Commercial
        public string RentDeedVerified { get; set; } //If application means if Tenant

        public string Neighborhood { get; set; }  // neighborhood fields
        public string AreaAccessibility { get; set; }
        public string ResidentsBelongsTo { get; set; }
        public string RepossessionInTheArea { get; set; }

        public virtual ResidenceVerification ResidenceVerification { get; set; }
    }
    public class NeighbourCheck
    {
        [Key, ForeignKey("ResidenceVerification")]
        public long NeighbourCheckId { get; set; }
        public string NeighborsName { get; set; }
        public string NeighborsAddress { get; set; }
        public bool KnowsApplicant { get; set; } //Does Neighbor know the applicant
        public string KnowsHowLong { get; set; }// if neighbor knows the applicant then how long.
        public string NeighborComments { get; set; } // Comments Regarding the applicant


        public string NeighborsName2 { get; set; }
        public string NeighborsAddress2 { get; set; }
        public bool KnowsApplicant2 { get; set; } //Does Neighbor know the applicant
        public string KnowsHowLong2 { get; set; }// if neighbor knows the applicant then how long.
        public string NeighborComments2 { get; set; } // Comments Regarding the applicant


        public virtual ResidenceVerification ResidenceVerification { get; set; }

    }


}