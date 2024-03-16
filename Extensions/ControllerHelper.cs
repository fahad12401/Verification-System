using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using VerificationSystem.DB;

namespace VerificationSystem.Extensions
{
    public static class ControllerHelper
    {
        public static string GetUserClaim(this ControllerBase controller, string type)
        {
            try
            {
                var claims = controller.ControllerContext.HttpContext.User.Identity as ClaimsIdentity;
                if (claims.FindFirst(type) == null)
                    return null;
                return claims.FindFirst(type).Value;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static string GetUserClaim(string type)
        {
            try
            {
                var claims = HttpContext.Current.User.Identity as ClaimsIdentity;
                //controller.ControllerContext.HttpContext.User.Identity as ClaimsIdentity;
                if (claims.FindFirst(type) == null)
                    return null;
                return claims.FindFirst(type).Value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static string RenderPartialViweToString(this ControllerBase controller, string viewName, object model)
        {

            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }

        }

        public static List<DB.ApplicationUser> GetAllUsers(this ControllerBase controller, string roleName)
        {
            try
            {
                using (var db = new DB.ApplicationDbContext())
                {
                    var roleId = db.Roles.FirstOrDefault(x => x.Name == roleName).Id;
                    return db.Users.Where(x => x.Roles.Any(s => s.RoleId == roleId)).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Inquiry Authorize(this ControllerBase controller, ApplicationDbContext db, long _id)
        {

            Inquiry result = null;
            var user = controller.ControllerContext.HttpContext.User;

            if (user.IsInRole(constant.Roles.SuperAdmin) || user.IsInRole(constant.Roles.Head))
            {
                return db.Inquiries.FirstOrDefault(x => x.InquiryId == _id);
            }



            if (user.IsInRole(constant.Roles.Head))
            {
                var branchId = long.Parse(GetUserClaim(constant.Claims.BranchId));
                result = db.Inquiries.FirstOrDefault(x => x.InquiryId == _id && x.BranchId == branchId);
                if (result != null)
                    return result;
            }
           

            return null;
        }
        public static bool IsAuthorize(this ControllerBase controller, ApplicationDbContext db, long _id)
        {

            Inquiry result = null;
            var user = controller.ControllerContext.HttpContext.User;

            if (user.IsInRole(constant.Roles.SuperAdmin))
            {
                return db.Inquiries.Any(x => x.InquiryId == _id);
            }

       


            if (user.IsInRole(constant.Roles.Head))
            {
                var branchId = long.Parse(GetUserClaim(constant.Claims.BranchId));
                result = db.Inquiries.FirstOrDefault(x => x.InquiryId == _id && x.BranchId == branchId);
                if (result != null)
                    return true;
            }
        

            return false;
        }


    }

}