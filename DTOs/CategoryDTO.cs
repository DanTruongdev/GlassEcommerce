using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class CategoryDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name  must be greater than 1 and less than 100 characters")]
        public string CategoryName { get; set; }

    }
}
