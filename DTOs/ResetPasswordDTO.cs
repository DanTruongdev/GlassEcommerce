using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Token { get; set; }
    }
}
