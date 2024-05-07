using GlassECommerce.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface IWishlistService
    {
        public Task<IActionResult> AddWishlistItem(WishlistDTO model);
        public Task<IActionResult> GetAllWishlistItems();
        public Task<IActionResult> RemoveWishlistItems(int wishlistId);
    }
}
