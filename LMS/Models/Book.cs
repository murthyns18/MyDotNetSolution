using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Please enter book title")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Book title must be 3–10 characters")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please select category")]
        public int CategoryID { get; set; }

        [ValidateNever]
        public string CategoryName { get; set; } = string.Empty;

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();

        [Display(Name = "Publisher")]
        [Required(ErrorMessage = "Please select publisher")]
        public int PublisherID { get; set; }

        [ValidateNever]
        public string PublisherName { get; set; } = string.Empty;

        [ValidateNever]
        public IEnumerable<SelectListItem> PublisherList { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "Please enter price")]
        [Range(0.1, 1000, ErrorMessage = "Please enter valid price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please enter quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
