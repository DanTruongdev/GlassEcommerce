using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First name  must be greater than 1 and less than 100 characters")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name  must be greater than 1 and less than 100 characters")]
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Avatar { get; set; }
        public string? Address { get; set; }
        [Required]
        public bool IsActivated { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
        //
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; }
        public virtual ICollection<WishlistItem>? WishListItems { get; set; }
        public virtual ICollection<Feedback>? Feedbacks { get; set; }
        public virtual ICollection<WarehouseLog>? Imports { get; set; }

    }
}
