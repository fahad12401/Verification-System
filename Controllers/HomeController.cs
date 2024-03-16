using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerificationSystem.DB;
using VerificationSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using VerificationSystem.Extensions;

namespace VerificationSystem.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;
        public ApplicationDbContext db
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
            private set { _db = value; }
        }

        public ActionResult Index()
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
          
            if (User.IsInRole(constant.Roles.Head) || User.IsInRole(constant.Roles.SuperAdmin))
            {
               var TotalCases = db.Inquiries.Where(x => x.InquiryId > 0);
                model.Total = TotalCases.Count();
                model.Open = TotalCases.Count(x => x.Status == constant.Status.New);
                model.InProgress = TotalCases.Count(x => x.Status == constant.Status.InProgress);
                model.Partial = TotalCases.Count(x => x.Status == constant.Status.PartialComplete);
                model.QualityCheck = TotalCases.Count(x => x.Status == constant.Status.QualityCheck);
                model.Completed = TotalCases.Count(x => x.Status == constant.Status.Completed);
                
                model.OpenCurrent = TotalCases.Count(x => x.Status == constant.Status.New && x.StatusDate.Month == DateTime.Now.Month);
                model.InProgressCurrent = TotalCases.Count(x => x.Status == constant.Status.InProgress && x.StatusDate.Month == DateTime.Now.Month);
                model.PartialCurrent = TotalCases.Count(x => x.Status == constant.Status.PartialComplete && x.StatusDate.Month == DateTime.Now.Month);
                model.QualityCheckCurrent = TotalCases.Count(x => x.Status == constant.Status.QualityCheck && x.StatusDate.Month == DateTime.Now.Month);
                model.CompletedCurrent = TotalCases.Count(x => x.Status == constant.Status.Completed && x.StatusDate.Month == DateTime.Now.Month);
               

                model.InquiriesCompleted = TotalCases.Where(x => x.Status == constant.Status.Completed).Take(20).OrderByDescending(x => x.StatusDate).ToList();
                model.InquiriesInProgress = TotalCases.Where(x => x.Status == constant.Status.InProgress).Take(20).OrderByDescending(X => X.StatusDate).ToList();
            }
            


            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}