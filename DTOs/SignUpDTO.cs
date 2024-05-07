using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class SignUpDTO
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be contain between 2 and 50 characters")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be contain between 2 and 50 characters")]
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Password must be contain between 6 and 40 characters")]
        public string? Password { get; set; }

    }
}
