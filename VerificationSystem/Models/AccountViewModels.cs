using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace VerificationSystem.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required, Display(Name = "Company")]
        public long CompanyId { get; set; }


        [Required, Display(Name = "Customer Branch")]
        public long? CustomerBranchId { get; set; }

        [Required, Display(Name = "Branch")]
        public long BranchId { get; set; }

        [ Display(Name = "Customer Head Branch")]
        public long? ParentCode { get; set; }


        [Required, Display(Name = "Email", Description = "User email for contacting"), EmailAddress]
        [System.Web.Mvc.Remote("DoesEmailExist", "Account", HttpMethod = "POST", ErrorMessage = "Email already exists. Please enter a different email.")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be between {1} to {2} characters long.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be between {1} to {2} characters long.", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "UserName")]
        [System.Web.Mvc.Remote("DoesUserNameExist", "Account", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        public string UserName { get; set; }

        //[Required]
        //[Display(Name = "Role")]
        //public string Role { get; set; }


        [Required]
        [Display(Name = "Roles")]
        public string[] Roles { get; set; }


        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }



        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(Name = "Address"), DataType(DataType.MultilineText)]
        public string Address { get; set; }

    
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }


    public class AccountIndexVM
    {
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string CustomerBranchName { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime? DisableDate { get; set; }

        public string Roles { get; set; }



    }


    public class UserEditViewModel
    {

        public string id { get; set; }

        [Required, Display(Name = "Company")]
        public long? CompanyId { get; set; }


        [Required, Display(Name = "Branch")]
        public long BranchId { get; set; }


        [Required, Display(Name = "Customer Branch")]
        public long? CustomerBranchId { get; set; }

        [Display(Name = "Customer Head Branch")]
        public long? ParentCode { get; set; }

        [Required, Display(Name = "Email", Description = "User email for contacting"), EmailAddress]
        [System.Web.Mvc.Remote("DoesEmailExist", "Account", HttpMethod = "POST", ErrorMessage = "Email already exists. Please enter a different email.", AdditionalFields = "OldEmail")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be between {1} to {2} characters long.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be between {1} to {2} characters long.", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "UserName")]
        [System.Web.Mvc.Remote("DoesUserNameExist", "Account", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.", AdditionalFields = "OldUserName")]
        public string UserName { get; set; }

        //[Required]
        //[Display(Name = "Role")]
        //public string Role { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public string[] Roles { get; set; }



        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }



        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(Name = "Address"), DataType(DataType.MultilineText)]
        public string Address { get; set; }
        
        public string Code { get; internal set; }


    }

}
