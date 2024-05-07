using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class WishlistDTO
    {
        [Required(ErrorMessage = "ProductId field is required")]
        public int ProductId { get; set; }
    }
}
