using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class ChangeRoleDTO
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
