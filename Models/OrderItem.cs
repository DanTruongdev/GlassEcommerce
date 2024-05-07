using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "The product quantity must be betweem 1 and 100")]
        public int Quantity { get; set; }
        [Required]
        public double Cost { get; set; }
        //
        public virtual Order Order { get; set; }
        public virtual Model Model { get; set; }
    }
}
