using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class CheckoutDTO
    {
        [Required]
        public List<int>? CartItemIdList { get; set; }
    }
}
