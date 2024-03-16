using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace constant
{
    public static class Outcome
    {
        
        public const string Verified = "Satisfactory";
        public const string UnVerified = "UnSatisfactory";

        public static readonly string[] Outcomes = { Verified, UnVerified };
    }


    public static class Status
    {
        public const string Completed = "Completed"; // status date/./..
        public const string InProgress = "In Progress"; //inprogress date
        public const string Edited= "Edited"; // status date/./..

        public const string PartialComplete = "Partial Completed";
        public const string New = "New";//application Date 
        public const string Satisfactory = "Satisfactory";
        public const string UnSatisfactory = "UnSatisfactory";
        public const string Verified = "Verified";
        public const string UnVerified = "UnVerified";
        public const string QualityCheck = "Quality Check";

        

 
        public static readonly string[] Statuses = { Completed, InProgress, PartialComplete, New, QualityCheck };

    }
    public static class StatusTypes
    {
        public const string Created = "Created";
        public const string Downloaded = "Downloaded";

        public const string AssignedToFio = "Assigned To FIO.";

    }

    public static class Claims
    {
        public const string CustomerBranchId = "CustomerBranchId";
        public const string CustomerBranchName = "CustomerBranch";
        public const string BranchName = "Branch";
        public const string BranchId = "BranchId";
        public const string CustomerCode = "CustomerId";

        public const string CompanyId = "CompanyId";
        public const string CompanyName = "CompanyName";

    }

    public static class Roles
    {
        public const string SuperAdmin = "SUPER ADMIN";

        public const string Head = "User";

       

        public const string Fio = "FIO";

        

    }



    public static class VerificationType
    {
        public const string Residence = "Residence";
        public const string WorkOffice = "WorkOffice";
        public const string SalarySlip = "SalarySlip";
        public const string BankStatement = "BankStatement";
        public const string Tenant = "TenantVerification";
        
    }
    public static class PersonType
    {
        public const string Applicant = "Applicant";

        public const string Guarantor1 = "Guarantor 1";
        public const string Guarantor2 = "Guarantor 2";

        public const string Spouse1 = "Spouse 1";
        public const string CoApplicant = "CoApplicant";

        public const string Reference1 = "Reference 1";
        public const string Reference2 = "Reference 2";
        public const string Reference3 = "Reference 3";


        public static readonly string[] PersonTypes = { Applicant, Guarantor1, Guarantor2, Spouse1, CoApplicant , Reference1 , Reference2, Reference3};
        
    }
     
    public static class ApplicationSettings
    {
        public const string AuditStartDate = "AuditStartDate";
        public const string CompanyCodeForTwoImagesOnReportPage = "CompanyCodeForTwoImagesOnReportPage";
        public const string RootPath = "ImagesRootPath";
        public const string AuditPercentageRate = "AuditPercentageRate";
        public const string SpecificProduct = "SpecificProduct";
    }

    public static class GPS
    {
        public static string GetGpsURL(string latitude, string longitude)
        {
            return $"http://maps.google.com/?q={latitude},{longitude}";
        }
    }




}