using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        // Name
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter name.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        [MaxLength(10, ErrorMessage = "Name must not exceed 10 characters.")]
        public string UserName { get; set; } = string.Empty;

        // Email
        [Required(ErrorMessage = "Please enter email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        // Mobile Number
        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Please enter mobile number.")]
        [RegularExpression(@"^[6-9]\d{9}$",
            ErrorMessage = "Please enter a valid 10-digit mobile number.")]
        public string MobileNumber { get; set; } = string.Empty;

        // Address
        [Required(ErrorMessage = "Please enter address.")]
        public string Address { get; set; } = string.Empty;

        // Role
        [Display(Name = "Role")]
        [Required(ErrorMessage = "Please select role.")]
        public short? RoleID { get; set; }

        [ValidateNever]
        public string? RoleName { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }
            = new List<SelectListItem>();
            
        // Status
        [Required(ErrorMessage = "Please select Status.")]
        public bool Status { get; set; } = true;

        // Password
        [Required(ErrorMessage = "Please enter password.")]
        [StringLength(20, MinimumLength = 6,
            ErrorMessage = "Password must be between 6 and 20 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        // Confirm Password
        [Required(ErrorMessage = "Please enter confirm password.")]
        [DataType(DataType.Password)]
        [Compare("Password",
            ErrorMessage = "Password and confirm password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
