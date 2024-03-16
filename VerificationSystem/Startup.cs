 using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Linq;
using VerificationSystem.DB;

[assembly: OwinStartupAttribute(typeof(VerificationSystem.Startup))]
namespace VerificationSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            SetupPreDatabaseOptions();
        }

        public void SetupPreDatabaseOptions()
        {

            var db = new ApplicationDbContext();
            if (!db.ApplicationSettings.Any(x => x.Key == constant.ApplicationSettings.CompanyCodeForTwoImagesOnReportPage))
            {
                var applicationSetting = db.ApplicationSettings.Create();
                applicationSetting.Key = constant.ApplicationSettings.CompanyCodeForTwoImagesOnReportPage;
                applicationSetting.Value = @"6";

                db.ApplicationSettings.Add(applicationSetting);
                db.SaveChanges();
            }

            if (!db.ApplicationSettings.Any(x => x.Key == constant.ApplicationSettings.AuditStartDate))
            {
                var applicationSetting = db.ApplicationSettings.Create();
                applicationSetting.Key = constant.ApplicationSettings.AuditStartDate;
                applicationSetting.Value = @"2018-05-01";

                db.ApplicationSettings.Add(applicationSetting);
                db.SaveChanges();

            }

            if (!db.ApplicationSettings.Any(x => x.Key == constant.ApplicationSettings.AuditPercentageRate))
            {
                var applicationSetting = db.ApplicationSettings.Create();
                applicationSetting.Key = constant.ApplicationSettings.AuditPercentageRate;
                applicationSetting.Value = @"10";

                db.ApplicationSettings.Add(applicationSetting);
                db.SaveChanges();

            }


            if (!db.ApplicationSettings.Any(x => x.Key == constant.ApplicationSettings.RootPath))
            {
                var applicationSetting = db.ApplicationSettings.Create();
                applicationSetting.Key = constant.ApplicationSettings.RootPath;
                applicationSetting.Value = @"E:\";

                db.ApplicationSettings.Add(applicationSetting);
                db.SaveChanges();
            }



            if (!db.Roles.Any(x => x.Name == constant.Roles.SuperAdmin))
            {
                db.Roles.Add(new IdentityRole(constant.Roles.SuperAdmin));
                db.SaveChanges();
            }

            if (!db.Roles.Any(x => x.Name == constant.Roles.Head))
            {
                db.Roles.Add(new IdentityRole(constant.Roles.Head));
                db.SaveChanges();
            }


            if (!db.Users.Any(u=> u.UserName == "superadmin"))
            {
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new ApplicationUserManager(userStore);

                var roleStore = new RoleStore<IdentityRole>(db);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var user = new ApplicationUser()
                {
                    UserName = "superadmin",
                    FirstName = "Application Admin",
                    LastName = "Fahad",
                    RecordAt = System.DateTime.Now,
                    DisableDate = null,
                    Email = "admin@admin.com",
                    Code = "superadmin"

                };
                userManager.Create(user, "superAdmin");
                userManager.AddToRole(user.Id, constant.Roles.SuperAdmin);
                db.SaveChanges();
            }

        }

    }
}
