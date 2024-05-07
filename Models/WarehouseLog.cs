using GlassECommerce.Helper.Validation;
using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class WarehouseLog
    {

        [Key]
        public int WarehouseLogId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public bool IsImport { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        [Range(1, 1000, ErrorMessage = "The quantity must be betweem 1 and 1000")]
        public int Quantity { get; set; }
        public string? Note { get; set; }
        [Required]
        [CurrentDateValidation]
        public DateTime CreationDate { get; set; }
        //
        public virtual Model Model { get; set; }
        public virtual User User { get; set; }
    }
}
