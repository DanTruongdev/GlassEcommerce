using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class CartDTO
    {
        [Required]
        public int ModelId { get; set; }
        [Range(1, 100, ErrorMessage = "The quantity must be in range 1 to 100")]
        public int Quantity { get; set; }
    }
}
