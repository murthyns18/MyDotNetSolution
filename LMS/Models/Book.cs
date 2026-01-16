using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        // Book Title
        [Display(Name = "Book Title")]
        [Required(ErrorMessage = "Please enter book title.")]
        [StringLength(20, MinimumLength = 3,
            ErrorMessage = "Book title must be between 3 and 20 characters.")]
        public string Title { get; set; } = string.Empty;

        // Category
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please select category.")]
        public int CategoryID { get; set; }

        [ValidateNever]
        public string CategoryName { get; set; } = string.Empty;

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
            = new List<SelectListItem>();

        // Publisher
        [Display(Name = "Publisher")]
        [Required(ErrorMessage = "Please select publisher.")]
        public int PublisherID { get; set; }

        [ValidateNever]
        public string PublisherName { get; set; } = string.Empty;

        [ValidateNever]
        public IEnumerable<SelectListItem> PublisherList { get; set; }
            = new List<SelectListItem>();

        // Price
        [Display(Name = "Price")]
        [Required(ErrorMessage = "Please enter price.")]
        [Range(0.1, 1000,
            ErrorMessage = "Please enter a valid price between 0.1 and 1000.")]
        public decimal Price { get; set; }

        // Quantity
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Please enter quantity.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // ISBN
        [Display(Name = "ISBN")]
        [StringLength(13, ErrorMessage = "ISBN must not exceed 13 characters.")]
        public string ISBN { get; set; } = string.Empty;

        // Status
        [Display(Name = "Status")]
        public bool IsActive { get; set; } = true;
    }
}
