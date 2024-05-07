using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class Unit
    {
        [Key]
        public int UnitId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The unit name  must be greater than 1 and less than 100 characters")]
        public string UnitName { get; set; }
        //
        public virtual ICollection<Model> Models { get; set; }
    }
}
