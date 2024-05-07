using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class ColorDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Color name  must be greater than 1 and less than 100 characters")]
        public string ColorName { get; set; }
        public string? Image { get; set; }
    }
}
