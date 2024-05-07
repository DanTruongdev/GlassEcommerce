using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class AddCollectionDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Collection name must be greater than 1 and less than 100 characters")]
        public string CollectionName { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
    }
}
