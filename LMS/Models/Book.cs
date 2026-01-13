using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Book
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Book title is required")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Book title must be 3–10 characters")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Genre name must be 3–10 characters")]
        public string Genre { get; set; } = "";

        [Required(ErrorMessage = "Category is required")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Category name must be 3–10 characters")]
        public string Category { get; set; } = "";

        [Display (Name ="Publisher Name")]
        [Required (ErrorMessage ="Publisher Name is required")]
        public int PublisherID { get; set; }

        public string PublisherName { get; set; } = "";

        [ValidateNever]
        public IEnumerable<SelectListItem> PublisherList { get; set; } 

        [Required(ErrorMessage = "Price is required")]
        [Range(0.1, 1000, ErrorMessage = "Enter valid price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Publication year is required")]
        [Range(1000, 2100, ErrorMessage = "Enter a valid year")]
        [Display(Name ="Published Year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

    }
}
