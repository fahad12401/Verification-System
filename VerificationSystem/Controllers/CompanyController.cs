using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerificationSystem.DB;
using VerificationSystem.Models;
using VerificationSystem.Extensions;
using System.Net;
using System.Data.Entity;

namespace VerificationSystem.Controllers
{
    public class CompanyController : Controller
    {
        private ApplicationDbContext _db;
        public ApplicationDbContext db
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
            private set { _db = value; }
        }
        
        // GET: Company
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        //Get Company Create
        public ActionResult Create()
        {

            return View();

        }

        //Post Company Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(CompanyVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                Company company = db.Companies.Create();

                company.Name = model.Name;
                company.Phone1 = model.Phone;
                company.Phone2 = model.Phone2;
                company.Descriptions = model.Description;
                company.Address = model.Address;
                company.RecordBy = User.Identity.GetUserId();
                company.RecordAt = DateTime.Now;
              

                if (model.Disabled) company.DisableDate = DateTime.Now;


                db.Companies.Add(company);
                db.SaveChanges();
                this.AddNotification("Record Saved Succefully", NotificationType.Success);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.AddNotification("Something went wrong at this time ERROR:" + ex.Message, NotificationType.Error);
                return View(model);
            }

        }

        //Get Company Edit
        [HttpGet]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound("There is no Company Selected");
            }

            CompanyVM model = new CompanyVM
            {
                Id = company.CompanyId,
                Name = company.Name,
                Description = company.Descriptions,
                Phone = company.Phone1,
                Phone2 = company.Phone2,
                Address = company.Address
                

            };

            if (company.DisableDate !=  null)
            {
                model.DisableDate = company.DisableDate;
                model.Disabled = true;
            }
                

            return View(model);

        }


        //Post Company Edit
        [HttpPost]
        public ActionResult Edit(CompanyVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Company company = db.Companies.Find(model.Id);

                    company.Name = model.Name;
                    company.Phone1 = model.Phone;
                    company.Phone2 = model.Phone2;
                    company.Descriptions = model.Description;
                    company.Address = model.Address;
                    company.UpdateBy = User.Identity.GetUserId().ToString();
                    company.UpdateAt = DateTime.Now;
                  

                    if (model.Disabled )
                        company.DisableDate = model.DisableDate == null ? DateTime.Now : model.DisableDate;
                    else
                        company.DisableDate = null;

                    db.Entry(company).State = EntityState.Modified;
                    db.SaveChanges();


                    this.AddNotification("Record Update Successfully", NotificationType.Success);
                    return RedirectToAction("Index");


                }

                return View(model);

            }
            catch (Exception q)
            {

                this.AddNotification("Something went wrong at this time ERROR:" + q.Message, NotificationType.Error);
                return View(model);
            }


        }

        //Get Details
        public ActionResult Details(long? id, string message)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound("There is no Company is Selected");
            }

            ViewBag.Message = message;
            return View(company);
        }


    }
}