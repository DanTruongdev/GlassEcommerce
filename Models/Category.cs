using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name  must be greater than 1 and less than 100 characters")]
        public string CategoryName { get; set; }
        //
        public virtual ICollection<Product> Products { get; set; }
    }
}
