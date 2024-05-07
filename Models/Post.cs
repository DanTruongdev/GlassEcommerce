using GlassECommerce.Helper.Validation;
using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Title  must be greater than 1 and less than 500 characters")]
        public string Title { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Content must be greater than 10 characters")]
        public string Content { get; set; }
        public string? Thumbnail { get; set; }
        [Required]
        [CurrentDateValidation]
        public DateTime CreationDate { get; set; }
        [CurrentDateValidation]
        public DateTime? LatestUpdate { get; set; }
        //
        public virtual User User { get; set; }
    }
}
