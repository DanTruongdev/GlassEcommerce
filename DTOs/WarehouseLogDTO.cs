using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class WarehouseLogDTO
    {
        [Required]
        public bool IsImport { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        [Range(1, 1000, ErrorMessage = "The quantity must be betweem 1 and 1000")]
        public int Quantity { get; set; }
        public string? Note { get; set; }
    }
}
