using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "The item quantity must be betweem 1 and 100")]
        public int Quantity { get; set; }
        //
        public virtual User User { get; set; }
        public virtual Model Model { get; set; }

    }
}
