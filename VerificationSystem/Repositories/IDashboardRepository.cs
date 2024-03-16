using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VerificationSystem.Models.Dashboard;

namespace VerificationSystem.Repositories
{
    public interface IDashboardRepository
    {
        Task<MainCountVM> GetMainCounts();

       
        Task<int> GetAllInquiriesCount();

        Task<int> GetAllVerificationCount();
        Task<int> GetAllBankStatementVerificationCount();
        Task<int> GetAllResidenceVerificationCount();
        Task<int> GetAllWorkOfficeVerificationCount();
        Task<int> GetAllTenantVerificationCount();
        Task<int> GetAllSalarySlipVerificationCount();
         
        Task<int> GetCompanyCount();
        Task<int> GetCustomerBranchCount();
        Task<int> GetProductCount();

        
    }
}