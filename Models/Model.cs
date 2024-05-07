using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class Model
    {
        [Key]
        public int ModelId { get; set; }
        public string? ModelName { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int UnitId { get; set; }
        public int? ColorId { get; set; }
        public string? Specification { get; set; }
        //[Required]
        //[Range(0, double.MaxValue, ErrorMessage = "The primary price must be greater than 0")]
        //public double PrimaryPrice { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The price must be greater than 0")]
        public double Price { get; set; }
        public int? Available { get; set; }
        public string? Description { get; set; }
        //
        public virtual Product Product { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual Color Color { get; set; }
        public virtual ICollection<WarehouseLog> WarehouseLogs { get; set; }
        public virtual ICollection<ModelAttachment> ModelAttachments { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }


    }
}
