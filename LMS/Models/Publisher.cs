using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Publisher
    {
        [Key]
        public int PublisherID { get; set; }

        [Display(Name = "Publisher Name")]
        [Required(ErrorMessage = "Please enter publisher name.")]
        [StringLength(100, ErrorMessage = "Publisher name must not exceed 100 characters.")]
        public string PublisherName { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public bool IsActive { get; set; } = true;
    }
}
