using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerificationSystem.DB
{
    public class WorkOfficeVerification
    {
        public long WorkOfficeVerificationId { get; set; }

        public long InquiryId { get; set; }

        public string PersonType { get; set; }
        public string PersonName { get; set; }

        public string PersonContactNo { get; set; }
        public string PersonDesignation { get; set; }

        public string OfficeName { get; set; }
        public string OfficeAddress { get; set; }
        public string NearestLandMark { get; set; }



        public string GeneralComments { get; set; }
        public string OutComeVerification { get; set; }
        public string GpsLocation { get; set; }
        public string GpsURL { get; set; }


   
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }

        //Quality check fields
        public string QCComments { get; set; }
        public double?  Price { get; set; }
        public string VerifiedBy { get; set; }

        public virtual Inquiry Inquiry { get; set; }
        public virtual  OfficeAddressDetail OfficeAddressDetail { get; set; }
        public virtual BusinessWorkOfficeDetail BusinessWorkOfficeDetail { get; set; }
        public virtual MarketeCheck MarketeCheck { get; set; }
        public virtual OfficeHRDetail OfficeHRDetail { get; set; }
        [NotMapped]
        public virtual List<InquiryApplicationUser> InquiryApplicationUser
        {
            get
            {
                if (Inquiry != null)
                {
                    return Inquiry.InquiryApplicationUsers.Where(x => x.PersonType == this.PersonType && x.VerificationType == constant.VerificationType.WorkOffice).ToList();
                }

                return null;
            }
        }

        private Address _address;

        [NotMapped]
        public virtual Address Address
        {
            get
            {
                if (_address != null)
                {
                    return _address;
                }

                if (Inquiry != null  )
                {
                    _address = Inquiry.Addresses?.FirstOrDefault(x => x.PersonType == this.PersonType && x.VerificationType == constant.VerificationType.WorkOffice);
                    if (_address == null)
                    {
                        _address = new Address();
                        _address.FullAddress = this.OfficeAddress;
                        _address.PersonType = this.PersonType;
                        _address.VerificationType = constant.VerificationType.WorkOffice;
                        _address.InquiryId = InquiryId;
                    }
                    _address.FullAddress = OfficeAddress;
                    return _address;
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

                images = Inquiry.Images.Where(x => x.VerificationType == constant.VerificationType.WorkOffice 
                && x.PersonType == this.PersonType && x.VerificationId == WorkOfficeVerificationId).ToList();
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
                                  x.VerificationType == constant.VerificationType.WorkOffice &&
                                   x.VerificationId == WorkOfficeVerificationId).ToList();

                    return statuses;

                }
                return new List<DB.Status>();
            }
        }

    }


    public class OfficeAddressDetail
    {
        //Office Address Verification
        [Key, ForeignKey("WorkOfficeVerification")]
        public long OfficeAddressDetailId { get; set; }
        public bool WasActualAddressSame { get; set; }//Was actual address same as above mention.?
        public string CorrectAddress { get; set; }//Give correct addres if address not same ..
        public string EstablishedTime { get; set; }//Length of time the business / office has been established overaLL
        public bool WorkAtGivenAddress { get; set; }//does applicant work at the given address
        public string GiveNewAddress { get; set; }
        public bool IsApplicantAvailable { get; set; }//Did you meet the applicant
        public string MetPersonName { get; set; }
        public string GiveReason { get; set; } //if applicant not available give the reason y not u meet the applicant.
        public string CNICOS { get; set; }// Applicant's NIC # (O/s Physically if possible)
        public string CNICNo { get; set; }


        public string MetPersonCNIC { get; set; }



        public virtual WorkOfficeVerification WorkOfficeVerification { get; set; }

    }
    public class BusinessWorkOfficeDetail
    {
        [Key, ForeignKey("WorkOfficeVerification")]
        public long BusinessWorkOfficeDetailId { get; set; }

        //Business WOrk Office Verification For (SEB/SEP)
        public string TypeOfBusiness { get; set; }// Shop - Office - Restaurant Factory - other
        public string OtherTypeOfBusiness { get; set; }

        public string ApplicantIsA { get; set; }
        public string MentionOther { get; set; }
        public string MentionRent { get; set; }

        public string NatureOfBusiness { get; set; } // Manufactoring - Services - Trading - Govt Emp. - other
        public string OtherNatureOfBusiness { get; set; }

        public string BusinessLegalEntity { get; set; } // Proprietor - Partnership - Pvt. Ltd. - Public Lt. - Govt
        public string GovtEmpBusinessLegalEntity { get; set; }

        public bool NamePlateAffixed { get; set; } //Was Business Name plate affixed at business place same as in application.
        public string SizeApproxArea { get; set; }

        public string BusinessActivity { get; set; }// Low - Medium - High
        public int NoOfEmployees { get; set; } //Number of employees work in 

        public string BusinessEstablesSince { get; set; }

 
        public string LineOfBusiness { get; set; } 
        //public bool BankStatmentVerified { get; set; }
        public virtual WorkOfficeVerification WorkOfficeVerification { get; set; }
    }
    public class MarketeCheck
    {
        [Key, ForeignKey("WorkOfficeVerification")]
        public long MarketeCheckId { get; set; }
        //Neighbor /  Markete  Check
        public string NeighborsName { get; set; }
        public string NeighborsAddress { get; set; }
        public bool KnowsApplicant { get; set; } //Does Neighbor know the applicant
        public string KnowsHowLong { get; set; }// if neighbor knows the applicant then how long.
        public string NeighborComments { get; set; } // Comments Regarding the applicant
        public string BusinessEstablishedSinceMarketeCheck { get; set; }

        public string NeighborsTwoName { get; set; }
        public string NeighborsTwoAddress { get; set; }
        public bool NeighborsTwoKnowsApplicant { get; set; } //Does Neighbor know the applicant
        public string NeighborsTwoKnowsHowLong { get; set; }// if neighbor knows the applicant then how long.
        public string NeighborsTwoNeighborComments { get; set; } // Comments Regarding the applicant
        public string NeighborsTwoBusinessEstablishedSinceMarketeCheck { get; set; }
        public virtual WorkOfficeVerification WorkOfficeVerification { get; set; }
    }
    public class OfficeHRDetail
    {
        //Office /Hr Verifcation For (Salary Salaried Individual);
        [Key, ForeignKey("WorkOfficeVerification")]
        public long OfficeHRDetailId { get; set; }
        public string NameOfPersonToMeet { get; set; }
        public bool OHrKnowsApplicant { get; set; }
        public string ApplicantEmployementStatus { get; set; }
        public string ApplicantEmployementPeriod { get; set; }
        public string ApplicantDesignation { get; set; }
        public string OHrNatureOfBusiness { get; set; }
        public string OHrOtherNatureOfBusiness { get; set; }  // this field was missing

        public double GrossSalary { get; set; }
        public double NetTakeHomeSalary { get; set; }
        public bool SalarySlipVerified { get; set; }
      // public bool OHrBankStatmentVerified { get; set; }
        public virtual WorkOfficeVerification WorkOfficeVerification { get; set; }
    }

}