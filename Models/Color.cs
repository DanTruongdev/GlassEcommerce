using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        [Required]

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Color name  must be greater than 1 and less than 100 characters")]
        public string ColorName { get; set; }
        public string? Image { get; set; }
        //
        public virtual ICollection<Model> Models { get; set; }
    }
}
