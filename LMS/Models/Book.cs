using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter book title")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Book title must be 3–10 characters")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Please enter genre")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Genre name must be 3–10 characters")]
        public string Genre { get; set; } = "";

        [Required(ErrorMessage = "Please enter category")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Category name must be 3–10 characters")]
        public string Category { get; set; } = "";

        [Display (Name ="Publisher")]
        [Required (ErrorMessage ="Please enter publisher")]
        public int PublisherID { get; set; }

        public string PublisherName { get; set; } = "";

        [ValidateNever]
        public IEnumerable<SelectListItem> PublisherList { get; set; } 

        [Required(ErrorMessage = "Please enter price")]
        [Range(0.1, 1000, ErrorMessage = "Please enter valid price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please enter published year")]
        [Range(1000, 2100, ErrorMessage = "Please enter a valid published year")]
        [Display(Name ="Published Year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Please enter quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

    }
}
