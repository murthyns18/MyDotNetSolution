using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(3, ErrorMessage = "Name length should be at least 3 characters.")]
        [MaxLength(10, ErrorMessage = "Name length should not exceed 10 characters.")]
        public string UserName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number.")]
        public string MobileNumber { get; set; } = string.Empty;


        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;


        [Display(Name = "Role")]
        [Required(ErrorMessage = "Role is required.")]
        public short? RoleID { get; set; }

        public string? RoleName { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; } = new List<SelectListItem>();

        public bool Status { get; set; } = true;

        
        [Required(ErrorMessage = "Please enter a password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be 6–20 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;


        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
