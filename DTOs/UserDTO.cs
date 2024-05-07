using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class UserDTO
    {

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First name  must be greater than 1 and less than 100 characters")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name  must be greater than 1 and less than 100 characters")]
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Avatar { get; set; }
        public string? Address { get; set; }
    }
}
