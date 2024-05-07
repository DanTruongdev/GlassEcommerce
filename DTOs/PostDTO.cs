using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class PostDTO
    {
        public string? Thumbnail { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Title  must be greater than 1 and less than 500 characters")]
        public string? Title { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Content must be greater than 10 characters")]
        public string? Description { get; set; }

    }
}
