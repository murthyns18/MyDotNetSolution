using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Book title is required")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Book title must be 10–20 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author name is required")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Author name must be 10–20 characters")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Genre must be 10–20 characters")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 100000, ErrorMessage = "Enter valid price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Publication year is required")]
        [Range(1000, 2100, ErrorMessage = "Enter a valid year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "ISBN must be 10–20 characters")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Publisher is required")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Publisher must be 10–20 characters")]
        public string Publisher { get; set; }
    }
}
