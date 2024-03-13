using System.ComponentModel.DataAnnotations;
using UtilitiesLib.NET6._0.Attribute;

namespace Stock_Manager.ViewModel
{
    public class RegisterViewModel
    {
        [Display(Name = "Your name")]
        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Must be a valid email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        [My1stPhoneValidation]           // verkar funka 24/10
        [DataType(DataType.PhoneNumber)]
        //[Required]
        public string? Phone { get; set; }

        [Required]
        [DataType(DataType.Password)] // string and Password type
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        // Account
        [Display(Name = "Account name")]
        public string? AccountName { get; set; }
        [Required(ErrorMessage = "Specify an amount to start with")]
        public double Saldo { get; set; }
    }
}
