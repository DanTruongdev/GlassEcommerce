using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services
{
    public class WishlistService : ControllerBase, IWishlistService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthenticationService _authService;

        public WishlistService(ApplicationDbContext dbContext, IAuthenticationService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<IActionResult> AddWishlistItem(WishlistDTO model)
        {
            try
            {
                var currentUser = await _authService.GetCurrentLoggedInUser();
                if (currentUser == null) return Unauthorized();
                bool checkExist = currentUser.WishListItems.Any(w => w.ProductId == model.ProductId);
                if (checkExist) return BadRequest(new Response("Error", $"The product with id = {model.ProductId} already exist"));
                var productExist = await _dbContext.Products.FindAsync(model.ProductId);
                if (productExist == null) return NotFound(new Response("Error", $"The product with id = {model.ProductId} was not found"));

                WishlistItem newWishlistItem = new WishlistItem()
                {
                    ProductId = productExist.ProductId,
                    UserId = currentUser.Id
                };
                await _dbContext.AddAsync(newWishlistItem);
                await _dbContext.SaveChangesAsync();
                return Created("New wishlist item created", new
                {
                    WishlistItemId = newWishlistItem.WishlistItemId,
                    UserId = newWishlistItem.UserId,
                    Product = new
                    {
                        ProductId = productExist.ProductId,
                        ProductCode = productExist.ProductCode,
                        ProductName = productExist.ProductName,
                        Category = new
                        {
                            CategoryId = productExist.CategoryId,
                            CategoryName = productExist.Category.CategoryName
                        },
                        Collection = productExist.CollectionProducts.Select(cd => new
                        {
                            CollectionId = cd.CollectionId,
                            CollectionName = cd.Collection.CollectionName
                        }),
                        VoteStar = productExist.VoteStar,
                        Sold = productExist.Sold,
                        Price = productExist.Models.Any() ? productExist.Models.First().Price : 0,
                        Image = productExist.Models.Any(m => m.ModelAttachments.Any()) ? 
                                productExist.Models?.FirstOrDefault()?.ModelAttachments?.FirstOrDefault(a => a.Type.Equals("Image"))?.Path : ""
                    }
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  new Response("Error", "An error occurs when adding new wishlist item"));
            }


        }

        public async Task<IActionResult> GetAllWishlistItems()
        {
            var currentUser = await _authService.GetCurrentLoggedInUser();
            if (currentUser == null) return Unauthorized();
            if (!currentUser.WishListItems.Any()) Ok(new List<WishlistItem>());
            var wishlistItems = currentUser.WishListItems.ToList();
            return Ok(wishlistItems.Select(w => new
            {
                WishlistItemId = w.WishlistItemId,
                UserId = w.UserId,
                Product = new
                {
                    ProductId = w.Product.ProductId,
                    ProductCode = w.Product.ProductCode,
                    ProductName = w.Product.ProductName,
                    Category = new
                    {
                        CategoryId = w.Product.CategoryId,
                        CategoryName = w.Product.Category.CategoryName
                    },
                    Collection = w.Product.CollectionProducts.Select(cd => new
                    {
                        CollectionId = cd.CollectionId,
                        CollectionName = cd.Collection.CollectionName
                    }),
                    VoteStar = w.Product.VoteStar,
                    Sold = w.Product.Sold,
                    Price = w.Product.Models.Any() ? w.Product.Models.First().Price : 0,
                    Image = w.Product.Models.Any(m => m.ModelAttachments.Any()) ? w.Product.Models?.FirstOrDefault()?.ModelAttachments?.FirstOrDefault(a => a.Type.Equals("Image"))?.Path : ""
                }
            }));
        }

        public async Task<IActionResult> RemoveWishlistItems(int wishlistId)
        {
            try
            {
                var currentUser = await _authService.GetCurrentLoggedInUser();
                if (currentUser == null) return Unauthorized();
                var wishlistItem = currentUser.WishListItems?.FirstOrDefault(w => w.WishlistItemId == wishlistId);
                if (wishlistItem == null) return NotFound(new Response("Error", $"The wishlist item with id = {wishlistId} does not exist in user's wishlist"));
                _dbContext.Remove(wishlistItem);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  new Response("Error", $"An error occurs when removing wishlist item with id = {wishlistId}"));
            }


        }


    }
}
