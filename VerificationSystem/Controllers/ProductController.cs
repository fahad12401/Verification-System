using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerificationSystem.Models;
using VerificationSystem.DB;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using VerificationSystem.Extensions;
using System.Net;

namespace VerificationSystem.Controllers
{
    public class ProductController : Controller
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
            return View(db.Products.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Companies = new SelectList(db.Companies.Where(x => x.DisableDate == null ), "CompanyId", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var product = db.Products.Create();
                product.CompanyId = model.CompanyId;
                product.Description = model.Description;
                product.Name = model.Name;
                product.RecordAt = DateTime.Now;
                product.RecordBy = User.Identity.GetUserId();
                product.Sms = model.SMSApplicable;
                db.Products.Add(product);
                db.SaveChanges();
                this.AddNotification("Record Saved Succefully", NotificationType.Success);
                return RedirectToAction("Index", "Product");

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
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound("No product found.");
            }

            ProductEditVM model = new ProductEditVM
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                CompanyId = product.CompanyId,
                SMSApplicable = product.Sms
        };
            //if (product.DisableDate != null)
            //    model.DisableDate = product.DisableDate;
            ViewBag.Companies = new SelectList(db.Companies.Where(x => x.DisableDate == null), "CompanyId", "Name", model.CompanyId);
            return View(model);

        }
        [HttpPost]
        public ActionResult Edit(ProductEditVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Product product = db.Products.Find(model.ProductId);

                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.UpdateBy = User.Identity.GetUserId().ToString();
                    product.UpdateAt = DateTime.Now;
                    product.Sms = model.SMSApplicable;
                    
                    //if (model.Disabled)
                    //    product.DisableDate = model.DisableDate == null ? DateTime.Now : model.DisableDate;
                    //else
                    //    product.DisableDate = null;

                    db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();


                    this.AddNotification("Record Update Successfully", NotificationType.Success);
                    return RedirectToAction("Index");


                }
                ViewBag.Companies = new SelectList(db.Companies.Where(x => x.DisableDate == null ), "CompanyId", "Name", model.CompanyId);
                return View(model);

            }
            catch (Exception q)
            {

                this.AddNotification("Something went wrong at this time ERROR:" + q.Message, NotificationType.Error);
                ViewBag.Companies = new SelectList(db.Companies.Where(x => x.DisableDate == null ), "CompanyId", "Name", model.CompanyId);
                return View(model);
            }


        }
        public ActionResult Details(long? id, string message)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound("There is no Company is Selected");
            }

            ViewBag.Message = message;
            return View(product);
        }


        public ActionResult GetProductsByCompany(long companyId)
        {
            var products = db.Products.Where(x => x.CompanyId == companyId ).Select(x => new { x.ProductId, x.Name }).ToList();
            if (products.Count > 0)
                return Json(new { products = products, status = 1 }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
        }
    }
}