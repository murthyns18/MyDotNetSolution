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
        [Required(ErrorMessage = "Please enter Name.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        [MaxLength(10, ErrorMessage = "Name must not exceed 10 characters.")]
        public string UserName { get; set; } = string.Empty;

        // Email
        [Required(ErrorMessage = "Please enter Email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid Email.")]
        public string Email { get; set; } = string.Empty;

        // Mobile Number
        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Please enter Mobile Nuumber.")]
        [RegularExpression(@"^[6-9]\d{9}$",
            ErrorMessage = "Please enter a valid 10-digit Mobile Number.")]
        public string MobileNumber { get; set; } = string.Empty;

        // Address
        [Required(ErrorMessage = "Please enter Address.")]
        public string Address { get; set; } = string.Empty;

        // Role
        [Display(Name = "Role")]
        [Required(ErrorMessage = "Please select Role.")]
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
        [Required(ErrorMessage = "Please enter Password.")]
        [StringLength(20, MinimumLength = 6,
            ErrorMessage = "Password must be between 6 and 20 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        // Confirm Password
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please enter Confirm Password.")]
        [DataType(DataType.Password)]
        [Compare("Password",
            ErrorMessage = "Password and confirm password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
