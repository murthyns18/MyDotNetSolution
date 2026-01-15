using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Publisher
    {
        public int PublisherID { get; set; }

        [Required(ErrorMessage = "Please enter publisher name")]
        [StringLength(100, ErrorMessage = "Publisher name cannot exceed 100 characters")]
        public string PublisherName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
