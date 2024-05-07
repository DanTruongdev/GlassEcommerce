using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class ProductDTO
    {
        public string? ProductCode { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Product name  must be greater than 1 and less than 500 characters")]
        public string ProductName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<int>? CollectionIdList { get; set; }
    }
}
