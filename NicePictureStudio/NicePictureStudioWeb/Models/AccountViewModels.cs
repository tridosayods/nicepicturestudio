using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NicePictureStudio.Models
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
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        public string Position { get; set; }

        [Display(Name = "Supervisor Name (If any)")]
        public string ManagerId { get; set; }

        [Display(Name = "Start Date of Working")]
        public DateTime StartDate { get; set; }

        public int Status { get; set; }

        [Display(Name = "Identification Number")]
        public int IdentificationNumber { get; set; }

        [Display(Name = "Mobile Phone Number")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Education { get; set; }
        public string Specialability { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ApplicationUser> SupervisorList { get; set; }
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

    public class EmployeePositionTemp
    {
        public string Position {get;set;}
        public int Id {get;set;}
    }

    public class EmployeePositionDictionary
    {
        public Dictionary<int, string> EmployeePositions = new Dictionary<int, string>();

        public IEnumerable<EmployeePositionTemp> generateList()
        {
            Dictionary<int,string> position = new Dictionary<int,string>();
            position.Add(1,"SaleManager");
            position.Add(2,"CameraMan");
            position.Add(3,"PhotoGraph");
            position.Add(4,"Sale");
            position.Add(5,"Media");
            position.Add(6,"Manager");

            List<EmployeePositionTemp> empList = new List<EmployeePositionTemp>();
            foreach (var item in position)
            {
                EmployeePositionTemp empPosition = new EmployeePositionTemp();
                empPosition.Id = item.Key;
                empPosition.Position = item.Value;
            }

            return empList as IEnumerable<EmployeePositionTemp>;
        }

        public IEnumerable<EmployeePositionTemp> generateThaiList()
        {
            Dictionary<int, string> position = new Dictionary<int, string>();
            position.Add(1, "ผู้บริหารการขาย");
            position.Add(2, "ช่างถ่ายภาพวีดีโอ");
            position.Add(3, "PhotoGraph");
            position.Add(4, "Sale");
            position.Add(5, "Media");
            position.Add(6, "Manager");

            List<EmployeePositionTemp> empList = new List<EmployeePositionTemp>();
            foreach (var item in position)
            {
                EmployeePositionTemp empPosition = new EmployeePositionTemp();
                empPosition.Id = item.Key;
                empPosition.Position = item.Value;
            }

            return empList as IEnumerable<EmployeePositionTemp>;
        }

       
    } 
}