using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace VerificationSystem.DB
{
    public class Inquiry
    {

        public Inquiry()
        { 
            InquiryApplicationUsers = new HashSet<InquiryApplicationUser>();
            Statuses = new HashSet<Status>();

            ResidenceVerifications = new HashSet<ResidenceVerification>();
            WorkOfficeVerifications = new HashSet<WorkOfficeVerification>();

            SalarySlipVerifications = new HashSet<SalarySlipVerification>();
            BankStatementVerifications = new HashSet<BankStatementVerification>();
            TenantVerifications = new HashSet<TenantVerification>();

            Images = new HashSet<InquiryImage>();

            Addresses = new HashSet<Address>(); 
        }


        #region Properties

        public long InquiryId { get; set; }
        public string CustomerCode { get; set; }

        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        
        public long BranchId { get; set; }
        public string BranchName { get; set; }

        public long CustomerBranchId { get; set; }
        public string CustomerBranchName { get; set; }
        
        public long ProductId { get; set; }
        public string AppNo { get; set; }
        

        //applicant detail 

        public string AppName { get; set; }
        public string AppContact { get; set; }
        public string AppCNIC { get; set; }

        #endregion


        public double? Price { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusDescription { get; set; }

        public DateTime RecordAt { get; set; }
        public string RecordBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string UpdateBy { get; set; }


        public string FileName { get; set; }
        public string BatchNo { get; set; }
        public bool IsExported { get; set; }


        [NotMapped]
        public int HoldDaysCount { get; set; }



        #region Relative Objects
        // have to create all relative objecs 




        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
        [ForeignKey("CustomerBranchId")]
        public virtual CustomerBranch CustomerBranch { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<InquiryApplicationUser> InquiryApplicationUsers { get; set; }

        public virtual ICollection<ResidenceVerification> ResidenceVerifications { get; set; }
        public virtual ICollection<WorkOfficeVerification> WorkOfficeVerifications { get; set; }
        public virtual ICollection<TenantVerification> TenantVerifications { get; set; }


        public virtual ICollection<SalarySlipVerification> SalarySlipVerifications { get; set; }
        public virtual ICollection<BankStatementVerification> BankStatementVerifications { get; set; }
       

        //Inquiries for matching address...........
        [NotMapped]
        public virtual ICollection<Inquiry> AddressInquiries { get; set; }




        public virtual ICollection<Status> Statuses { get; set; }
        public virtual ICollection<InquiryImage> Images { get; set; }
        //public virtual ICollection<BQCInquiryImage> BQCImages { get; set; }


        [NotMapped]
        private List<PersonVerifications> personsVerification;
        [NotMapped]
        public List<PersonVerifications> PersonsVerifications
        {
            get
            {
                if (personsVerification != null) return personsVerification;
                //creating list 
                personsVerification = new List<PersonVerifications>();

                //applicant 
                PersonVerifications applicantVerification = new PersonVerifications();
                applicantVerification.ResidenceVerifications = this.ResidenceVerifications.Where(x => x.PersonType == constant.PersonType.Applicant).ToList();
                applicantVerification.WorkOfficeVerifications = this.WorkOfficeVerifications.Where(x => x.PersonType == constant.PersonType.Applicant).ToList();
                applicantVerification.SalarySlipVerifications = this.SalarySlipVerifications.Where(x => x.PersonType == constant.PersonType.Applicant).ToList();
                applicantVerification.TenantVerifications = this.TenantVerifications.Where(x => x.PersonType == constant.PersonType.Applicant).ToList();
                applicantVerification.BankStatementVerifications = this.BankStatementVerifications.Where(x => x.PersonType == constant.PersonType.Applicant).ToList();

                applicantVerification.PersonType = constant.PersonType.Applicant;
                //adding in list 
                if(applicantVerification.HasVerification)
                personsVerification.Add(applicantVerification);

                PersonVerifications spouseOneVerfication = new PersonVerifications();
                spouseOneVerfication.ResidenceVerifications = this.ResidenceVerifications.Where(x => x.PersonType == constant.PersonType.Spouse1).ToList();
                spouseOneVerfication.WorkOfficeVerifications = this.WorkOfficeVerifications.Where(x => x.PersonType == constant.PersonType.Spouse1).ToList();
                spouseOneVerfication.SalarySlipVerifications = this.SalarySlipVerifications.Where(x => x.PersonType == constant.PersonType.Spouse1).ToList();
                spouseOneVerfication.TenantVerifications = this.TenantVerifications.Where(x => x.PersonType == constant.PersonType.Spouse1).ToList();
                spouseOneVerfication.BankStatementVerifications = this.BankStatementVerifications.Where(x => x.PersonType == constant.PersonType.Spouse1).ToList();
                spouseOneVerfication.PersonType = constant.PersonType.Spouse1;
                //adding in list
                if (spouseOneVerfication.HasVerification)
                    personsVerification.Add(spouseOneVerfication);

                PersonVerifications coApplicantVerfication = new PersonVerifications();
                coApplicantVerfication.ResidenceVerifications = this.ResidenceVerifications.Where(x => x.PersonType == constant.PersonType.CoApplicant).ToList();
                coApplicantVerfication.WorkOfficeVerifications = this.WorkOfficeVerifications.Where(x => x.PersonType == constant.PersonType.CoApplicant).ToList();
                coApplicantVerfication.SalarySlipVerifications = this.SalarySlipVerifications.Where(x => x.PersonType == constant.PersonType.CoApplicant).ToList();
                coApplicantVerfication.TenantVerifications = this.TenantVerifications.Where(x => x.PersonType == constant.PersonType.CoApplicant).ToList();
                coApplicantVerfication.BankStatementVerifications = this.BankStatementVerifications.Where(x => x.PersonType == constant.PersonType.CoApplicant).ToList();
                coApplicantVerfication.PersonType = constant.PersonType.CoApplicant;
                //adding in list
                if (coApplicantVerfication.HasVerification)
                    personsVerification.Add(coApplicantVerfication);


                PersonVerifications guarantorOneVerfication = new PersonVerifications();
                guarantorOneVerfication.ResidenceVerifications = this.ResidenceVerifications.Where(x => x.PersonType == constant.PersonType.Guarantor1).ToList();
                guarantorOneVerfication.WorkOfficeVerifications = this.WorkOfficeVerifications.Where(x => x.PersonType == constant.PersonType.Guarantor1).ToList();
                guarantorOneVerfication.SalarySlipVerifications = this.SalarySlipVerifications.Where(x => x.PersonType == constant.PersonType.Guarantor1).ToList();
                guarantorOneVerfication.TenantVerifications = this.TenantVerifications.Where(x => x.PersonType == constant.PersonType.Guarantor1).ToList();
                guarantorOneVerfication.BankStatementVerifications = this.BankStatementVerifications.Where(x => x.PersonType == constant.PersonType.Guarantor1).ToList();
                guarantorOneVerfication.PersonType = constant.PersonType.Guarantor1;
                //adding in list
                if (guarantorOneVerfication.HasVerification)
                    personsVerification.Add(guarantorOneVerfication);

                PersonVerifications guarantorTwoVerfication = new PersonVerifications();
                guarantorTwoVerfication.ResidenceVerifications = this.ResidenceVerifications.Where(x => x.PersonType == constant.PersonType.Guarantor2).ToList();
                guarantorTwoVerfication.WorkOfficeVerifications = this.WorkOfficeVerifications.Where(x => x.PersonType == constant.PersonType.Guarantor2).ToList();
                guarantorTwoVerfication.SalarySlipVerifications = this.SalarySlipVerifications.Where(x => x.PersonType == constant.PersonType.Guarantor2).ToList();
                guarantorTwoVerfication.TenantVerifications = this.TenantVerifications.Where(x => x.PersonType == constant.PersonType.Guarantor2).ToList();
                guarantorTwoVerfication.BankStatementVerifications = this.BankStatementVerifications.Where(x => x.PersonType == constant.PersonType.Guarantor2).ToList();
                guarantorTwoVerfication.PersonType = constant.PersonType.Guarantor2;
                //adding in list               
                if (guarantorTwoVerfication.HasVerification)
                personsVerification.Add(guarantorTwoVerfication);


                PersonVerifications referenceOneVerfication = new PersonVerifications();
                referenceOneVerfication.ResidenceVerifications = this.ResidenceVerifications.Where(x => x.PersonType == constant.PersonType.Reference1).ToList();
                referenceOneVerfication.WorkOfficeVerifications = this.WorkOfficeVerifications.Where(x => x.PersonType == constant.PersonType.Reference1).ToList();
                referenceOneVerfication.SalarySlipVerifications = this.SalarySlipVerifications.Where(x => x.PersonType == constant.PersonType.Reference1).ToList();
                referenceOneVerfication.TenantVerifications = this.TenantVerifications.Where(x => x.PersonType == constant.PersonType.Reference1).ToList();
                referenceOneVerfication.BankStatementVerifications = this.BankStatementVerifications.Where(x => x.PersonType == constant.PersonType.Reference1).ToList();
                referenceOneVerfication.PersonType = constant.PersonType.Reference1;
                //adding in list
                if (referenceOneVerfication.HasVerification)
                    personsVerification.Add(referenceOneVerfication);


                PersonVerifications referenceTwoVerfication = new PersonVerifications();
                referenceTwoVerfication.ResidenceVerifications = this.ResidenceVerifications.Where(x => x.PersonType == constant.PersonType.Reference2).ToList();
                referenceTwoVerfication.WorkOfficeVerifications = this.WorkOfficeVerifications.Where(x => x.PersonType == constant.PersonType.Reference2).ToList();
                referenceTwoVerfication.SalarySlipVerifications = this.SalarySlipVerifications.Where(x => x.PersonType == constant.PersonType.Reference2).ToList();
                referenceTwoVerfication.TenantVerifications = this.TenantVerifications.Where(x => x.PersonType == constant.PersonType.Reference2).ToList();
                referenceTwoVerfication.BankStatementVerifications = this.BankStatementVerifications.Where(x => x.PersonType == constant.PersonType.Reference2).ToList();
                referenceTwoVerfication.PersonType = constant.PersonType.Reference2;
                //adding in list
                if (referenceTwoVerfication.HasVerification)
                    personsVerification.Add(referenceTwoVerfication);

                PersonVerifications referenceThreeVerfication = new PersonVerifications();
                referenceThreeVerfication.ResidenceVerifications = this.ResidenceVerifications.Where(x => x.PersonType == constant.PersonType.Reference3).ToList();
                referenceThreeVerfication.WorkOfficeVerifications = this.WorkOfficeVerifications.Where(x => x.PersonType == constant.PersonType.Reference3).ToList();
                referenceThreeVerfication.SalarySlipVerifications = this.SalarySlipVerifications.Where(x => x.PersonType == constant.PersonType.Reference3).ToList();
                referenceThreeVerfication.TenantVerifications = this.TenantVerifications.Where(x => x.PersonType == constant.PersonType.Reference3).ToList();
                referenceThreeVerfication.BankStatementVerifications = this.BankStatementVerifications.Where(x => x.PersonType == constant.PersonType.Reference3).ToList();
                referenceThreeVerfication.PersonType = constant.PersonType.Reference3;

                ////adding in list
                if (referenceThreeVerfication.HasVerification)
                    personsVerification.Add(referenceThreeVerfication);
                return personsVerification;
            }
            set { personsVerification = value; }
        }

        #endregion

    }




    [NotMapped]
    public class PersonVerifications
    {
        public PersonVerifications()
        {
            ResidenceVerifications = new List<ResidenceVerification>();
            WorkOfficeVerifications = new List<WorkOfficeVerification>();
            TenantVerifications = new List<TenantVerification>();
            SalarySlipVerifications = new List<SalarySlipVerification>();
            BankStatementVerifications = new List<BankStatementVerification>();
        }
        public string PersonType { get; set; }
        public List<ResidenceVerification> ResidenceVerifications { get; set; }
        public List<WorkOfficeVerification> WorkOfficeVerifications { get; set; }
        public List<TenantVerification> TenantVerifications { get; set; }
        public List<SalarySlipVerification> SalarySlipVerifications { get; set; }
        public List<BankStatementVerification> BankStatementVerifications { get; set; }

     
        public bool HasVerification
        {
            get
            {
                if (ResidenceVerifications.Count > 0 || WorkOfficeVerifications.Count > 0 ||
                    TenantVerifications.Count > 0 || SalarySlipVerifications.Count > 0 
                    || BankStatementVerifications.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}