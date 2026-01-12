
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Display(Name = "Name")]
        [Required (ErrorMessage="Name is required.")]
        [MinLength(3, ErrorMessage ="Name length should be atleast 3 characters.")]
        [MaxLength(10)]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10-digit mobile number.")]
        public string MobileNumber { get; set; }


        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Role is required.")]
        public short? RoleID { get; set; }

        public string RoleName =>
            RoleID.HasValue ? ((RoleType)RoleID.Value).ToString() : "";

        [ValidateNever]

        public IEnumerable<SelectListItem> RoleList { get; set; }

        public bool Status { get; set; }
    }

    public enum RoleType : short
    {
        Admin = 1,
        Staff = 2,
        GeneralUser = 3
    }
}