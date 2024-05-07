using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.DTOs
{
    public class ModelDTO
    {
        [Required]
        public string ModelName { get; set; }
        [Required]
        public int UnitId { get; set; }
        public int? ColorId { get; set; }
        public string? Specification { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The price must be greater than 0")]
        public double Price { get; set; }
        public string? Description { get; set; }
        public List<AttachmentDto>? Attachments { get; set; }

    }

    public class AttachmentDto
    {
        [Required]
        public string Path { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
