using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class ModelAttachment
    {
        [Key]
        public int ModelAttachmentId { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string Type { get; set; }
        //
        public virtual Model Model { get; set; }
    }
}
