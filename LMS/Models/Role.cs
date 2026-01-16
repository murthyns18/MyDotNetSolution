using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models
{
    public class Role
    {
        [Key]
        public short RoleID { get; set; }

        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Please enter role name.")]
        [MinLength(3, ErrorMessage = "Role name must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "Role name must not exceed 20 characters.")]
        public string RoleName { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public bool IsActive { get; set; } = true;
    }
}
