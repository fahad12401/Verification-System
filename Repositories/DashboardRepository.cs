using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VerificationSystem.DB;
using VerificationSystem.Extensions;
using VerificationSystem.Models.Dashboard;

namespace VerificationSystem.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private ApplicationDbContext db;
        public DashboardRepository()
        {
            db = new ApplicationDbContext();
        }

        System.Security.Principal.IPrincipal User = HttpContext.Current.User;

        #region global properties for role purpose

        private long _BranchId { get; set; }
        public long _Claim_BranchId
        {
            get
            {
                if (_BranchId > 0) {
                    return _BranchId;
                }
                string userClaim = ControllerHelper.GetUserClaim(constant.Claims.BranchId);
                long parsedbranchId;
               if (!string.IsNullOrEmpty(userClaim) && long.TryParse(userClaim, out  parsedbranchId)){
                    return parsedbranchId;
                }
                else
                {
                    return 0;
                }
            }
            set { _BranchId = value; }
        }

        private long _CustomerBranchId { get; set; }
        public long _Claim_CustomerBranchId
        {
            get
            {
                if (_CustomerBranchId > 0) return _CustomerBranchId;

                return long.Parse(ControllerHelper.GetUserClaim(constant.Claims.CustomerBranchId));
            }
            set { _CustomerBranchId = value; }
        }

        private long _CompanyId { get; set; }
        public long _Claim_CompanyId
        {
            get
            {
                if (_CompanyId > 0) return _CompanyId;

                return long.Parse(ControllerHelper.GetUserClaim(constant.Claims.CompanyId));
            }
            set { _CompanyId = value; }
        }

        #endregion 

        public async Task<MainCountVM> GetMainCounts()
        {
            var model = new MainCountVM();

        
            if (User.IsInRole(constant.Roles.Head) || User.IsInRole(constant.Roles.SuperAdmin))
            {
                model.AllCount = await GetAllInquiriesCount();
                model.OpenCount = await db.Inquiries.CountAsync(x => x.Status == constant.Status.New);
                model.InProgressCount = await db.Inquiries.CountAsync(x => x.Status == constant.Status.InProgress);
                model.PartialCompletedCount = await db.Inquiries.CountAsync(x => x.Status == constant.Status.PartialComplete);
                model.QCCount = await db.Inquiries.CountAsync(x => x.Status == constant.Status.QualityCheck);
                model.CompletedCount = await db.Inquiries.CountAsync(x => x.Status == constant.Status.Completed);
                

                model.CompanyCount = await GetCompanyCount();
                model.CustomerBranchCount = await GetCustomerBranchCount();
                model.ProductCount = await GetProductCount();
                
                model.AllVerificationCount = await GetAllVerificationCount();
            }
           


            return model;
        }



        public async Task<int> GetAllInquiriesCount()
        {
           

            if (User.IsInRole(constant.Roles.SuperAdmin))
            {
                return await db.Inquiries.CountAsync(x => x.BranchId == _Claim_BranchId);
            }
            else
            {
                return 0;
            } 

           

          
        }




        public async Task<int> GetAllVerificationCount()
        {
            return await GetAllResidenceVerificationCount() +
               await GetAllTenantVerificationCount() +
               await GetAllWorkOfficeVerificationCount() +
               await GetAllSalarySlipVerificationCount() +
               await GetAllBankStatementVerificationCount();
        }



        public async Task<int> GetCompanyCount()
        {
           

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.Companies.CountAsync();

            

            return 0;

        }

        public async Task<int> GetCustomerBranchCount()
        {
           

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.CustomerBranches.CountAsync();

            

            return 0;
        }

        public async Task<int> GetProductCount()
        {
            

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.Products.CountAsync();


            return 0;
        }

     



        public async Task<int> GetAllBankStatementVerificationCount()
        {
            

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.BankStatementVerifications.CountAsync(x => x.Inquiry.BranchId == _Claim_BranchId);

         

            return 0;
        }

        public async Task<int> GetAllResidenceVerificationCount()
        {
          ;

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.ResidenceVerifications.CountAsync(x => x.Inquiry.BranchId == _Claim_BranchId);

            

            return 0;
        }

        public async Task<int> GetAllWorkOfficeVerificationCount()
        {
           

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.WorkOfficeVerifications.CountAsync(x => x.Inquiry.BranchId == _Claim_BranchId);

         

            return 0;
        }

        public async Task<int> GetAllTenantVerificationCount()
        {
            

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.TenantVerifications.CountAsync(x => x.Inquiry.BranchId == _Claim_BranchId);

            

            return 0;
        }

        public async Task<int> GetAllSalarySlipVerificationCount()
        {
            

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.SalarySlipVerifications.CountAsync(x => x.Inquiry.BranchId == _Claim_BranchId);

           

            return 0;
        }

        public async Task<int> GetAllBankStatementVerificationCountWithFilter(string outcome, long company)
        {
           

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.BankStatementVerifications.CountAsync(x => x.Inquiry.BranchId == _Claim_BranchId && x.OutComeVerification == outcome && x.Inquiry.CompanyId == company);

          

            return 0;
        }

        public async Task<int> GetAllResidenceVerificationCountWithFilter(string outcome, long company)
        {
            

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.ResidenceVerifications.CountAsync(x => x.Inquiry.BranchId == _Claim_BranchId && x.OutComeVerification == outcome && x.Inquiry.CompanyId == company);

           

            return 0;
        }

        public async Task<int> GetAllWorkOfficeVerificationCountWithFilter(string outcome, long company)
        {
            

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.WorkOfficeVerifications.CountAsync(x => x.Inquiry.BranchId == _Claim_BranchId && x.OutComeVerification == outcome && x.Inquiry.CompanyId == company);

            

            return 0;
        }

        public async Task<int> GetAllTenantVerificationCountWithFilter(string outcome, long company)
        {
           

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.TenantVerifications.CountAsync(x => x.Inquiry.BranchId == _Claim_BranchId && x.OutComeVerification == outcome && x.Inquiry.CompanyId == company);

            

            return 0;
        }

        public async Task<int> GetAllSalarySlipVerificationCountWithFilter(string outcome, long company)
        {
           

            if (User.IsInRole(constant.Roles.SuperAdmin))
                return await db.SalarySlipVerifications.CountAsync(x => x.Inquiry.BranchId == _Claim_BranchId);

            

            return 0;
        }





    }
}