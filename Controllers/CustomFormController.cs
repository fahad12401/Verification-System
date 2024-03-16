using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerificationSystem.DB;

namespace VerificationSystem.Controllers
{
    public class CustomFormController : Controller
    {
        private ApplicationDbContext _db;

        public ApplicationDbContext db
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
            private set { _db = value; }
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}