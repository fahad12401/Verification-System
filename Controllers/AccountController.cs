using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VerificationSystem.Models;
using VerificationSystem.DB;
using System.Data.Entity;
using VerificationSystem.Extensions;
using System.Web.Security;
using Newtonsoft.Json;

namespace VerificationSystem.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _dbContext;
      
        public ApplicationDbContext DbContext
        {
            get { return _dbContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
            private set { _dbContext = value; }
        }

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //So that the user can be referred back to where they were when they click logon
            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(returnUrl);

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login( LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }




            //var User = DbContext.Users.FirstOrDefault(x => x.UserName == model.UserName && x.Code == model.Password);
            var user = UserManager.FindByName(model.UserName);
            if (user != null)
            {
                await SignInManager.SignInAsync(user, true,true);
                /*string userdata = JsonConvert.SerializeObject(User);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(30), false, userdata);
                string encrticket = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrticket);
                Response.Cookies.Add(cookie);*/
                //FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            //This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> LoginTab(LoginViewModel model)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return View();
                //}

                var user = await UserManager.FindAsync(model.UserName, model.Password);

                if (user == null)
                {
                    return Json(new { message = "username or password is incorrect" }, JsonRequestBehavior.AllowGet);
                }
               

                //  string isoJson = JsonConvert.SerializeObject(user);

                return Json(new { message = "Login successfully", user = new { Id = user.Id, UserName = user.UserName, PasswordHash = user.PasswordHash, BranchId = user.BranchId, FirstName = user.FirstName, LastName = user.LastName, code = model.Password} }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { message = "error", error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }





        [Authorize(Roles = constant.Roles.SuperAdmin + "," + constant.Roles.Head)]
        public ActionResult Index()
        {
            if (User.IsInRole(constant.Roles.Head))
            {

                var superAdminNHead = DbContext.Roles.Where(x => x.Name == constant.Roles.SuperAdmin || x.Name == constant.Roles.Head).Select(x => x.Id).ToList();
                var users = UserManager.Users.Where(x => !superAdminNHead.Contains(x.Roles.FirstOrDefault().RoleId))
                    .Select(x => new AccountIndexVM()
                    {
                        CompanyName = x.Company == null ? null : x.Company.Name,
                        CustomerBranchName = x.CustomerBranch == null ? null : x.CustomerBranch.Name,
                        DisableDate = x.DisableDate,
                        FirstName = x.FirstName,
                        BranchName = x.Branch.Name,
                        LastName = x.LastName,
                        UserId = x.Id,
                        UserName = x.UserName
                    }
                    )
                    .ToList();
                return View(users);

            }
            else
            {
                using (var db = new ApplicationDbContext())
                {

                    var superAdminNHead = DbContext.Roles.Where(x => x.Name == constant.Roles.SuperAdmin).Select(x => x.Id).ToList();

                    var users = UserManager.Users.Where(x => !superAdminNHead.Contains(x.Roles.FirstOrDefault().RoleId))
                        .ToList().Select(x => new AccountIndexVM()
                        {
                            CompanyName = x.Company == null ? null : x.Company.Name,
                            CustomerBranchName = x.CustomerBranch == null ? null : x.CustomerBranch.Name,
                            DisableDate = x.DisableDate,

                            Roles = GetRoles(x.Id),

                            FirstName = x.FirstName,
                            BranchName = x.Branch.Name,
                            LastName = x.LastName,
                            UserId = x.Id,
                            UserName = x.UserName
                        }
                        ).ToList();
                    return View(users);
                }

            }

        }


        public string GetRoles(string userid)
        {
            return string.Join("|", UserManager.GetRoles(userid).ToArray());
        }


        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = constant.Roles.SuperAdmin + "," + constant.Roles.Head)]
        public ActionResult Register()
        {
            if (User.IsInRole(constant.Roles.SuperAdmin))
            {
                
                ViewBag.Roles = new MultiSelectList(DbContext.Roles.Where(x => x.Name != constant.Roles.SuperAdmin && x.Name != constant.Roles.Head), "Name", "Name");
            }
            else
            {
             
                ViewBag.Roles = new MultiSelectList(DbContext.Roles.Where(x => x.Name != constant.Roles.SuperAdmin), "Name", "Name");
            }

            ViewBag.Branches = new SelectList(DbContext.Branches.Where(x => x.IsActive == true), "BranchId", "Name");
          

            ViewBag.Companies = new SelectList(DbContext.Companies.Where(x => x.DisableDate == null), "CompanyId", "Name");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = constant.Roles.SuperAdmin + "," + constant.Roles.Head)]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {

            
            ModelState.Remove("CustomerBranchId");
            ModelState.Remove("CompanyId");
            ModelState.Remove("ParentCode");
            if (ModelState.IsValid)
            {

                ApplicationUser user;


                user = new ApplicationUser { Code = model.Password, Address = model.Address, FirstName = model.FirstName, LastName = model.LastName, RecordAt = DateTime.Now, RecordBy = User.Identity.GetUserId(), UserName = model.UserName, Email = model.Email, BranchId = model.BranchId, CompanyId = model.CompanyId, ParentCode = model.ParentCode };   //// do nothing here completed.

               
                user.CompanyId = null;
                user.CustomerBranchId = null;
                user.ParentCode = null;
                //setting disable date by cheking isActive 
                if (!model.IsActive)
                    user.DisableDate = DateTime.Now;
                if (model.CustomerBranchId.HasValue)
                    user.CustomerBranchId = model.CustomerBranchId;


                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // result = await UserManager.AddToRoleAsync(user.Id, model.Role);
                    result = await UserManager.AddToRolesAsync(user.Id, model.Roles);

                    if (result.Succeeded)
                    {



                        this.AddNotification("User '" + user.UserName + "' has been created successfully", NotificationType.Success);
                    }
                   

                    return RedirectToAction("Index", "Account");
                }
                AddErrors(result);
            }

            ViewBag.Branches = new SelectList(DbContext.Branches.Where(x => x.IsActive == true), "BranchId", "Name");

            // ViewBag.CustomerBranches = new SelectList(DbContext.CustomerBranches.Where(x => x.DisableDate == null), "CustomerBranchId", "Name");



            ViewBag.Companies = new SelectList(DbContext.Companies.Where(x => x.DisableDate == null), "CompanyId", "Name", model.CompanyId);

            if (User.IsInRole(constant.Roles.Head))
            {
                ViewBag.Roles = new MultiSelectList(DbContext.Roles.Where(x => x.Name != constant.Roles.SuperAdmin && x.Name != constant.Roles.Head), "Name", "Name", model.Roles);
            }
            else
            {
                ViewBag.Roles = new MultiSelectList(DbContext.Roles.Where(x => x.Name != constant.Roles.SuperAdmin), "Name", "Name", model.Roles);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }



        [HttpGet, Authorize(Roles = constant.Roles.SuperAdmin)]
        public ActionResult Edit(string id)
        {

            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            UserEditViewModel model = new UserEditViewModel()
            {
                CustomerBranchId = user.CustomerBranchId,
                Email = user.Email,
                FirstName = user.FirstName,
                id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
                Code = user.Code,
                BranchId = user.BranchId.Value,
                Address = user.Address,
                CompanyId = user.CompanyId,
                ParentCode = user.ParentCode

            };

            //  model.Role = user.Roles.First().RoleId;
            //  model.Role = DbContext.Roles.FirstOrDefault(x => x.Id == model.Role).Name;
            model.Roles = UserManager.GetRoles(user.Id).ToArray();


            if (user.DisableDate == null)
                model.IsActive = true;


            ViewBag.Branches = new SelectList(DbContext.Branches.Where(x => x.IsActive == true), "BranchId", "Name", model.BranchId);

            ViewBag.Companies = new SelectList(DbContext.Companies.Where(x => x.DisableDate == null), "CompanyId", "Name", model.CompanyId);

            if (User.IsInRole(constant.Roles.Head))
                ViewBag.Roless = new MultiSelectList(DbContext.Roles.Where(x => x.Name != constant.Roles.SuperAdmin && x.Name != constant.Roles.Head), "Name", "Name", model.Roles);
            else
                ViewBag.Roles = new MultiSelectList(DbContext.Roles.Where(x => x.Name != constant.Roles.SuperAdmin), "Name", "Name", model.Roles);


            return View(model);
        }


        [HttpPost, Authorize(Roles = constant.Roles.SuperAdmin), ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel model)
        {
            ModelState.Remove("CompanyId");
            ModelState.Remove("CustomerBranchId");
            ModelState.Remove("ParentCode");

            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(model.id);

                if (user != null)
                {
                    user.CompanyId = null;
                    user.CustomerBranchId = null;
                    user.ParentCode = null;
                   
                    user.BranchId = model.BranchId;

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Address = model.Address;
                    user.Email = model.Email;
                    user.CompanyId = model.CompanyId;
                    user.ParentCode = model.ParentCode;
                    user.UpdateAt = DateTime.Now;
                    user.UpdateBy = User.Identity.GetUserId();

                    if (model.IsActive)
                        user.DisableDate = null;
                    else
                        user.DisableDate = DateTime.Now;


                    var result = UserManager.Update(user);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.Password))
                        {
                            result = UserManager.ChangePassword(user.Id, user.Code, model.ConfirmPassword);

                            if (result.Succeeded)
                            {
                                user.Code = model.Password;
                                result = UserManager.Update(user);
                                this.AddNotification("Application user has been updated successfully.", NotificationType.Success);

                            }
                            else
                            {
                                //error when password can't be changed. for some reason then do some stuff here
                                this.AddNotification("Password can't be change right now try again later.", NotificationType.Error);
                            }
                        }
                        else
                        {
                            this.AddNotification("Application user has been updated successfully.", NotificationType.Success);
                        }

                        //if (!UserManager.IsInRole(user.Id, model.Role))
                        //{
                        //    var roleName = UserManager.GetRoles(user.Id).First();
                        //    UserManager.RemoveFromRole(user.Id, roleName);
                        //    UserManager.Update(user);
                        //    result = UserManager.AddToRole(user.Id, model.Role);
                        //    if (!result.Succeeded)
                        //    {
                        //        this.AddNotification(result.Errors.First(), NotificationType.Info);
                        //    }
                        //}

                        var roleNames = UserManager.GetRoles(user.Id).ToArray();
                        UserManager.RemoveFromRoles(user.Id, roleNames);
                        UserManager.Update(user);
                        result = UserManager.AddToRoles(user.Id, model.Roles);
                        if (!result.Succeeded)
                        {
                            this.AddNotification(result.Errors.First(), NotificationType.Info);
                        }


                        return RedirectToAction("Index", "Account");
                    }

                }
            }

            this.AddNotification("There is an error or some field is missing.", NotificationType.Info);


            ViewBag.Branches = new SelectList(DbContext.Branches.Where(x => x.IsActive == true), "BranchId", "Name", model.BranchId);

            ViewBag.Companies = new SelectList(DbContext.Companies.Where(x => x.DisableDate == null), "CompanyId", "Name", model.CompanyId);

            if (User.IsInRole(constant.Roles.Head))
                ViewBag.Roles = new MultiSelectList(DbContext.Roles.Where(x => x.Name != constant.Roles.SuperAdmin && x.Name != constant.Roles.Head), "Name", "Name", model.Roles);
            else
                ViewBag.Roles = new MultiSelectList(DbContext.Roles.Where(x => x.Name != constant.Roles.SuperAdmin), "Name", "Name", model.Roles);
            return View(model);
        }



        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            ApplicationUser model = UserManager.FindById(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            foreach (var item in model.Roles)
            {
                ViewBag.RoleName = GetRoles(item.UserId);
            }
            return View(model);
        }




        public ActionResult DoesUserNameExist(string UserName, string OldUserName)
        {
            if (OldUserName == UserName)
                return Json(true, JsonRequestBehavior.AllowGet);

            var user = UserManager.FindByName(UserName);
            return Json(user == null);
        }
        public ActionResult DoesEmailExist(string email, string oldEmail)
        {
            if (email == oldEmail)
                return Json(true, JsonRequestBehavior.AllowGet);

            var user = UserManager.FindByEmail(email);

            return Json(user == null);
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Dashboard");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                returnUrl = Server.UrlDecode(returnUrl);
                return Redirect(returnUrl);
            }
            //   return RedirectToAction("Index", "Inquiry");

            return RedirectToAction("Index", "Dashboard");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}