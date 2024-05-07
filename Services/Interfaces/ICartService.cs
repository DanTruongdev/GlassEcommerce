using GlassECommerce.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface ICartService
    {
        public Task<IActionResult> AddToCart(CartDTO model);
        public Task<IActionResult> Checkout();
        public Task<IActionResult> EditCart(int cartId, CartDTO model);
        public Task<IActionResult> GetAllCarts();
        public Task<IActionResult> RemoveCartItem(int cartId);
    }
}
