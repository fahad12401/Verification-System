using System;
using System.IO;
using System.Linq;
using VerificationSystem.DB;

namespace VerificationSystem.Extensions
{
    public static class GlobalHelper
    {


        public static string GetLastInnerMessage(Exception ex)
        {
            string message = "";
            for (int i = 0; ex != null; i++)
            {
                message += $@" {1 + i}:-{ex.Message }. {Environment.NewLine}";
                ex = ex.InnerException;
            }
            return message;
        }

        public static string SetImagePath(Inquiry inquiry,string personType,string verificationType)
        {
            string path = "";

            path += $@"Images EVS\{inquiry.CompanyName.Trim()}\{inquiry.CustomerBranchName.Trim()}\{inquiry.Product.Name.Trim()}\{inquiry.RecordAt.ToString("yyyy-MM-dd")}\{inquiry.InquiryId}_Id\{personType}\{verificationType}\";
            // {DateTime.Now.ToString("yyyyMMdd_hhmmss_ms")}.jpeg";


            return path;
        }



        public static int GetNumberOfWorkingDays(DateTime start, DateTime stop)
        {
            int days = 0;
            while (start <= stop)
            {
                if (start.DayOfWeek != DayOfWeek.Saturday && start.DayOfWeek != DayOfWeek.Sunday)
                {
                    ++days;
                }
                start = start.AddDays(1);
            }
            return days;
        }


        public static void WriteLog(string error, string detail = "", string userId = "", string actionName = "" , ApplicationDbContext db = null)
        {
            try
            {
                if (db == null)
                    db = new ApplicationDbContext();

                 var errorLog = db.ErrorLogs.Create();

                errorLog.ActionName = actionName;
                errorLog.UserId = userId;
                errorLog.Detail = detail;
                errorLog.Error = error;
                errorLog.RecortAt = DateTime.Now;
                db.ErrorLogs.Add(errorLog);
                db.Entry(errorLog).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public static string GetImageInBase64(string path)
        {
            string stringImage = "";
            try
            {
                if (File.Exists(path))
                {

                    byte[] bytes = HtmlExtensions. ReadImageFile(path);
                    if (bytes == null)
                        stringImage = "";
                    else
                    {
                        stringImage = Convert.ToBase64String(bytes);
                    }

                    return stringImage;

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return stringImage;

        }

        public static bool UserIsInRole( ApplicationUser user, string roleName, ApplicationDbContext db)
        {

            var role = db.Roles.FirstOrDefault(x => x.Name == roleName);

            if (role == null)
            {
                return false;
            }

            if (user.Roles.Any(x => x.RoleId == role.Id))
            {
                return true;
            }
            else return false;
        }

    }

}