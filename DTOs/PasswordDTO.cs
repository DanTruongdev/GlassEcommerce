using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class PasswordDTO
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
