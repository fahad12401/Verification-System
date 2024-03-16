using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VerificationSystem.DB;
using VerificationSystem.Extensions;
using VerificationSystem.Models;

namespace VerificationSystem.Controllers
{
    public class CustomerBranchController : Controller
    {
        private ApplicationDbContext _db;
        public ApplicationDbContext db
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
            private set { _db = value; }
        }

        // GET: Product
        public ActionResult Index()
        {
            return View(db.CustomerBranches.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Companies = new SelectList(db.Companies.Where(x => x.DisableDate == null ), "CompanyId", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(CustomerBranchCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var customerBranch = db.CustomerBranches.Create();
                customerBranch.CompanyId = model.CompanyId;
                customerBranch.Name = model.Name;
                customerBranch.Address1 = model.Address;
                customerBranch.Country = model.Country;
                customerBranch.City = model.City;
                customerBranch.Province = model.Province;
                customerBranch.Phone1 = model.Phone1;
                customerBranch.Phone2 = model.Phone2;
                customerBranch.Email = model.Email;

                if (model.Disabled )
                {
                    customerBranch.DisableDate = DateTime.Now;
                }



                db.CustomerBranches.Add(customerBranch);
                db.SaveChanges();
                this.AddNotification("Record Saved Succefully", NotificationType.Success);
                return RedirectToAction("Index" );

            }

            this.AddNotification("There is an error or some field is missing.", NotificationType.Info);

            ViewBag.Companies = new SelectList(db.Companies.Where(x => x.DisableDate == null), "CompanyId", "Name");
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerBranch  customerBranch = db.CustomerBranches.Find(id);
            if (customerBranch == null)
            {
                return HttpNotFound("No product found.");
            }

            CustomerBranchEditVM model = new CustomerBranchEditVM
            {
                CustomerBranchId = customerBranch.CustomerBranchId,
                Name = customerBranch.Name,
                CompanyId = customerBranch.CompanyId, Address = customerBranch.Address1, City = customerBranch.City, Country = customerBranch.Country, Email = customerBranch.Email, Phone1 = customerBranch.Phone1, Phone2 = customerBranch.Phone2, Province = customerBranch.Province
            };
            if (customerBranch.DisableDate != null)
                model.Disabled = true;
           ViewBag.Companies = new SelectList(db.Companies.Where(x => x.DisableDate == null), "CompanyId", "Name", model.CompanyId);
            return View(model);

        }
        [HttpPost]
        public ActionResult Edit(CustomerBranchEditVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CustomerBranch customerBranch = db.CustomerBranches.Find(model.CustomerBranchId);

                    customerBranch.Name = model.Name;
                    customerBranch.Address1 = model.Address;
                    customerBranch.Country = model.Country;
                    customerBranch.City = model.City;
                    customerBranch.Province = model.Province;
                    customerBranch.Phone1 = model.Phone1;
                    customerBranch.Phone2 = model.Phone2;
                    customerBranch.Email = model.Email;


                    if (model.Disabled)
                        customerBranch.DisableDate = customerBranch.DisableDate == null ? DateTime.Now : customerBranch.DisableDate;
                    else
                        customerBranch.DisableDate = null;
                    

                    db.Entry(customerBranch).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();


                    this.AddNotification("Record Update Successfully", NotificationType.Success);
                    return RedirectToAction("Index");
                }
                ViewBag.Companies = new SelectList(db.Companies.Where(x => x.DisableDate == null), "CompanyId", "Name", model.CompanyId);
                return View(model);
            }
            catch (Exception q)
            {
                this.AddNotification("Something went wrong at this time ERROR:" + q.Message, NotificationType.Error);
                ViewBag.Companies = new SelectList(db.Companies.Where(x => x.DisableDate == null), "CompanyId", "Name", model.CompanyId);
                return View(model);
            }


        }
        public ActionResult Details(long? id, string message)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerBranch  customerBranch = db.CustomerBranches.Find(id);
            if (customerBranch == null)
            {
                return HttpNotFound("There is no Company is Selected");
            }

            ViewBag.Message = message;
            return View(customerBranch);
        }

        
        public ActionResult GetBranchesByCompany(long companyId)
        {
            var branches = db.CustomerBranches.Where(x => x.CompanyId == companyId && x.DisableDate == null).Select(x=> new { x.CustomerBranchId, x.Name }).ToList();
            if (branches.Count > 0)
                return Json(new { branches = branches, status = 1 }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
        }
    }
}