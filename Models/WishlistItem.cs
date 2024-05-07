using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class WishlistItem
    {
        [Key]
        public int WishlistItemId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        //
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
