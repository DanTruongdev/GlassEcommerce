using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class Collection
    {
        [Key]
        public int CollectionId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Collection name must be greater than 1 and less than 100 characters")]
        public string CollectionName { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        //
        public virtual ICollection<CollectionProduct> CollectionProducts { get; set; }
    }
}
