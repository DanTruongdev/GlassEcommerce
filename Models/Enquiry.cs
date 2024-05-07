using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class Enquiry
    {
        [Key]
        public string EnquiryId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Guest name  must be greater than 1 and less than 100 characters")]
        public string GuestName { get; set; }
        public int? ProducId { get; set; }
        public int? ModelId { get; set; }
        public int? CollectionId { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        //
        public virtual Product? Product { get; set; }
        public virtual Model? Model { get; set; }
        public virtual Collection? Collection { get; set; }

    }
}
