
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerificationSystem.Extensions;
using VerificationSystem.DB;

namespace VerificationSystem.Models
{
    public class InquiryCreateVM
    {
        System.Security.Principal.IPrincipal User = HttpContext.Current.User;
        private ApplicationDbContext db;
        public InquiryCreateVM()
        {
            db = new ApplicationDbContext();
        }

        public InquiryCreateVM(ApplicationDbContext db)
        {
            this.db = db;

        }

        //this constructor is used for existing inquiry in the record
        public InquiryCreateVM(ApplicationDbContext db, long InquiryId)
        {
            this.db = db;
            this.InquiryId = InquiryId;

            ////fetching attributes based on the existing inquiry id
            var currentInquiry = db.Inquiries.FirstOrDefault(x => x.InquiryId == InquiryId);
            this.AppName = currentInquiry.AppName;
            this.AppCNIC = currentInquiry.AppCNIC;
            this.AppContact = currentInquiry.AppContact;
        }
        public string UserId
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return HttpContext.Current.User.Identity.GetUserId();
                }
                else
                {
                    return null;
                }
            }
            set { UserId = value; }
        }
        public string message { get; set; }
        public bool _status { get; set; }



        [Required, Display(Name = "Company")]
        public long CompanyId { get; set; }
        [Required, Display(Name = "Branch")]
        public long BranchId { get; set; }
        [Required, Display(Name = "Product")]
        public long ProductId { get; set; }
        [Required, Display(Name = "Customer Branch")]
        public long CustomerBranchId { get; set; }

        [Required, Display(Name = "Applicant Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters.")]
        public string AppName { get; set; }

        [Required, Display(Name = "Applicant Contact")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only Numbers.")]
        [StringLength(23, MinimumLength = 11, ErrorMessage = "The Applicant Contact field length must be between 11 to 23")]
        public string AppContact { get; set; }


        [Required, Display(Name = "Applicant CNIC")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "The Applicant CNIC field must contains 13 digits")]
        public string AppCNIC { get; set; }

        public long InquiryId { get; set; }



        private SelectList companyList;
        public SelectList CompanyList
        {
            get
            {
          
                return
                 new SelectList(
                        db.Companies.Where(x => x.DisableDate == null).ToList(),
                         "CompanyId", "Name", this.CompanyId);

            }
            set { companyList = value; }
        }


        private SelectList branchList;
        public SelectList BranchList
        {
            get
            {
                return new SelectList(db.Branches.ToList(), "BranchId", "Name", this.BranchId);

            }
            set { branchList = value; }
        }

        private SelectList customerBranchList;
        public SelectList CustomerBranchList
        {
            get
            {
                if (customerBranchList != null) return customerBranchList;

                   return customerBranchList ?? new SelectList(db.CustomerBranches.Where(x => x.DisableDate == null && x.CompanyId == this.CompanyId).ToList(), "CustomerBranchId", "Name", this.CustomerBranchId);


            }
            set { customerBranchList = value; }
        }


        public SelectList productList;
        public SelectList ProductList
        {
            get
            {
                return productList ?? new SelectList(db.Products.Where(x => x.CompanyId == this.CompanyId).ToList(), "ProductId", "Name", this.ProductId);
            }
            set { productList = value; }
        }


        public Inquiry Save()
        {
            try
            {
                var inquiry = db.Inquiries.Create();
                inquiry.AppName = AppName;
                inquiry.AppCNIC = AppCNIC;
                inquiry.AppContact = AppContact;

                inquiry.BranchId = BranchId;
                inquiry.BranchName = BranchList.FirstOrDefault(x => x.Value == BranchId.ToString()).Text;

                inquiry.CompanyId = CompanyId;
                inquiry.CompanyName = CompanyList.FirstOrDefault(x => x.Value == CompanyId.ToString()).Text;

                inquiry.CustomerBranchId = CustomerBranchId;
                inquiry.CustomerBranchName = CustomerBranchList.FirstOrDefault(x => x.Value == CustomerBranchId.ToString()).Text;

                inquiry.ProductId = ProductId;

                inquiry.Status = constant.Status.New;
                inquiry.StatusDate = DateTime.Now;

                inquiry.RecordAt = DateTime.Now;
                inquiry.RecordBy = UserId;

                Status status = db.Statuses.Create();
                
                status.Descriptions = $"Inquiry Created.";
                status.StatusMessage = constant.Status.New;
                status.VerificationType = null;
                status.PersonType = null;
                //new field..
                status.VerificationId = 0;
                status.UserId = UserId;
                status.StatusDate = DateTime.Now;
                inquiry.Statuses.Add(status);


                db.Inquiries.Add(inquiry);
                db.SaveChanges();

                


                _status = true;
                message = "records has been saved successfully.";
                return inquiry;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                _status = false;
                return null;
            }
        }


        private ResidenceCreateVM residenceCreateVM;
        public ResidenceCreateVM ResidenceCreateVM
        {
            get
            {
                return residenceCreateVM ?? new ResidenceCreateVM(db, InquiryId);
            }
            set { residenceCreateVM = value; }
        }

        private WorkOfficeVM workOfficeVM;
        public WorkOfficeVM WorkOfficeVM
        {
            get
            {
                return workOfficeVM ?? new WorkOfficeVM(db, InquiryId);
            }
            set { workOfficeVM = value; }
        }


        private SalarySlipVM salarySlipVM;
        public SalarySlipVM SalarySlipVM
        {
            get
            {
                return salarySlipVM ?? new SalarySlipVM(db, InquiryId);
            }
            set { salarySlipVM = value; }
        }


        private BankStatementCreateVM bankStatementVM;
        public BankStatementCreateVM BankStatementCreateVM
        {
            get
            {
                return bankStatementVM ?? new BankStatementCreateVM(db, InquiryId);
            }
            set { bankStatementVM = value; }
        }



        private TenantCreateVM tenantCreateVM;
        public TenantCreateVM TenantCreateVM
        {
            get
            {
                return tenantCreateVM ?? new TenantCreateVM(db, InquiryId);
            }
            set { tenantCreateVM = value; }
        }




    }

    public class ResidenceCreateVM
    {

        private ApplicationDbContext db;
        public ResidenceCreateVM()
        {
            db = new ApplicationDbContext();
        }
        public ResidenceCreateVM(ApplicationDbContext db, long inquiryId)
        {
            this.db = db;
            InquiryId = inquiryId;
        }


        public long? ResidenceVerificationId { get; set; }

        [Required]
        public long InquiryId { get; set; }

        [Required, Display(Name = "Person Type")]
        public string PersonType { get; set; }
        public string OldPersonType { get; set; }

        [Required, Display(Name = "Person Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters.")]
        public string PersonName { get; set; }

        [Required, Display(Name = "Person CNIC")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "CNIC field must contain 13 digits")]
        public string PersonCNIC { get; set; }

        [Required, Display(Name = "Contact No.")]
        [StringLength(23, MinimumLength = 11, ErrorMessage = "The Applicant Contact field length must be between 11 to 23")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only Numbers.")]
        public string PersonContactNo { get; set; }
        [Required, Display(Name = "Residence Address")]
        public string ResidenceAddress { get; set; }
        [Required, Display(Name = "Nearest Landmark")]
        public string NearestLandMark { get; set; }



        public SelectList personTypeList;
        public SelectList PersonTypeList
        {
            get
            {
                return personTypeList ?? new SelectList(constant.PersonType.PersonTypes, PersonType as object);
            }
            set { personTypeList = value; }
        }



        private List<ResidenceVerification> residenceVerifications;
        public List<ResidenceVerification> ResidenceVerifications
        {
            get { return residenceVerifications ?? db.ResidenceVerifications.Where(x => x.InquiryId == InquiryId).ToList(); }
            set { residenceVerifications = value; }
        }

    }

    public class WorkOfficeVM
    {

        private ApplicationDbContext db;
        public WorkOfficeVM()
        {
            db = new ApplicationDbContext();
        }
        public WorkOfficeVM(ApplicationDbContext db, long inquiryId)
        {
            this.db = db;
            InquiryId = inquiryId;
        }


        public long? WorkOfficeVerificationId { get; set; }

        [Required]
        public long InquiryId { get; set; }

        [Required, Display(Name = "Person Type")]
        public string PersonType { get; set; }
        public string OldPersonType { get; set; }

        [Required, Display(Name = "Person Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters.")]
        public string PersonName { get; set; }

        [Required, Display(Name = "Designation")]
        public string PersonDesignation { get; set; }

        [Required, Display(Name = "Office Name")]
        public string OfficeName { get; set; }


        [Required, Display(Name = "Contact No.")]
        [StringLength(23, MinimumLength = 11, ErrorMessage = "The Applicant Contact field length must be between 11 to 23")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only Numbers.")]
        public string PersonContactNo { get; set; }

        [Required, Display(Name = "Office Address")]
        public string OfficeAddress { get; set; }


        [Required, Display(Name = "Nearest Landmark")]
        public string NearestLandMark { get; set; }



        public SelectList personTypeList;
        public SelectList PersonTypeList
        {
            get
            {
                return personTypeList ?? new SelectList(constant.PersonType.PersonTypes, PersonType as object);
            }
            set { personTypeList = value; }
        }



        private List<WorkOfficeVerification> workOfficeVerifications;
        public List<WorkOfficeVerification> WorkOfficeVerifications
        {
            get { return workOfficeVerifications ?? db.WorkOfficeVerifications.Where(x => x.InquiryId == InquiryId).ToList(); }
            set { workOfficeVerifications = value; }
        }

    }
    public class SalarySlipVM
    {

        private ApplicationDbContext db;
        public SalarySlipVM()
        {
            db = new ApplicationDbContext();
        }
        public SalarySlipVM(ApplicationDbContext db, long inquiryId)
        {
            this.db = db;
            InquiryId = inquiryId;
        }


        public long? SalarySlipVerificationId { get; set; }

        [Required]
        public long InquiryId { get; set; }


        [Required, Display(Name = "Person Type")]
        public string PersonType { get; set; }
        public string OldPersonType { get; set; }

        [Required, Display(Name = "Person Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters.")]
        public string PersonName { get; set; }

        [Required, Display(Name = "Contact No.")]
        [StringLength(23, MinimumLength = 11, ErrorMessage = "The Applicant Contact field length must be between 11 to 23")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only Numbers.")]
        public string PersonContactNo { get; set; }



        [Required, Display(Name = "Office Name")]
        public string OfficeName { get; set; }

        [Required, Display(Name = "Office Address")]
        public string OfficeAddress { get; set; }

        [Required, Display(Name = "Nearest Landmark")]
        public string NearestLandMark { get; set; }

        // [Display(Name = "Select PaySlip")]
        //public HttpPostedFileBase PaySlip { get; set; }

        public SelectList personTypeList;
        public SelectList PersonTypeList
        {
            get
            {
                return personTypeList ?? new SelectList(constant.PersonType.PersonTypes, PersonType as object);
            }
            set { personTypeList = value; }
        }



        private List<SalarySlipVerification> salarySlipVerification;
        public List<SalarySlipVerification> SalarySlipVerification
        {
            get { return salarySlipVerification ?? db.SalarySlipVerifications.Where(x => x.InquiryId == InquiryId).ToList(); }
            set { salarySlipVerification = value; }
        }

    }
    public class BankStatementCreateVM
    {

        private ApplicationDbContext db;
        public BankStatementCreateVM()
        {
            db = new ApplicationDbContext();
        }
        public BankStatementCreateVM(ApplicationDbContext db, long inquiryId)
        {
            this.db = db;
            InquiryId = inquiryId;
        }


        public long? BankStatementVerificationId { get; set; }

        [Required]
        public long InquiryId { get; set; }

        [Required, Display(Name = "Person Type")]
        public string PersonType { get; set; }
        public string OldPersonType { get; set; }


        [Required, Display(Name = "Person Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters.")]
        public string PersonName { get; set; }

        [Required, Display(Name = "Contact No.")]
        [StringLength(23, MinimumLength = 11, ErrorMessage = "The Applicant Contact field length must be between 11 to 23")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only Numbers.")]
        public string PersonContactNo { get; set; }

        [Required, Display(Name = "Bank Name")]
        public string BankName { get; set; }


        [Required, Display(Name = "Bank Address")]
        public string BankAddress { get; set; }
        [Required, Display(Name = "Nearest Landmark")]
        public string NearestLandMark { get; set; }



        public SelectList personTypeList;
        public SelectList PersonTypeList
        {
            get
            {
                return personTypeList ?? new SelectList(constant.PersonType.PersonTypes, PersonType as object);
            }
            set { personTypeList = value; }
        }



        private List<BankStatementVerification> bankVerification;
        public List<BankStatementVerification> BankVerifications
        {
            get { return bankVerification ?? db.BankStatementVerifications.Where(x => x.InquiryId == InquiryId).ToList(); }
            set { bankVerification = value; }
        }

    }


    public class TenantCreateVM
    {

        private ApplicationDbContext db;
        public TenantCreateVM()
        {
            db = new ApplicationDbContext();
        }
        public TenantCreateVM(ApplicationDbContext db, long inquiryId)
        {
            this.db = db;
            InquiryId = inquiryId;
        }


        public long? TenantVerificationId { get; set; }

        [Required]
        public long InquiryId { get; set; }

        [Required, Display(Name = "Person Type")]
        public string PersonType { get; set; }
        public string OldPersonType { get; set; }

        [Required, Display(Name = "Person Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only letters.")]
        public string PersonName { get; set; }

        [Required, Display(Name = "Contact No.")]
        [StringLength(23, MinimumLength = 11, ErrorMessage = "The Applicant Contact field length must be between 11 to 23")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only Numbers.")]
        public string PersonContactNo { get; set; }

        [Required, Display(Name = "Tenant Address")]
        public string TenantAddress { get; set; }

        [Required, Display(Name = "Nearest Landmark")]
        public string NearestLandMark { get; set; }



        public SelectList personTypeList;
        public SelectList PersonTypeList
        {
            get
            {
                return personTypeList ?? new SelectList(constant.PersonType.PersonTypes, PersonType as object);
            }
            set { personTypeList = value; }
        }



        private List<TenantVerification> tenantVerification;
        public List<TenantVerification> TenantVerifications
        {
            get { return tenantVerification ?? db.TenantVerifications.Where(x => x.InquiryId == InquiryId).ToList(); }
            set { tenantVerification = value; }
        }

    }




     

    public class InquiryIndexVM
    {
        System.Security.Principal.IPrincipal User =HttpContext.Current .User;
        private ApplicationDbContext db;


        #region FilterProperties

        public string[] Statuses { get; set; }


        private MultiSelectList statusList;
        public MultiSelectList StatusList
        {
            get
            {
                return statusList ?? new MultiSelectList(constant.Status.Statuses, Statuses);
            }
            set { statusList = value; }
        }


        public string[] Companies { get; set; }

        private MultiSelectList companyList;
        public MultiSelectList CompanyList
        {
            get
            {
                if (User.IsInRole(constant.Roles.SuperAdmin) || User.IsInRole(constant.Roles.Head))
                {
                    return companyList ?? new MultiSelectList(db.Companies.Select(x=> x.Name.Trim()).ToList(), Companies);
                }
                else //User.IsInRole(constant.Roles.CHead) || User.IsInRole(constant.Roles.CBranch)
                {
                    var companyId = long.Parse(ControllerHelper.GetUserClaim(constant.Claims.CompanyId));
                    return companyList ?? new MultiSelectList(db.Companies.Where(x => x.CompanyId == companyId).Select(x=> x.Name.Trim() ).ToList(), Companies);
                }
            }
            set { companyList = value; }
        }

        public long[] Branches { get; set; }

        private MultiSelectList branchList;
        public MultiSelectList BranchList
        {
            get
            {
                if (User.IsInRole(constant.Roles.SuperAdmin) || User.IsInRole(constant.Roles.Head))
                {
                    return branchList ?? new MultiSelectList(db.CustomerBranches.ToList(), "CustomerBranchId", "Name", "Company.Name", Branches);
                }
                else if (User.IsInRole(constant.Roles.Head))
                {
                    var companyId = long.Parse(ControllerHelper.GetUserClaim(constant.Claims.CompanyId));
                    return branchList ?? new MultiSelectList(db.CustomerBranches.Where(x => x.CompanyId == companyId).ToList(), "CustomerBranchId", "Name", "Company.Name", Branches);
                }
                else if ( User.IsInRole(constant.Roles.Head))
                {
                    var branchId = long.Parse(ControllerHelper.GetUserClaim(constant.Claims.BranchId));
                    var customerBranchIds = db.Users.Where(x => x.BranchId == branchId && x.CustomerBranchId > 0).Select(x => x.CustomerBranchId).ToList();
                    return branchList ?? new MultiSelectList(db.CustomerBranches.Where(x => customerBranchIds.Any(f => f.Value == x.CustomerBranchId)
                    ).ToList(), "CustomerBranchId", "Name", "Company.Name", Branches);
                }
                else
                {
                    var customerBranchId = long.Parse(ControllerHelper.GetUserClaim(constant.Claims.CustomerBranchId));
                    return branchList ?? new MultiSelectList(db.CustomerBranches.Where(x => x.CustomerBranchId == customerBranchId ).ToList(), "CustomerBranchId", "Name", "Company.Name", Branches);
                }
            }
            set { branchList = value; }
        }



        public long[] Branch { get; set; }

        private MultiSelectList BranchLists;
        public MultiSelectList CBranchList
        {
            get
            {
                if (User.IsInRole(constant.Roles.SuperAdmin) || User.IsInRole(constant.Roles.Head))
                {
                    return BranchLists ?? new MultiSelectList(db.Branches.Select(x => new { x.Name, x.BranchId }).ToList(), "BranchId", "Name", Branches);
                }
                else 
                {
                    var branchId = long.Parse(ControllerHelper.GetUserClaim(constant.Claims.BranchId));
                    return BranchLists ?? new MultiSelectList(db.Branches.Select(x => new { x.Name, x.BranchId }).ToList(), "BranchId", "Name", Branch);
                }
                
            }
            set { BranchList = value; }
        }

        public long[] Products { get; set; }

        private MultiSelectList productList;
        public MultiSelectList ProductList
        {
            get
            {
                if (User.IsInRole(constant.Roles.SuperAdmin) || User.IsInRole(constant.Roles.Head) )
                {
                    return productList ?? new MultiSelectList(db.Products.Select(x => new { Name = x.Name, ProductId  =x.ProductId , CompanyName =  x.Company.Name}).ToList(), "ProductId", "Name", "CompanyName",  Products);
                }
                else if (User.IsInRole(constant.Roles.Head)  ) 
                {
                    var companyId = long.Parse(ControllerHelper.GetUserClaim(constant.Claims.CompanyId));

                    return productList ?? new MultiSelectList(db.Products.Where( x=> x.CompanyId == companyId) .Select(x => new { Name = x.Name, ProductId = x.ProductId, CompanyName = x.Company.Name }).ToList(), "ProductId", "Name", "CompanyName", Products);
                }
                else 
                {
                    var branchId = long.Parse(ControllerHelper.GetUserClaim(constant.Claims.BranchId));

                    var companyIds =  db.Users.Where(x => x.BranchId == branchId && x.CustomerBranchId > 0).Select(x => x.CompanyId).ToList();

                    return productList ?? new MultiSelectList(db.Products.Where(x => 
                    companyIds.Any(f => f.Value == x.CompanyId)
                    ).Select(x => new { Name = x.Name, ProductId = x.ProductId, CompanyName = x.Company.Name }).ToList(), "ProductId", "Name", "CompanyName", Products);
                }
                //jab customer ko or branch user ko filter karnay ka haq hi nahi hai tou select list q banao?
            }
            set { productList = value; }
        }

        
        public string InquiryIds { get; set; }

        public string AppName { get; set; }
        public string AppCnic { get; set; }

        #endregion

         

        public InquiryIndexVM()
        {
            db = new ApplicationDbContext();
        }

        public InquiryIndexVM(ApplicationDbContext db, int? page, string st,string  cm, string ib, string cb, string p, string an, string id,string ac)
        {
            this.db = db;

            Inquiries = new List<Inquiry>();

            if (User.IsInRole(constant.Roles.Head))
            {
                
                long branchId = long.Parse(ControllerHelper.GetUserClaim(constant.Claims.BranchId));
                Inquiries.AddRange(db.Inquiries.Where(x => x.BranchId == branchId).ToList());
                Inquiries = Inquiries.Distinct().ToList();
            }
           
            

            
            if (User.IsInRole(constant.Roles.Head) || User.IsInRole(constant.Roles.SuperAdmin))
            {
                Inquiries = db.Inquiries.ToList();
            }

  
            if (st != null && st.Count() > 0)
            {
                string[] statusArray = st.Split(',').TakeWhile(x => x != "").ToArray();
                Statuses = statusArray;
                Inquiries = Inquiries.Where(x => statusArray.Any(f => f == x.Status)).ToList();
            }

            if (cm != null && cm.Count() > 0)
            {
                string[] companyArray = cm.Split(',').TakeWhile(x => x != "").ToArray();
                Companies = companyArray;
                Inquiries = Inquiries.Where(x => companyArray.Any(f => f.Trim() == x.CompanyName.Trim())).ToList();
            }

            if (cb != null && cb.Count() > 0)
            {
                long l = 0;
                long[] branchesArray = cb.Split(',').TakeWhile(x => x != "" && long.TryParse(x, out l) ).Select(long.Parse).ToArray();
                
                Branches = branchesArray;
                Inquiries = Inquiries.Where(x => branchesArray.Any(f => f == x.CustomerBranchId)).ToList();
            }

            if (ib != null && ib.Count() > 0)
            {
                long l = 0;
                long[] BranchArray = ib.Split(',').TakeWhile(x => x != "" && long.TryParse(x, out l)).Select(long.Parse).ToArray();

                Branches = BranchArray;
                Inquiries = Inquiries.Where(x => BranchArray.Any(f => f == x.BranchId)).ToList();
            }


            if (p != null && p.Count() > 0)
            {
                long l = 0;
                long[] productsArray = p.Split(',').TakeWhile(x => x != "" && long.TryParse(x, out l)).Select(long.Parse).ToArray();

                Products = productsArray;
                Inquiries = Inquiries.Where(x => productsArray.Any(f => f == x.ProductId)).ToList();
            }



            if (id != null && id.Count() > 0)
            {
                long l = 0;
                long[] inquiryArray = id.Split(',').TakeWhile(x => x != "" && long.TryParse(x, out l)).Select(long.Parse).ToArray();

                InquiryIds = id;
                Inquiries = Inquiries.Where(x => inquiryArray.Any(f => f == x.InquiryId)).ToList();
            }

            if (an != null && an.Count() > 0)
            {
                AppName = an;
                Inquiries = Inquiries.Where(x => x.AppName.ToLower().Contains(an.ToLower())).ToList();
            }

            if (ac != null && ac.Count() > 0)
            {
                AppCnic = ac;
                Inquiries = Inquiries.Where(x => x.AppCNIC.ToLower().Contains(ac)).ToList();
            }






            Pager = new Pager(Inquiries.Count(), page, 10, 6);
            Inquiries = Inquiries.OrderByDescending(x => x.StatusDate).Skip((Pager.CurrentPage - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
            

            FIOUsers = UserHelper.GetAllUsers(constant.Roles.Fio);



        }



        public List<ApplicationUser> FIOUsers { get; set; }


        public List<Inquiry> Inquiries
        {
            get;
            set;
        }

        public Pager Pager { get; set; }


    }

    public class InquiryVerificationVM
    {
        public Inquiry Inquiry { get; set; }
        public List<ApplicationUser> FIOUsers
        {
            get;
            set;
        }

    }















}