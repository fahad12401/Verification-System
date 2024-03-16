using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace VerificationSystem.Extensions
{
    public static class UserHelper
    {
        public static List<DB.ApplicationUser> GetAllUsers(string roleName)
        {
            try
            {
                using (var db = new DB.ApplicationDbContext())
                {
                    var roleId = db.Roles.FirstOrDefault(x => x.Name == roleName).Id;
                    return db.Users.Include(x => x.Branch).Where(x => x.Roles.Any(s => s.RoleId == roleId)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetUserNameById(string id)
        {
            try
            {
                using (var db = new DB.ApplicationDbContext())
                {
                    var user = db.Users.FirstOrDefault(x => x.Id == id);
                    if (user == null)
                        return string.Empty;

                    return user.UserName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetNameById(string id)
        {
            try
            {
                using (var db = new DB.ApplicationDbContext())
                {
                    var user = db.Users.FirstOrDefault(x => x.Id == id);
                    if (user == null)
                        return string.Empty;

                    return $@"{user.FirstName} {user.LastName}";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetProductNameById(long id)
        {
            try
            {
                using (var db = new DB.ApplicationDbContext())
                {
                    var product = db.Products.FirstOrDefault(x => x.ProductId == id);
                    if (product == null)
                        return string.Empty;

                    return $@"{product.Name}";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetProductNameAndCompanyNameByProductId(long id)
        {
            try
            {
                using (var db = new DB.ApplicationDbContext())
                {
                    var product = db.Products.FirstOrDefault(x => x.ProductId == id);
                    if (product == null)
                        return string.Empty;

                    return $@"{product.Name} - {product.Company.Name}";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetCompanyNameByProductId(long id)
        {
            try
            {
                using (var db = new DB.ApplicationDbContext())
                {
                    var product = db.Products.FirstOrDefault(x => x.ProductId == id);
                    if (product == null)
                        return string.Empty;

                    return $@"{product.Company.Name}";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}