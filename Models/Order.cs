using GlassECommerce.Helper.Validation;
using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Delivery address  must be greater than 1 and less than 500 characters")]
        public string DeliveryAddress { get; set; }

        [Required]
        public string PaymentMethod { get; set; }
        public string? Note { get; set; }
        [Required]
        [CurrentDateValidation(ErrorMessage = "The order date cannot be less than current date")]
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        [Required]
        public string OrderStatus { get; set; }
        [Required]
        public double TotalCost { get; set; }
        //
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
