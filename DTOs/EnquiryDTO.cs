using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class EnquiryDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Guest name  must be greater than 1 and less than 100 characters")]
        public string? GuestName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public int? ProductId { get; set; }
        public int? ModelId { get; set; }
        public int? CollectionId { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Comment { get; set; }


    }
}
