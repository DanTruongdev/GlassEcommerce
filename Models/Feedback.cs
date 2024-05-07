using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
        [Required]
        public string UserId { get; set; }
        public int? OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Title  must be greater than 1 and less than 500 characters")]
        public string Title { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Content must be greater than 10 characters")]
        public string Content { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "The vote star must be between 1 and 5")]
        public int Star { get; set; }
        [Required]
        public bool IsAnonymous { get; set; }
        [Required]
        public bool AllowEdit { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        //
        public virtual User? User { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
