using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using UtilitiesLib.NET6._0.Attribute;

namespace Stock_Manager.ViewModel
{
    public class LoginViewModel
    {
        

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Must be a valid email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        

        [Required]
        [DataType(DataType.Password)] // string and Password type
        public string Password { get; set; }

        //[Display(Name = "Confirm password")]
        //[Required(ErrorMessage = "Confirm password is required")]
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "Passwords do not match")]
        //public string ConfirmPassword { get; set; }
    }
}
