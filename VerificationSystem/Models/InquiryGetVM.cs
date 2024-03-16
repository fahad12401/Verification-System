using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerificationSystem.DB;

namespace VerificationSystem.Models
{
    public class InquiryGetVM
    {
        
        public InquiryGetVM() { }
        public InquiryGetVM( Inquiry inquiry , string userId)
        {
            //assign inquiry properties
            InquiryId = inquiry.InquiryId;

            CompanyId = inquiry.CompanyId;
            CompanyName = inquiry.CompanyName;

            BranchId = inquiry.BranchId;
            BranchName = inquiry.BranchName;

            CustomerBranchId = inquiry.CustomerBranchId;
            CustomerBranchName = inquiry.CustomerBranchName;

            ProductId = inquiry.ProductId;
            ProductName = inquiry.Product.Name;

            AppName = inquiry.AppName;
            AppContact = inquiry.AppContact;
            AppCNIC = inquiry.AppCNIC;


            var residenceVerifiactionIds = inquiry.InquiryApplicationUsers
                .Where(x => x.UserId == userId && x.VerificationType == constant.VerificationType.Residence)
                .Select(c => c.VerificationId).ToList();

            //find Residence verification for the current 

            ResidenceVerifications = inquiry.ResidenceVerifications.Where(x => x.InquiryApplicationUser.Any(s => s.UserId == userId && residenceVerifiactionIds.Contains(x.ResidenceVerificationId)) && x.Status == constant.Status.InProgress)
               .Select(x => new ResidenceVerification() { InquiryId = x.InquiryId, NearestLandMark = x.NearestLandMark, PersonCNIC = x.PersonCNIC, PersonContactNo = x.PersonContactNo, PersonName = x.PersonName, PersonType = x.PersonType, ResidenceVerificationId = x.ResidenceVerificationId, ResidenceAddress = x.ResidenceAddress, Status = x.Status, StatusDate = x.StatusDate }).ToList();

             
            //find WOrkOffice verification for the current user.....

            var workOfficeVerifiactionIds = inquiry.InquiryApplicationUsers
              .Where(x => x.UserId == userId && x.VerificationType == constant.VerificationType.WorkOffice)
              .Select(c => c.VerificationId).ToList();

            WorkOfficeVerifications = inquiry.WorkOfficeVerifications.Where(x => x.InquiryApplicationUser.Any(s => s.UserId == userId && workOfficeVerifiactionIds.Contains(x.WorkOfficeVerificationId)) && x.Status == constant.Status.InProgress)
              .Select(f => new WorkOfficeVerification() { InquiryId = f.InquiryId, WorkOfficeVerificationId = f.WorkOfficeVerificationId, PersonType = f.PersonType, PersonName = f.PersonName, PersonContactNo = f.PersonContactNo, PersonDesignation = f.PersonDesignation, OfficeName = f.OfficeName, OfficeAddress = f.OfficeAddress, NearestLandMark = f.NearestLandMark }).ToList();
   

            //find BankStatement 

            var bsVerifiactionIds = inquiry.InquiryApplicationUsers
            .Where(x => x.UserId == userId && x.VerificationType == constant.VerificationType.BankStatement )
            .Select(c => c.VerificationId).ToList();

            BankStatementVerifications = inquiry.BankStatementVerifications.Where(x => x.InquiryApplicationUser.Any(s => s.UserId == userId && bsVerifiactionIds.Contains(x.BankStatementVerificationId)) && x.Status == constant.Status.InProgress).
              Select(x => new BankStatementVerification() { InquiryId = x.InquiryId, BankStatementVerificationId = x.BankStatementVerificationId, PersonType = x.PersonType, PersonName = x.PersonName, PersonContactNo = x.PersonContactNo, BankName = x.BankName, BankAddress = x.BankAddress, NearestLandMark = x.NearestLandMark }).ToList();
          
            //find SalarySlip

            var ssVerifiactionIds = inquiry.InquiryApplicationUsers
            .Where(x => x.UserId == userId && x.VerificationType == constant.VerificationType.SalarySlip)
            .Select(c => c.VerificationId).ToList();


            SalarySlipVerifications = inquiry.SalarySlipVerifications.Where(x => x.InquiryApplicationUser.Any(s => s.UserId == userId && ssVerifiactionIds.Contains(x.SalarySlipVerificationId)) && x.Status == constant.Status.InProgress).
             Select(x => new SalarySlipVerification() { InquiryId = x.InquiryId, SalarySlipVerificationId = x.SalarySlipVerificationId, PersonType = x.PersonType, PersonName = x.PersonName, PersonContactNo = x.PersonContactNo, OfficeName = x.OfficeName, OfficeAddress = x.OfficeAddress, NearestLandMark = x.NearestLandMark }).ToList();
          

            //find tenant

            var tVerifiactionIds = inquiry.InquiryApplicationUsers
            .Where(x => x.UserId == userId && x.VerificationType == constant.VerificationType.Tenant)
            .Select(c => c.VerificationId).ToList();


            TenantVerifications = inquiry.TenantVerifications.Where(x => x.InquiryApplicationUser.Any(s => s.UserId == userId && tVerifiactionIds.Contains(x.TenantVerificationId))
       && x.Status == constant.Status.InProgress).
         Select(x => new TenantVerification() { InquiryId = x.InquiryId, TenantVerificationId = x.TenantVerificationId, PersonType = x.PersonType, PersonName = x.PersonName, PersonContactNo = x.PersonContactNo, TenantAddress = x.TenantAddress, NearestLandMark = x.NearestLandMark }).ToList();

          
        }
        public List<InquiryGetVM> GetAll(List<Inquiry> inquiries, string userId, ApplicationDbContext db)
        {

            List<InquiryGetVM> modelList = new List<InquiryGetVM>();
            foreach (var inquiry in inquiries)
            {
                var inquiryGetVM = new InquiryGetVM(inquiry, userId);
                //do other things like save status and others...
                //save status for each verification.
                if (inquiryGetVM.ResidenceVerifications.Count <= 0 && inquiryGetVM.WorkOfficeVerifications.Count <= 0 && inquiryGetVM.SalarySlipVerifications.Count <= 0 && inquiryGetVM.BankStatementVerifications.Count <= 0 && inquiryGetVM.TenantVerifications.Count <= 0)
                {
                    continue;
                }

                foreach (var item in inquiryGetVM. ResidenceVerifications)
                {
                    var rv = inquiry.ResidenceVerifications.FirstOrDefault(x => x.ResidenceVerificationId == item.ResidenceVerificationId);

                    rv.StatusDate = DateTime.Now;
                    rv.Status = constant.Status.InProgress;
                    Status s = db.Statuses.Create();
                    s.StatusDate = DateTime.Now;
                    s.StatusMessage = constant.Status.InProgress;
                    s.PersonType = item.PersonType;
                    s.VerificationType = constant.VerificationType.Residence;
                    s.UserId= userId;
                    //new field..
                    s.VerificationId = item.ResidenceVerificationId;
                    s.Descriptions = constant.StatusTypes.Downloaded;

                    inquiry.Statuses.Add(s);
                }
                foreach (var item in inquiryGetVM.WorkOfficeVerifications)
                {
                    var rv = inquiry.WorkOfficeVerifications.FirstOrDefault(x => x.WorkOfficeVerificationId == item.WorkOfficeVerificationId);

                    rv.StatusDate = DateTime.Now;
                    rv.Status = constant.Status.InProgress;
                    Status s = db.Statuses.Create();
                    s.StatusDate = DateTime.Now;
                    s.StatusMessage = constant.Status.InProgress;
                    s.PersonType = item.PersonType;
                    s.VerificationType = constant.VerificationType.WorkOffice;
                    s.UserId = userId;
                    //new field..
                    s.VerificationId = item.WorkOfficeVerificationId;
                    s.Descriptions = constant.StatusTypes.Downloaded;

                    inquiry.Statuses.Add(s);
                }

                foreach (var item in inquiryGetVM.TenantVerifications)
                {
                    var rv = inquiry.TenantVerifications.FirstOrDefault(x => x.TenantVerificationId== item.TenantVerificationId);

                    rv.StatusDate = DateTime.Now;
                    rv.Status = constant.Status.InProgress;
                    Status s = db.Statuses.Create();
                    s.StatusDate = DateTime.Now;
                    s.StatusMessage = constant.Status.InProgress;
                    s.PersonType = item.PersonType;
                    s.VerificationType = constant.VerificationType.Tenant;
                    s.UserId = userId;
                    //new field..
                    s.VerificationId = item.TenantVerificationId;
                    s.Descriptions = constant.StatusTypes.Downloaded;

                    inquiry.Statuses.Add(s);
                }
                foreach (var item in inquiryGetVM.SalarySlipVerifications)
                {
                    var rv = inquiry.SalarySlipVerifications.FirstOrDefault(x => x.SalarySlipVerificationId== item.SalarySlipVerificationId);

                    rv.StatusDate = DateTime.Now;
                    rv.Status = constant.Status.InProgress;
                    Status s = db.Statuses.Create();
                    s.StatusDate = DateTime.Now;
                    s.StatusMessage = constant.Status.InProgress;
                    s.PersonType = item.PersonType;
                    s.VerificationType = constant.VerificationType.SalarySlip;
                    s.UserId = userId;
                    //new field..
                    s.VerificationId = item.SalarySlipVerificationId;
                    s.Descriptions = constant.StatusTypes.Downloaded;

                    inquiry.Statuses.Add(s);
                }
                foreach (var item in inquiryGetVM.BankStatementVerifications)
                {
                    var rv = inquiry.BankStatementVerifications.FirstOrDefault(x => x.BankStatementVerificationId== item.BankStatementVerificationId);

                    rv.StatusDate = DateTime.Now;
                    rv.Status = constant.Status.InProgress;
                    Status s = db.Statuses.Create();
                    s.StatusDate = DateTime.Now;
                    s.StatusMessage = constant.Status.InProgress;
                    s.PersonType = item.PersonType;
                    s.VerificationType = constant.VerificationType.BankStatement;
                    s.UserId = userId;
                    //new field..
                    s.VerificationId = item.BankStatementVerificationId;
                    s.Descriptions = constant.StatusTypes.Downloaded;

                    inquiry.Statuses.Add(s);
                }




                if (inquiryGetVM.ResidenceVerifications.Count <= 0)
                { ResidenceVerifications = null; }

                if (inquiryGetVM.WorkOfficeVerifications.Count <= 0)
                    WorkOfficeVerifications = null;

                if (inquiryGetVM.BankStatementVerifications.Count <= 0)
                    BankStatementVerifications = null;

                if (inquiryGetVM.SalarySlipVerifications.Count <= 0)
                    SalarySlipVerifications = null;

                if (inquiryGetVM.TenantVerifications.Count <= 0)
                    TenantVerifications = null;

                modelList.Add(inquiryGetVM);
                db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return modelList;
        }
        public long InquiryId { get; set; }

        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public long CustomerBranchId { get; set; }
        public string CustomerBranchName { get; set; }

        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string AppNo { get; set; }
        
        //applicant detail 

        public string AppName { get; set; }
        public string AppContact { get; set; }
        public string AppCNIC { get; set; }

        public virtual List<ResidenceVerification> ResidenceVerifications { get; set; }
        public virtual List<WorkOfficeVerification> WorkOfficeVerifications { get; set; }
        public virtual List<TenantVerification> TenantVerifications { get; set; }


        public virtual List<SalarySlipVerification> SalarySlipVerifications { get; set; }
        public virtual List<BankStatementVerification> BankStatementVerifications { get; set; }

    }
    

  
}