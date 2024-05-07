using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string? ProductCode { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Product name  must be greater than 1 and less than 500 characters")]
        public string ProductName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public double VoteStar { get; set; }
        public int Sold { get; set; }
        //
        public virtual Category? Category { get; set; }
        public virtual ICollection<Feedback>? Feedbacks { get; set; }
        public virtual ICollection<CollectionProduct>? CollectionProducts { get; set; }
        public virtual ICollection<Model>? Models { get; set; }
        public virtual ICollection<WishlistItem>? WishlistItems { get; set; }

    }
}
