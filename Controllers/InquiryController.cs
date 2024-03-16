using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerificationSystem.DB;
using VerificationSystem.Models;
using VerificationSystem.Extensions;
using Microsoft.AspNet.Identity;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json.Linq;
using NinjaNye.SearchExtensions;

using System.Threading.Tasks;

namespace VerificationSystem.Controllers
{
    public class InquiryController : Controller
    {

        private ApplicationDbContext _db;
        public ApplicationDbContext db
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
            private set { _db = value; }
        }




        // GET: Inquiry
        public ActionResult Index(int? page, string st, string cm, string ib, string cb, string p, string an, string id, string ac)
        {
            var model = new InquiryIndexVM(db, page, st, cm, ib, cb, p, an, id,ac);
            return View(model);
        }


        [HttpPost]
        public ActionResult  Address(Address model)
        {

            if (ModelState.IsValid)
            {
                Address address;
                if (model.AddressId > 0 )
                {
                    address = db.Addresses.FirstOrDefault(x => x.AddressId == model.AddressId);
                }
                else
                {
                    address = db.Addresses.Create();
                }

                address.Address1 = model.Address1;
                address.Address2 = model.Address2;
                address.Address3 = model.Address3;

                address.City = model.City;

                address.InquiryId = model.InquiryId;
                address.PersonType = model.PersonType;
                address.Province = model.Province;
                address.RecordedBy = User.Identity.GetUserId();
                address.RecordAt = DateTime.Now;
                address.VerificationType = model.VerificationType;
                

                db.Addresses.Add(address);
                if ( model.AddressId > 0 )
                    db.Entry(address).State = System.Data.Entity.EntityState.Modified;
                else
                    db.Entry(address).State = System.Data.Entity.EntityState.Added;

                db.SaveChanges();
                return Json(new { message = $"Address " + (model.AddressId > 0 ? "Updated" : "Saved") + " successfully. ", status = 1, addressId = address.AddressId }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "Field validation error.", status = 0 }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult Create(string id)
        {

            long _id = 0;

            //checking if inquiry id already exist in records, if exists then call different constructor
            if (string.IsNullOrEmpty(id) == false && long.TryParse(id, out _id))
            {
                //Checking if Inquiry Id is correct or not
                var model = new InquiryCreateVM(db, _id);
                return View(model);
            }
            else
            {
                var model = new InquiryCreateVM(db);
                return View(model);

            }
        }
        [HttpPost]

        public async Task<ActionResult> Create(InquiryCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var inquiry = model.Save();
                if (model._status)
                {
                    var ProductName = db.Products.FirstOrDefault(x => x.ProductId == model.ProductId);
               
                    return Json(new { status = 1, inquiryId = inquiry.InquiryId, message = "Inquiry Created susscessfully." });
                }
                else
                    return Json(new { status = 0, message = $"something went wront during the save record please try again later. Message:{model.message}" });
            }

            return Json(new { status = 0, message = "something wrong at this time. Please try again later." });


        }



        [HttpPost]
        public ActionResult CreateResidence(ResidenceCreateVM model)
        {
            if (model == null || model.InquiryId <= 0)
            {
                return Json(new { message = "No id was found.", status = 0 }, JsonRequestBehavior.AllowGet);
            }

            var inquiry = db.Inquiries.FirstOrDefault(x => x.InquiryId == model.InquiryId);


            ResidenceVerification residence;

            if (model.ResidenceVerificationId > 0)
            {
                residence = inquiry.ResidenceVerifications.FirstOrDefault(x => x.ResidenceVerificationId == model.ResidenceVerificationId);

                //checking if person type already exist or not
                if (model.PersonType != model.OldPersonType)
                {
                    if (inquiry.ResidenceVerifications.Any(x => x.PersonType == model.PersonType))
                    {
                        return Json(new { message = "cannot add multiple Residence Verificaiton for same person type.", status = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                if (inquiry.ResidenceVerifications.Any(x => x.PersonType == model.PersonType))
                    return Json(new { message = "cannot add multiple Residence Verificaiton for same person type.", status = 0 }, JsonRequestBehavior.AllowGet);

                residence = db.ResidenceVerifications.Create();

            }

            residence.PersonName = model.PersonName;

            residence.PersonType = model.PersonType;

            residence.PersonCNIC = model.PersonCNIC;

            residence.PersonContactNo = model.PersonContactNo;

            residence.ResidenceAddress = model.ResidenceAddress;

            residence.NearestLandMark = model.NearestLandMark;

            residence.InquiryId = inquiry.InquiryId;

            residence.Status = constant.Status.New;


            residence.StatusDate = DateTime.Now;


            var status = db.Statuses.Create();

            status.UserId = User.Identity.GetUserId();

            status.PersonType = residence.PersonType;

            status.VerificationType = constant.VerificationType.Residence;

            status.StatusDate = DateTime.Now;

            if (model.ResidenceVerificationId > 0)
            {

                status.StatusMessage = constant.Status.Edited;

                status.Descriptions = "Recorded after create.....";

                status.VerificationId = model.ResidenceVerificationId.Value;

                inquiry.Statuses.Add(status);

                db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                status.StatusMessage = constant.Status.New;

                status.Descriptions = "Newly created Residence Verification.....";

                inquiry.ResidenceVerifications.Add(residence);

                db.SaveChanges();

                status.VerificationId = residence.ResidenceVerificationId;
                inquiry.Statuses.Add(status);

                db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;

            }



            if (db.SaveChanges() > 0)
            {
                var residenceList = db.ResidenceVerifications.Where(x => x.InquiryId == model.InquiryId).Select(x => new { x.ResidenceVerificationId, x.InquiryId, x.PersonType, x.PersonName, x.PersonCNIC, x.PersonContactNo, x.ResidenceAddress, x.NearestLandMark }).ToList();
                return Json(new { data = residenceList, status = 1, message = "Verification detail successfully saved." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = "", status = 0, message = "somthing went wrong at this time." }, JsonRequestBehavior.AllowGet);
            }


        }


        [HttpPost]
        public ActionResult CreateWorkOffice(WorkOfficeVM model)
        {
            if (model == null || model.InquiryId <= 0)
            {
                return Json(new { message = "No id was found.", status = 0 }, JsonRequestBehavior.AllowGet);
            }

            var inquiry = db.Inquiries.FirstOrDefault(x => x.InquiryId == model.InquiryId);


            WorkOfficeVerification workoffice;

            if (model.WorkOfficeVerificationId > 0)
            {
                workoffice = inquiry.WorkOfficeVerifications.FirstOrDefault(x => x.WorkOfficeVerificationId == model.WorkOfficeVerificationId);

                //checking if person type already exist or not
                if (model.PersonType != model.OldPersonType)
                {
                    if (inquiry.WorkOfficeVerifications.Any(x => x.PersonType == model.PersonType))
                    {
                        return Json(new { message = "cannot add multiple Work/Office Verification for same person type.", status = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            else
            {
                if (inquiry.WorkOfficeVerifications.Any(x => x.PersonType == model.PersonType))
                    return Json(new { message = "cannot add multiple Work/Office Verification for same person type.", status = 0 }, JsonRequestBehavior.AllowGet);

                workoffice = db.WorkOfficeVerifications.Create();

            }



            workoffice.PersonType = model.PersonType;

            workoffice.PersonContactNo = model.PersonContactNo;

            workoffice.PersonDesignation = model.PersonDesignation;

            workoffice.PersonName = model.PersonName;

            workoffice.OfficeName = model.OfficeName;

            workoffice.OfficeAddress = model.OfficeAddress;

            workoffice.NearestLandMark = model.NearestLandMark;

            workoffice.InquiryId = model.InquiryId;

            workoffice.Status = constant.Status.New;

            workoffice.StatusDate = DateTime.Now;

            var status = db.Statuses.Create();

            status.UserId = User.Identity.GetUserId();

            status.PersonType = workoffice.PersonType;

            status.VerificationType = constant.VerificationType.WorkOffice;

            status.StatusDate = DateTime.Now;

            if (model.WorkOfficeVerificationId > 0)
            {
                status.StatusMessage = constant.Status.Edited;

                status.Descriptions = "Recorded after create.....";

                status.VerificationId = model.WorkOfficeVerificationId.Value;

                inquiry.Statuses.Add(status);

                db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                status.StatusMessage = constant.Status.New;

                status.Descriptions = "Newly created Work/Office Verification.....";

                inquiry.WorkOfficeVerifications.Add(workoffice);

                db.SaveChanges();

                status.VerificationId = workoffice.WorkOfficeVerificationId;

                inquiry.Statuses.Add(status);

                db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;

            }
              

            if (db.SaveChanges() > 0)
            {
                var workOfficeList = db.WorkOfficeVerifications.Where(x => x.InquiryId == model.InquiryId).Select(x => new { x.WorkOfficeVerificationId, x.InquiryId, x.PersonType, x.PersonName, x.PersonContactNo, x.PersonDesignation, x.OfficeName, x.OfficeAddress, x.NearestLandMark }).ToList();
                return Json(new { data = workOfficeList, status = 1, message = "Verification detail successfully saved." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = "", status = 0, message = "somthing went wrong at this time." }, JsonRequestBehavior.AllowGet);
            }


        }



        [HttpPost]
        public ActionResult CreateSalarySlip(SalarySlipVM model)
        {


            if (model == null || model.InquiryId <= 0)
            {
                return Json(new { message = "No id was found.", status = 0 }, JsonRequestBehavior.AllowGet);
            }

            var inquiry = db.Inquiries.FirstOrDefault(x => x.InquiryId == model.InquiryId);

            SalarySlipVerification salarySlip;


            if (model.SalarySlipVerificationId > 0)
            {
                salarySlip = inquiry.SalarySlipVerifications.FirstOrDefault(x => x.SalarySlipVerificationId == model.SalarySlipVerificationId);

                //checking if person type already exist or not
                if (model.PersonType != model.OldPersonType)
                {
                    if (inquiry.SalarySlipVerifications.Any(x => x.PersonType == model.PersonType))
                    {
                        return Json(new { message = "cannot add multiple Salary Slip Verificaiton for same person type.", status = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            else
            {
                if (inquiry.SalarySlipVerifications.Any(x => x.PersonType == model.PersonType))
                    return Json(new { message = "cannot add multiple Salary Slip Verificaiton for same person type.", status = 0 }, JsonRequestBehavior.AllowGet);
                salarySlip = db.SalarySlipVerifications.Create();

            }

            salarySlip.PersonType = model.PersonType;


            salarySlip.PersonContactNo = model.PersonContactNo;

            salarySlip.PersonName = model.PersonName;

            salarySlip.OfficeName = model.OfficeName;

            salarySlip.OfficeAddress = model.OfficeAddress;

            salarySlip.NearestLandMark = model.NearestLandMark;

            salarySlip.InquiryId = model.InquiryId;

            salarySlip.Status = constant.Status.New;

            salarySlip.StatusDate = DateTime.Now;

          

            var status = db.Statuses.Create();

            status.UserId = User.Identity.GetUserId();

            status.PersonType = salarySlip.PersonType;

            status.VerificationType = constant.VerificationType.SalarySlip;

            status.StatusDate = DateTime.Now;

            if (model.SalarySlipVerificationId > 0)
            {
                status.StatusMessage = constant.Status.Edited;

                status.Descriptions = "Recorded after create.....";

                status.VerificationId = model.SalarySlipVerificationId.Value;

                inquiry.Statuses.Add(status);

                db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                status.StatusMessage = constant.Status.New;

                status.Descriptions = "Newly created Salary Slip Verification.....";

                inquiry.SalarySlipVerifications.Add(salarySlip);

                db.SaveChanges();

                status.VerificationId = salarySlip.SalarySlipVerificationId;

                inquiry.Statuses.Add(status);

                db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;
            }
            
            if (db.SaveChanges() > 0)
            {



                var salarySlipList = db.SalarySlipVerifications.Where(x => x.InquiryId == model.InquiryId).Select(x => new { x.SalarySlipVerificationId, x.InquiryId, x.PersonType, x.PersonName, x.PersonContactNo, x.OfficeName, x.OfficeAddress, x.NearestLandMark }).ToList();
                return Json(new { data = salarySlipList, status = 1, message = "Verification detail successfully saved." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = "", status = 0, message = "somthing went wrong at this time." }, JsonRequestBehavior.AllowGet);
            }
        }

     


        [HttpPost]
        public ActionResult CreateBankStatement(BankStatementCreateVM model)
        {
            if (model == null || model.InquiryId <= 0)
            {
                return Json(new { message = "No id was found.", status = 0 }, JsonRequestBehavior.AllowGet);
            }

            var inquiry = db.Inquiries.FirstOrDefault(x => x.InquiryId == model.InquiryId);

            BankStatementVerification bankStatementVerification;

            if (model.BankStatementVerificationId > 0)
            {
                bankStatementVerification = inquiry.BankStatementVerifications.FirstOrDefault(x => x.BankStatementVerificationId == model.BankStatementVerificationId);

              //  checking if person type already exist or not
                if (model.PersonType != model.OldPersonType)
                {
                    if (inquiry.BankStatementVerifications.Any(x => x.PersonType == model.PersonType))
                    {
                        return Json(new { message = "cannot add multiple Bank Statement Verificaiton for same person type.", status = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            else
            {
                if (inquiry.BankStatementVerifications.Any(x => x.PersonType == model.PersonType))
                    return Json(new { message = "cannot add multiple Bank Statement Verificaiton for same person type.", status = 0 }, JsonRequestBehavior.AllowGet);

                bankStatementVerification = db.BankStatementVerifications.Create();

            }

            bankStatementVerification.PersonType = model.PersonType;

            bankStatementVerification.PersonContactNo = model.PersonContactNo;

            bankStatementVerification.PersonName = model.PersonName;

            bankStatementVerification.BankName = model.BankName;

            bankStatementVerification.BankAddress = model.BankAddress;

            bankStatementVerification.NearestLandMark = model.NearestLandMark;

            bankStatementVerification.InquiryId = model.InquiryId;

            bankStatementVerification.Status = constant.Status.New;

            bankStatementVerification.StatusDate = DateTime.Now;

            var status = db.Statuses.Create();

            status.UserId = User.Identity.GetUserId();

            status.PersonType = bankStatementVerification.PersonType;

            status.VerificationType = constant.VerificationType.BankStatement;

            status.StatusDate = DateTime.Now;

            if (model.BankStatementVerificationId > 0)
            {
                status.StatusMessage = constant.Status.Edited;

                status.Descriptions = "Recorded after create.....";

                status.VerificationId = model.BankStatementVerificationId.Value;

                inquiry.Statuses.Add(status);

                db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                status.StatusMessage = constant.Status.New;

                status.Descriptions = "Newly created Bank Statement Verification.....";

                inquiry.BankStatementVerifications.Add(bankStatementVerification);

                db.SaveChanges();

                status.VerificationId = bankStatementVerification.BankStatementVerificationId;

                inquiry.Statuses.Add(status);
               
                db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;
            }
             

            if (db.SaveChanges() > 0)
            {
                var bankStatemetnList = db.BankStatementVerifications.Where(x => x.InquiryId == model.InquiryId).Select(x => new { x.BankStatementVerificationId, x.InquiryId, x.PersonType, x.PersonName, x.PersonContactNo, x.BankName, x.BankAddress, x.NearestLandMark }).ToList();
                return Json(new { data = bankStatemetnList, status = 1, message = "Verification detail successfully saved." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = "", status = 0, message = "somthing went wrong at this time." }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult CreateTenant(TenantCreateVM model)
        {
            if (model == null || model.InquiryId <= 0)
            {
                return Json(new { message = "No id was found.", status = 0 }, JsonRequestBehavior.AllowGet);
            }

            var inquiry = db.Inquiries.FirstOrDefault(x => x.InquiryId == model.InquiryId);

            TenantVerification tenantVerification;

            if (model.TenantVerificationId > 0)
            {
                tenantVerification = inquiry.TenantVerifications.FirstOrDefault(x => x.TenantVerificationId == model.TenantVerificationId);

                //checking if person type already exist or not
                if (model.PersonType != model.OldPersonType)
                {
                    if (inquiry.TenantVerifications.Any(x => x.PersonType == model.PersonType))
                    {
                        return Json(new { message = "cannot add multiple Tenant Verificaiton for same person type.", status = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                if (inquiry.TenantVerifications.Any(x => x.PersonType == model.PersonType))
                    return Json(new { message = "cannot add multiple Tenant Verificaiton for same person type.", status = 0 }, JsonRequestBehavior.AllowGet);
                tenantVerification = db.TenantVerifications.Create();

            }
            
            tenantVerification.PersonType = model.PersonType;

            tenantVerification.PersonName = model.PersonName;

            tenantVerification.PersonContactNo = model.PersonContactNo;

            tenantVerification.TenantAddress = model.TenantAddress;

            tenantVerification.NearestLandMark = model.NearestLandMark;

            tenantVerification.InquiryId = model.InquiryId;

            tenantVerification.Status = constant.Status.New;

            tenantVerification.StatusDate = DateTime.Now;

            var status = db.Statuses.Create();

            status.UserId = User.Identity.GetUserId();

            status.PersonType = tenantVerification.PersonType;

            status.VerificationType = constant.VerificationType.Tenant;

            status.StatusDate = DateTime.Now;

            if (model.TenantVerificationId > 0)
            {
                status.StatusMessage = constant.Status.Edited;

                status.Descriptions = "Recorded after create.....";

                status.VerificationId = model.TenantVerificationId.Value;

                inquiry.Statuses.Add(status);

                db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                status.StatusMessage = constant.Status.New;

                status.Descriptions = "Newly created Tanent Verification.....";

                inquiry.TenantVerifications.Add(tenantVerification);

                db.SaveChanges();

                status.VerificationId = tenantVerification.TenantVerificationId;

                inquiry.Statuses.Add(status);

                db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;
            }

            if (db.SaveChanges() > 0)
            {
                var tenantList = db.TenantVerifications.Where(x => x.InquiryId == model.InquiryId).Select(x => new { x.TenantVerificationId, x.InquiryId, x.PersonType, x.PersonName, x.PersonContactNo, x.TenantAddress, x.NearestLandMark }).ToList();

                status.UserId = User.Identity.GetUserId();

                return Json(new { data = tenantList, status = 1, message = "Verification detail successfully saved." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = "", status = 0, message = "somthing went wrong at this time." }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult InquiryDetail(string id)
        {
            
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            long _id = 0;
            if (long.TryParse(id, out _id))
            {
                Inquiry model = null;
                model = this.Authorize(db, _id);

               

                if (model == null)
                {
                    this.AddNotification("Unauthorized Access.", NotificationType.Error);
                    return RedirectToAction("Index", "Inquiry");
                }

                //hold days count work...
                int HoldCount = 0;
                var HoldStatusObject = db.Statuses.FirstOrDefault(x => x.StatusMessage == constant.Status.InProgress && x.InquiryId == _id);
                if (HoldStatusObject != null)
                {
                    var InProgressStatusObject = db.Statuses.FirstOrDefault(x => ( x.StatusMessage == constant.Status.InProgress  || x.StatusMessage == constant.Status.QualityCheck) && x.InquiryId == _id && x.StatusId > HoldStatusObject.StatusId);
                    HoldCount = GlobalHelper.GetNumberOfWorkingDays(HoldStatusObject.StatusDate, InProgressStatusObject != null ? InProgressStatusObject.StatusDate : DateTime.Now);
                    
                  
                }
                model.HoldDaysCount = HoldCount;

                FillIdsForViewBag(model);

                return View(model);
            }
            else
            {
                this.AddNotification("No inquiry found.", NotificationType.Error);
                return RedirectToAction("Index", "Inquiry");
            }
        }

        private void FillIdsForViewBag(Inquiry model)
        {
            //fetching master object..
            var resMaster = model.PersonsVerifications?.ToList().Select(x => x.ResidenceVerifications.ToList().Select(y => y.ResidenceVerificationId).ToArray());
            var workOfficeMaster = model.PersonsVerifications?.ToList().Select(x => x.WorkOfficeVerifications.ToList().Select(y => y.WorkOfficeVerificationId).ToArray());
            var bankStatementMaster = model.PersonsVerifications?.ToList().Select(x => x.BankStatementVerifications.ToList().Select(y => y.BankStatementVerificationId).ToArray());
            var salarySlipMaster = model.PersonsVerifications?.ToList().Select(x => x.SalarySlipVerifications.ToList().Select(y => y.SalarySlipVerificationId).ToArray());
            var tenantMaster = model.PersonsVerifications?.ToList().Select(x => x.TenantVerifications.ToList().Select(y => y.TenantVerificationId).ToArray());

            //declaring arrays for ids...
            var resIds = new List<long>();
            var workOfficeIds = new List<long>();
            var bankStatementIds = new List<long>();
            var salarySlipIds = new List<long>();
            var tenantIds = new List<long>();

            //filling list of ids...
            foreach (var item in resMaster)
            {
                foreach (var item2 in item)
                    resIds.Add(item2);
            }

            foreach (var item in workOfficeMaster)
            {
                foreach (var item2 in item)
                    workOfficeIds.Add(item2);
            }
           
            foreach (var item in bankStatementMaster)
            {
                foreach (var item2 in item)
                    bankStatementIds.Add(item2);
            }

            foreach (var item in salarySlipMaster)
            {
                foreach (var item2 in item)
                    salarySlipIds.Add(item2);
            }
            
            foreach (var item in tenantMaster)
            {
                foreach (var item2 in item)
                    tenantIds.Add(item2);
            }

            //assigning to view bag...
            ViewBag.ResidenceIDS = resIds.ToArray();
            ViewBag.WorkOfficeIDS = workOfficeIds.ToArray();
            ViewBag.BankStatementIDS = bankStatementIds.ToArray();
            ViewBag.SalarySlipIDS = salarySlipIds.ToArray();
            ViewBag.TenantIDS = tenantIds.ToArray();
        }

        [AllowAnonymous]
        public ActionResult AssignInquiry(long? inquiryId, long? id, string[] userIds, string personType, string verificationType)
        {

            try
            {
                var inquiry = db.Inquiries.FirstOrDefault(x => x.InquiryId == inquiryId );
                
                //new field verifitionId
                //get all relation with that specific inquiry
                var iAppUsers = inquiry.InquiryApplicationUsers.Where(x => x.PersonType == personType 
                && x.VerificationType == verificationType && x.VerificationId == id).ToList();

                userIds = userIds.Except(iAppUsers.Select(x => x.UserId).ToArray()).ToArray();

                if (userIds.Count() <= 0)
                    return Json(new { message = "No new user were selected.", status = 1, tyep = "info " }, JsonRequestBehavior.AllowGet);


                string assignedUsers = "";
                var fioUsers = db.Users.Where(x => userIds.Contains(x.Id)).ToList();

                if (inquiry != null && inquiry.Status != constant.Status.Completed  && inquiry.Status != constant.Status.QualityCheck)
                {
                    if (inquiry.Status != constant.Status.PartialComplete )
                    {
                        inquiry.Status = constant.Status.InProgress;
                        inquiry.StatusDate = DateTime.Now;
                    }


                    foreach (var user in fioUsers)
                    {
                        var newAppUser = db.InquiryApplicationUsers.Create();
                        newAppUser.UserId = user.Id;
                        newAppUser.VerificationType = verificationType;
                        newAppUser.PersonType = personType;
                        //new field verificationId
                        newAppUser.VerificationId = id.HasValue ? id.Value : 0;
                        inquiry.InquiryApplicationUsers.Add(newAppUser);

                        assignedUsers += "{ " + user.UserName + " },";
                    }

                    if (verificationType == constant.VerificationType.Residence)
                    {
                        var residence = inquiry.ResidenceVerifications.FirstOrDefault(x => x.ResidenceVerificationId == id && x.Status != constant.Status.QualityCheck && x.Status != constant.Status.Completed);
                        if (residence == null) return Json(new { message = "Completed or In Quality Check.", status = 0, type = "info" }, JsonRequestBehavior.AllowGet);
                        residence.Status = constant.Status.InProgress;
                        residence.StatusDate = DateTime.Now;
                    }
                    else if (verificationType == constant.VerificationType.WorkOffice)
                    {
                        var officeBusines = inquiry.WorkOfficeVerifications.FirstOrDefault(x => x.WorkOfficeVerificationId == id && x.Status != constant.Status.QualityCheck && x.Status != constant.Status.Completed);
                        if (officeBusines == null) return Json(new { message = "Completed or In Quality Check.", status = 0, type = "info" }, JsonRequestBehavior.AllowGet);
                        officeBusines.Status = constant.Status.InProgress;
                        officeBusines.StatusDate = DateTime.Now;
                    }

                    else if (verificationType == constant.VerificationType.BankStatement)
                    {
                        var bankSt = inquiry.BankStatementVerifications.FirstOrDefault(x => x.BankStatementVerificationId == id && x.Status != constant.Status.QualityCheck && x.Status != constant.Status.Completed);
                        if (bankSt == null) return Json(new { message = "Completed or In Quality Check.", status = 0, type = "info" }, JsonRequestBehavior.AllowGet);
                        bankSt.Status = constant.Status.InProgress;
                        bankSt.StatusDate = DateTime.Now;
                    }
                    else if (verificationType == constant.VerificationType.SalarySlip)
                    {
                        var salary = inquiry.SalarySlipVerifications.FirstOrDefault(x => x.SalarySlipVerificationId == id && x.Status != constant.Status.QualityCheck && x.Status != constant.Status.Completed);
                        if (salary == null) return Json(new { message = "Completed or In Quality Check.", status = 0, type = "info" }, JsonRequestBehavior.AllowGet);
                        salary.Status = constant.Status.InProgress;
                        salary.StatusDate = DateTime.Now;
                    }
                    else if (verificationType == constant.VerificationType.Tenant)
                    {
                        var tenant = inquiry.TenantVerifications.FirstOrDefault(x => x.TenantVerificationId == id && x.Status != constant.Status.QualityCheck && x.Status != constant.Status.Completed);
                        if (tenant == null) return Json(new { message = "Completed or In Quality Check.", status = 0, type = "info" }, JsonRequestBehavior.AllowGet);
                        tenant.Status = constant.Status.InProgress;
                        tenant.StatusDate = DateTime.Now;
                    }


                    var status = db.Statuses.Create();
                    status.StatusDate = DateTime.Now;
                    status.StatusMessage = constant.Status.InProgress;
                    status.Descriptions = $"Inquiry:'{inquiry.InquiryId}' assigned to FIO:{assignedUsers.Remove(assignedUsers.Length - 1)}";
                    status.PersonType = personType;
                    status.VerificationType = verificationType;
                    //new field verificatonId
                    status.VerificationId = id.HasValue ? id.Value : 0;
                    status.UserId = User.Identity.GetUserId();
                    inquiry.Statuses.Add(status);
                    db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    return Json(new { message = "Not found  or completed or In Quality Check.", status = 0, type = "info" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { message = $"Inquiry:'{inquiry.InquiryId}' {verificationType} {personType} assigned to FIO:{assignedUsers.Remove(assignedUsers.Length - 1)} ", status = 1, type = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { message = "error" + ex.Message, status = 0, tyep = "error" }, JsonRequestBehavior.AllowGet);
            }


        }//end method.



        [AllowAnonymous, HttpGet]
        public ActionResult GetInquiry(string userId,/* long[] inquiryIds,*/ long[] residenceIds, long[] workOfficeIds, long[] bankStatementIds, long[] salarySlipIds, long[] tenantIds)
        {
            if (string.IsNullOrEmpty(userId))
                return Json(new { message = "UserId cannot be empty or null.", status = 0 }, JsonRequestBehavior.AllowGet);

            // inquiryIds = inquiryIds ?? new long[] { 0 };
            residenceIds = residenceIds ?? new long[] { 0 };
            workOfficeIds = workOfficeIds ?? new long[] { 0 };
            bankStatementIds = bankStatementIds ?? new long[] { 0 };
            salarySlipIds = salarySlipIds ?? new long[] { 0 };
            tenantIds = tenantIds ?? new long[] { 0 };

            //Inquiry inquiry = OracleDbContext.GetByInquiryNo(inquiryNo, cnic);
            var fioUser = db.Users.Find(userId);


            if (fioUser == null)
                return Json(new { message = "No F.I.O User found for this UserId.", status = 0 },
                    JsonRequestBehavior.AllowGet);

            //this condition will not work if assign another part of already assigned inquiry 


            var dbInquiries = db.Inquiries.Where(x =>
            (x.Status == constant.Status.InProgress || x.Status == constant.Status.PartialComplete || x.Status == constant.Status.Edited) &&
            x.InquiryApplicationUsers.Any(s => s.UserId == userId)
            //&& !inquiryIds.Contains(x.InquiryId)
            ).ToList();


            // need to modify for remove specific inquiry this is generic for
            //  var completedIds = db.Inquiries.Where(x => inquiryIds.Contains(x.InquiryId) && x.Status == constant.Status.Completed).Select(x => x.InquiryId).ToList();

            residenceIds = db.ResidenceVerifications.Where(x => (x.Status == constant.Status.Completed || x.Status == constant.Status.QualityCheck) && residenceIds.Contains(x.ResidenceVerificationId)).Select(x => x.ResidenceVerificationId).ToArray();


            workOfficeIds = db.WorkOfficeVerifications.Where(x => (x.Status == constant.Status.Completed || x.Status == constant.Status.QualityCheck) && workOfficeIds.Contains(x.WorkOfficeVerificationId)).Select(x => x.WorkOfficeVerificationId).ToArray();
            tenantIds = db.TenantVerifications.Where(x => (x.Status == constant.Status.Completed || x.Status == constant.Status.QualityCheck) && tenantIds.Contains(x.TenantVerificationId)).Select(x => x.TenantVerificationId).ToArray();
            salarySlipIds = db.SalarySlipVerifications.Where(x => (x.Status == constant.Status.Completed || x.Status == constant.Status.QualityCheck) && salarySlipIds.Contains(x.SalarySlipVerificationId)).Select(x => x.SalarySlipVerificationId).ToArray();
            bankStatementIds = db.BankStatementVerifications.Where(x => (x.Status == constant.Status.Completed || x.Status == constant.Status.QualityCheck) && bankStatementIds.Contains(x.BankStatementVerificationId)).Select(x => x.BankStatementVerificationId).ToArray();




            //need to modify for remove 
            //  var notInUserInquiryIds = fioUser.InquiryApplicationUsers.Where(x => !inquiryIds.Contains(x.InquiryId)).Where(s => inquiryIds.Contains(s.InquiryId)).Select(x => x.InquiryId).ToList();


            //completedIds.AddRange(notInUserInquiryIds);

            if (dbInquiries.Count < 1)
                return Json(new { message = "No new Inquiry found. ", status = 0,/* completedIds = completedIds, */salarySlipIds = salarySlipIds, residenceIds = residenceIds, workOfficeIds = workOfficeIds, bankStatementIds = bankStatementIds, tenantIds = tenantIds }, JsonRequestBehavior.AllowGet);

            var getVm = new InquiryGetVM();
            var model = getVm.GetAll(dbInquiries, userId, db);
            return Json(new { message = "success", status = 1, data = model,/* completedIds = completedIds,*/ salarySlipIds = salarySlipIds, residenceIds = residenceIds, workOfficeIds = workOfficeIds, bankStatementIds = bankStatementIds, tenantIds = tenantIds }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous, HttpPost]
        //public async Task<ActionResult> PostInquiry(Inquiry model, string userId)
        public async Task<ActionResult> PostInquiry(string json, string userId)
        {
            var trans = db.Database.BeginTransaction();

                Inquiry model = new Inquiry();
                try
                {
                JObject data = JObject.Parse(json);
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<Inquiry>(data.ToString());
                //return Json(new { status = 0, message = "No Data found.", messageType = "Error" });
                
                Inquiry inquiry = db.Inquiries.FirstOrDefault(x => x.InquiryId == model.InquiryId);

                if (inquiry == null || inquiry.Status == constant.Status.Completed || inquiry.Status == constant.Status.QualityCheck)
                    return Json(new { status = 2, message = "Not found or completed. Please delete whole family of this inquiry.", messageType = "Info" });

                List<ResidenceVerification> residenceList = new List<ResidenceVerification>();
                List<WorkOfficeVerification> workOfficeList = new List<WorkOfficeVerification>();
                List<TenantVerification> tenantList = new List<TenantVerification>();
                List<BankStatementVerification> bankStatementList = new List<BankStatementVerification>();
                List<SalarySlipVerification> salarySlipList = new List<SalarySlipVerification>();
                List<InquiryImage> inquiryImages = new List<InquiryImage>();

                List<long> residenceIds = new List<long>();
                List<long> workOfficeIds = new List<long>();
                List<long> bankStatementIds = new List<long>();
                List<long> salarySlipIds = new List<long>();
                List<long> tenantIds = new List<long>();

                var deleteApplicationUsersFios = inquiry.InquiryApplicationUsers.Where(x => x.UserId == "asdfasdfasdf")
                    .Select(x => new { x.UserId, x.VerificationType, x.PersonType, x.VerificationId }).ToList();
                string loc = "";


                var residences = inquiry.ResidenceVerifications.Where(x => model.ResidenceVerifications.Any(s => s.ResidenceVerificationId == x.ResidenceVerificationId) && x.Status == constant.Status.InProgress).ToList();

                #region Assing Verifications

                foreach (var item in residences)
                {
                    var vO = model.ResidenceVerifications.FirstOrDefault(x => x.ResidenceVerificationId == item.ResidenceVerificationId);

                    item.GeneralComments = vO.GeneralComments;

                    //item.GpsLocation = vO.GpsLocation;
                    loc = GetLocationName(vO.GpsLocation);
                    item.GpsLocation = loc;

                    item.GpsURL = vO.GpsURL;
                    item.NearestLandMark = vO.NearestLandMark;
                    item.NeighbourCheck = vO.NeighbourCheck;
                    item.OutComeVerification = vO.OutComeVerification;
                    item.ResidenceAddress = vO.ResidenceAddress;
                    item.ResidenceDetail = vO.ResidenceDetail;

                    if (vO.ResidenceDetail.IsApplicantAvailable)
                    {
                        //item.ResidenceDetail.NameOfPersonToMet = inquiry.AppName;
                        item.ResidenceDetail.NameOfPersonToMet = item.PersonName;
                    }
                    else
                    {
                        item.ResidenceDetail.NameOfPersonToMet = item.ResidenceDetail.NameOfPersonToMet;
                    }

                    item.ResidenceProfile = vO.ResidenceProfile;


                    item.Status = constant.Status.QualityCheck;
                    item.StatusDate = vO.StatusDate;
                    item.VerifiedBy = userId;

                    var status = db.Statuses.Create();
                    status.StatusDate = vO.StatusDate;
                    status.StatusMessage = constant.Status.QualityCheck;
                    status.VerificationType = constant.VerificationType.Residence;
                    status.PersonType = vO.PersonType;
                    status.UserId = userId;
                    //new field..
                    status.VerificationId = vO.ResidenceVerificationId;
                    status.Descriptions = "User posted Inquiry for quality check.";

                    inquiry.Statuses.Add(status);

                    deleteApplicationUsersFios.Add(
                        new { UserId = userId, VerificationType = constant.VerificationType.Residence,
                            PersonType = vO.PersonType, VerificationId = vO.ResidenceVerificationId });

                    residenceList.Add(item);

                    #region Saving Video
                    if (item.PersonType.Equals(constant.PersonType.Applicant))
                    {
                        var VideoDirectory = GlobalHelper.SetImagePath(inquiry, item.PersonType, constant.VerificationType.Residence);
                        string videoFileName = VideoDirectory + DateTime.Now.ToString("yyyyMMddmm") + ".mp4";

                        if (Request.Files.Count > 0)
                        {
                            var httpPostedFile = Request.Files[0];
                            if (httpPostedFile != null)
                            {
                                var uploadFilesDir = VideoDirectory;
                                Directory.CreateDirectory(uploadFilesDir);
                                var fileSavePath = Path.Combine(uploadFilesDir, videoFileName);
                                httpPostedFile.SaveAs(fileSavePath);

                                var saveVideo = db.InquiryImages.Create();
                                saveVideo.ImagePath = fileSavePath;
                                saveVideo.VerificationId = item.ResidenceVerificationId;
                                saveVideo.VerificationType = constant.VerificationType.Residence;
                                saveVideo.PersonType = item.PersonType;
                                saveVideo.InquiryId = item.InquiryId;
                                db.InquiryImages.Add(saveVideo);

                            }
                        }
                    }
                    #endregion
                }
                var workOffices = inquiry.WorkOfficeVerifications.Where(x => model.WorkOfficeVerifications.Any(s => s.WorkOfficeVerificationId == x.WorkOfficeVerificationId) && x.Status == constant.Status.InProgress).ToList();


                foreach (var item in workOffices)
                {
                    var vO = model.WorkOfficeVerifications.FirstOrDefault(x => x.WorkOfficeVerificationId == item.WorkOfficeVerificationId);

                    item.BusinessWorkOfficeDetail = vO.BusinessWorkOfficeDetail;
                    item.GeneralComments = vO.GeneralComments;

                    loc = GetLocationName(vO.GpsLocation);
                    item.GpsLocation = loc;

                    item.GpsURL = vO.GpsURL;
                    item.MarketeCheck = vO.MarketeCheck;
                    item.NearestLandMark = vO.NearestLandMark;
                    item.OfficeAddressDetail = vO.OfficeAddressDetail;
                    item.OfficeHRDetail = vO.OfficeHRDetail;
                    item.OutComeVerification = vO.OutComeVerification;
                    item.Status = constant.Status.QualityCheck;
                    item.StatusDate = vO.StatusDate;
                    item.VerifiedBy = userId;


                    var status = db.Statuses.Create();
                    status.StatusDate = vO.StatusDate;
                    status.StatusMessage = constant.Status.QualityCheck;
                    status.VerificationType = constant.VerificationType.WorkOffice;
                    status.PersonType = vO.PersonType;
                    status.UserId = userId;
                    status.Descriptions = "User posted Inquiry for quality check.";
                    //new field..
                    status.VerificationId = vO.WorkOfficeVerificationId;
                    inquiry.Statuses.Add(status);
                    workOfficeList.Add(item);

                    deleteApplicationUsersFios.Add(new { UserId = userId, VerificationType = constant.VerificationType.WorkOffice,
                        PersonType = vO.PersonType, VerificationId = vO.WorkOfficeVerificationId });

                }

                var bs = inquiry.BankStatementVerifications.Where(x => model.BankStatementVerifications.Any(s => s.BankStatementVerificationId == x.BankStatementVerificationId) && x.Status == constant.Status.InProgress).ToList();


                foreach (var item in bs)
                {
                    var vO = model.BankStatementVerifications.FirstOrDefault(x => x.BankStatementVerificationId == item.BankStatementVerificationId);

                    item.OutComeVerification = vO.OutComeVerification;

                    item.GeneralComments = vO.GeneralComments;
                    loc = GetLocationName(vO.GpsLocation);
                    item.GpsLocation = loc;

                    item.GpsURL = vO.GpsURL;

                    item.Status = constant.Status.QualityCheck;
                    item.StatusDate = vO.StatusDate;
                    item.VerifiedBy = userId;

                    var status = db.Statuses.Create();
                    status.StatusDate = vO.StatusDate;
                    status.StatusMessage = constant.Status.QualityCheck;
                    status.VerificationType = constant.VerificationType.BankStatement;
                    status.PersonType = vO.PersonType;
                    status.UserId = userId;
                    status.Descriptions = "User posted Inquiry for quality check.";
                    status.VerificationId = vO.BankStatementVerificationId;
                    inquiry.Statuses.Add(status);
                    bankStatementList.Add(item);

                    deleteApplicationUsersFios.Add(new { UserId = userId, VerificationType = constant.VerificationType.BankStatement,
                        PersonType = vO.PersonType, 
                        VerificationId = vO.BankStatementVerificationId
                    });

                }

                var ss = inquiry.SalarySlipVerifications.Where(x => model.SalarySlipVerifications.Any(s => s.SalarySlipVerificationId == x.SalarySlipVerificationId) && x.Status == constant.Status.InProgress).ToList();


                foreach (var item in ss)
                {
                    var vO = model.SalarySlipVerifications.FirstOrDefault(x => x.SalarySlipVerificationId == item.SalarySlipVerificationId);

                    item.GeneralComments = vO.GeneralComments;

                    loc = GetLocationName(vO.GpsLocation);
                    item.GpsLocation = loc;

                    item.GpsURL = vO.GpsURL;
                    item.NearestLandMark = vO.NearestLandMark;
                    item.OutComeVerification = vO.OutComeVerification;

                    item.Status = constant.Status.QualityCheck;
                    item.StatusDate = vO.StatusDate;
                    item.VerifiedBy = userId;

                    var status = db.Statuses.Create();
                    status.StatusDate = vO.StatusDate;
                    status.StatusMessage = constant.Status.QualityCheck;
                    status.VerificationType = constant.VerificationType.SalarySlip;
                    status.PersonType = vO.PersonType;
                    status.UserId = userId;
                    status.Descriptions = "User posted Inquiry for quality check.";
                    //new field..
                    status.VerificationId = vO.SalarySlipVerificationId;
                    inquiry.Statuses.Add(status);
                    salarySlipList.Add(item);

                    deleteApplicationUsersFios.Add(new { UserId = userId, VerificationType = constant.VerificationType.SalarySlip,
                        PersonType = vO.PersonType,
                        VerificationId = vO.SalarySlipVerificationId
                    });


                }

                var tv = inquiry.TenantVerifications.Where(x => model.TenantVerifications.Any(s => s.TenantVerificationId == x.TenantVerificationId) && x.Status == constant.Status.InProgress).ToList();


                foreach (var item in tv)
                {
                    var vO = model.TenantVerifications.FirstOrDefault(x => x.TenantVerificationId == item.TenantVerificationId);

                    item.TenancyPeriod = vO.TenancyPeriod;
                    item.TenantCNIC = vO.TenantCNIC;
                    item.TenantName = vO.TenantName;
                    item.TenantRent = vO.TenantRent;
                    item.TenantContactNo = vO.TenantContactNo;
                    item.GeneralComments = vO.GeneralComments;

                    loc = GetLocationName(vO.GpsLocation);
                    item.GpsLocation = loc;

                    item.GpsURL = vO.GpsURL;
                    item.NearestLandMark = vO.NearestLandMark;
                    item.OutComeVerification = vO.OutComeVerification;

                    item.Status = constant.Status.QualityCheck;
                    item.StatusDate = vO.StatusDate;
                    item.VerifiedBy = userId;

                    var status = db.Statuses.Create();
                    status.StatusDate = vO.StatusDate;
                    status.StatusMessage = constant.Status.QualityCheck;
                    status.VerificationType = constant.VerificationType.Tenant;
                    status.PersonType = vO.PersonType;
                    status.UserId = userId;
                    status.Descriptions = "User posted Inquiry for quality check.";
                    //new field..
                    status.VerificationId = vO.TenantVerificationId;
                    inquiry.Statuses.Add(status);
                    tenantList.Add(item);

                    deleteApplicationUsersFios.Add(new { UserId = userId, VerificationType = constant.VerificationType.Tenant,
                        PersonType = vO.PersonType,
                        VerificationId = vO.TenantVerificationId
                    });

                }
                #endregion

                //check all status and mark qc for 
                //and mark status for qc and done this inquiry


                if (inquiry.ResidenceVerifications.Any(x => x.Status != constant.Status.Completed && x.Status != constant.Status.QualityCheck)
                    ||
                    inquiry.WorkOfficeVerifications.Any(x => x.Status != constant.Status.Completed && x.Status != constant.Status.QualityCheck)
                    ||
                    inquiry.BankStatementVerifications.Any(x => x.Status != constant.Status.Completed && x.Status != constant.Status.QualityCheck)
                    ||
                    inquiry.SalarySlipVerifications.Any(x => x.Status != constant.Status.Completed && x.Status != constant.Status.QualityCheck)
                    ||
                    inquiry.TenantVerifications.Any(x => x.Status != constant.Status.Completed && x.Status != constant.Status.QualityCheck)
                    )
                {
                    inquiry.Status = constant.Status.PartialComplete;
                    inquiry.StatusDate = DateTime.Now;


                    var status = db.Statuses.Create();
                    status.StatusDate = DateTime.Now;
                    status.StatusMessage = constant.Status.PartialComplete;
                    status.Descriptions = "A part of inquiry has been completed.";
                    status.UserId = userId;
                    inquiry.Statuses.Add(status);
                }
                else
                {
                    inquiry.Status = constant.Status.QualityCheck;
                    inquiry.StatusDate = DateTime.Now;

                    var status = db.Statuses.Create();
                    status.StatusDate = DateTime.Now;
                    status.StatusMessage = constant.Status.QualityCheck;
                    status.Descriptions = "Physical verification completed. Now is moving for Quality Check";
                    status.UserId = userId;

                    inquiry.Statuses.Add(status);
                }

                var rootPath = db.ApplicationSettings.FirstOrDefault(x => x.Key == constant.ApplicationSettings.RootPath);
                int counter = 0;
                foreach (var item in model.Images)
                {
                    if (item.Image == null)
                    {
                        continue;
                    }
                    
                    string subDirectory = GlobalHelper.SetImagePath(inquiry, item.PersonType, item.VerificationType);
                    subDirectory = rootPath.Value + subDirectory;


                    if (!Directory.Exists(subDirectory))
                    {
                        Directory.CreateDirectory(subDirectory);
                    }
                    string fileName = subDirectory + DateTime.Now.ToString("yyyyMMdd_hhmmss_fffffff") +"_"+ counter+ ".jpg";

                    try
                    {
                        using (var memStream = new MemoryStream(item.Image))
                        {
                            using (Image image = Image.FromStream(memStream))
                            {
                                image.Save(fileName, ImageFormat.Jpeg);  // Or Png
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        using (ApplicationDbContext dbs = new ApplicationDbContext())
                        {
                            ApplicationSettings set = new ApplicationSettings();
                            set.Key = model.InquiryId.ToString() + "FileName:'"+fileName+"' " + "First exception."+  GlobalHelper.GetLastInnerMessage(exx);
                            set.Value = Convert.ToBase64String(item.Image);
                            dbs.ApplicationSettings.Add(set);
                            dbs.SaveChanges();
                        }
                     
                        try
                        {
                            using (var memStream = new MemoryStream(item.Image))
                            {
                                using (Bitmap f = new Bitmap(memStream))
                                {
                                    f.Save(fileName, ImageFormat.Jpeg);
                                }
                            }
                        }
                        catch (Exception exs)
                        {
                            using (ApplicationDbContext dbs = new ApplicationDbContext())
                            {
                                ApplicationSettings set = new ApplicationSettings();
                                set.Key = model.InquiryId.ToString() + "FileName:'" + fileName + "' " + GlobalHelper.GetLastInnerMessage(exs);
                                set.Value = Convert.ToBase64String(item.Image);
                                dbs.ApplicationSettings.Add(set);
                                dbs.SaveChanges();
                            }
                            throw exs;
                        }

                    }

                    item.ImagePath = fileName;
           
                    inquiry.Images.Add(item);
                    //db.InquiryImages.Add(item);
                    inquiryImages.Add(item);
                    counter++;
                }

              
                

                db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    //SaveBQC(ref inquiry, residenceList, workOfficeList, salarySlipList, bankStatementList, tenantList, inquiryImages);

                    //new field verificationId..
                    //here remove the fios from the part of that which move to quality check by another fio.
                    var forRemoveUser = inquiry.InquiryApplicationUsers.Where(x => deleteApplicationUsersFios.Any(f => f.UserId != x.UserId 
                    && f.PersonType == x.PersonType && f.VerificationType == x.VerificationType && f.VerificationId == x.VerificationId)).ToList();
                    foreach (var item in forRemoveUser)
                    {
                        inquiry.InquiryApplicationUsers.Remove(item);
                    }

                    db.Entry(inquiry).State = System.Data.Entity.EntityState.Modified;

                    if (db.SaveChanges() <= 0)
                    {
                        trans.Rollback();
                        return Json(new { status = 0, message = "No History save or update.", messageType = "Undefined." }, JsonRequestBehavior.AllowGet);
                    }

                    trans.Commit();
                    var ProductName = db.Products.FirstOrDefault(x => x.ProductId == model.ProductId);
                    //if (ProductName != null && ProductName.Sms)
                    //{
                    //    #region SMS
                    //    ZongSMS sms = new ZongSMS();
                    //    string message_ = $@"آپ کی ویریفکیشن کاعمل مکمل ہو گیا ہے لہذا آپ سے گزارش ہے کے ہمارےنمائندے سےاگر کسی قسم کی شکایت ہے تو براہ مہربانی اگلے 12 گھنٹوں کے اندر دیئے گے نمبر پرمطلع کردیں- 021-35246559 شکریہ";
                    //    await sms.SingleSMS(model.AppContact, message_, model.AppCNIC, constant.Status.QualityCheck);
                    //    #endregion
                    //}
                    return Json(new
                    {

                        status = 1,
                        message = "Record updated successfully'" + inquiry.InquiryId + "'",
                        messageType = "Success",
                        residenceIds = inquiry.ResidenceVerifications.Where(x => x.Status == constant.Status.Completed || x.Status == constant.Status.QualityCheck).Select(x => x.ResidenceVerificationId).ToArray(),
                        bankStatementIds = inquiry.BankStatementVerifications.Where(x => x.Status == constant.Status.Completed || x.Status == constant.Status.QualityCheck).Select(x => x.BankStatementVerificationId).ToArray(),
                        tenantIds = inquiry.TenantVerifications.Where(x => x.Status == constant.Status.Completed || x.Status == constant.Status.QualityCheck).Select(x => x.TenantVerificationId).ToArray(),
                        workOfficeIds = inquiry.WorkOfficeVerifications.Where(x => x.Status == constant.Status.Completed || x.Status == constant.Status.QualityCheck).Select(x => x.WorkOfficeVerificationId).ToArray(),
                        salarySlipIds = inquiry.SalarySlipVerifications.Where(x => x.Status == constant.Status.Completed || x.Status == constant.Status.QualityCheck).Select(x => x.SalarySlipVerificationId).ToArray()
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    trans.Rollback();
                    return Json(new { status = 0, message = "No record save or update. Something went wrong at this time unspecified error.", messageType = "Undefined." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();

                return Json(new { status = 0, message = GlobalHelper.GetLastInnerMessage(ex), messageType = "ERRORS" }, JsonRequestBehavior.AllowGet);

            }


        }


     



     


       


        public string GetLocationName(string location)
        {
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(@"http://maps.googleapis.com/maps/api/geocode/json?latlng=" + location + "&sensor=true");
                myReq.ContentType = "application/json";


                // here's how to set response content type:
                Response.ContentType = "application/json"; // that's all

                var response = (HttpWebResponse)myReq.GetResponse();
                string text;

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    text = sr.ReadToEnd();
                    //var myJsonString = "{report: {Id: \"aaakkj98898983\"}}";
                    //var jo = JObject.Parse(myJsonString);
                    //var id = jo["report"]["Id"].ToString();

                    var jos = JObject.Parse(text);
                    if (jos["results"][1]["formatted_address"] != null)
                    {
                        text = jos["results"][1]["formatted_address"].ToString();
                    }
                    else
                    {
                        text = location;
                    }

                    return text;
                }
            }
            catch (Exception)
            {
                return location;
            }
        }










        // FOR EVS   DOWNLOAD APK
        [AllowAnonymous, HttpGet]
        public ActionResult DownloadFile(string userId, double appVersion)
        {
            if (appVersion > 1.4)
            {
                return Json(new
                {
                    URL = "null",
                    appVersion = appVersion,
                    message = "No Updates"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    URL = "https://drive.google.com/file/d/1ZxilgO63-tGSJmcnF286QEUrrqOajMtn/view?usp=sharing",

                    appVersion = "1.5",
                    message = "New Update"
                }, JsonRequestBehavior.AllowGet);
            }
        }



    }//end class

}//end namespace