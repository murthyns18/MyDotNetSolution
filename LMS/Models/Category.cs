using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Please enter category name")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string CategoryName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

    }
}
