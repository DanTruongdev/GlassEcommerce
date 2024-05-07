using Castle.Core.Internal;
using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Schema;

namespace GlassECommerce.Services
{
    public class CartService : ControllerBase, ICartService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthenticationService _authService;

        public CartService(ApplicationDbContext dbContext, IAuthenticationService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<IActionResult> AddToCart(CartDTO model)
        {
            var userExist = await _authService.GetCurrentLoggedInUser();

            if (userExist == null) return Unauthorized();
            var modelExist = await _dbContext.Models.FindAsync(model.ModelId);
            if (modelExist == null) return NotFound(new Response("Error", $"The model with id = {model.ModelId} was not found"));
            if (modelExist.Available < model.Quantity) return BadRequest(new Response("Error", $"The quantity is greater than model available"));
            var cartItemExist = userExist.CartItems.FirstOrDefault(c => c.ModelId == modelExist.ModelId);
            try
            {
                if (cartItemExist == null)
                {
                    CartItem newCartItem = new CartItem()
                    {
                        UserId = userExist.Id,
                        ModelId = model.ModelId,
                        Quantity = model.Quantity,
                    };
                    await _dbContext.AddAsync(newCartItem);
                }
                else
                {
                    cartItemExist.Quantity += model.Quantity;
                    _dbContext.Update(cartItemExist);
                }
                await _dbContext.SaveChangesAsync();
                return Ok(new Response("Success", "Add to cart successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
              new Response("Error", "An error occurs when adding model to cart"));
            }
        }

        public async Task<IActionResult> Checkout()
        {
            var useExist = await _authService.GetCurrentLoggedInUser();
            if (useExist == null) return Unauthorized();
            var userCartItem = useExist.CartItems;
            if (userCartItem.IsNullOrEmpty()) return BadRequest(new Response("Error", "There is no items in user cart"));
            double total = 0;
            foreach (var cartItemExist in userCartItem)
            {
                total += cartItemExist.Model.Price * cartItemExist.Quantity; 
            }
     
            
            return Ok(new
            {
                Detail = userCartItem.Select(c => new
                {
                    ModelId = c.Model.ModelId,
                    ModelName = c.Model?.ModelName,
                    ProductId = c.Model?.ProductId,
                    Color = new
                    {
                        ColorId = c.Model?.Color?.ColorId,
                        ColorName = c.Model?.Color?.ColorName,
                        Image = c.Model?.Color?.Image
                    },
                    UnitId = c.Model.UnitId,
                    Specification = c.Model.Specification,
                    Price = c.Model.Price,
                    Available = c.Model.Available,
                    Description = c.Model.Description,
                    Attachments = c.Model.ModelAttachments.Select(a => new
                    {
                        Path = a.Path,
                        Type = a.Type
                    }),
                    Quantity = c.Quantity,
                    Total = c.Model.Price * c.Quantity
                }),
                TotalCost = total
            });
        }

        public async Task<IActionResult> EditCart(int cartId, CartDTO model)
        {
            var userExist = await _authService.GetCurrentLoggedInUser();
            if (userExist == null) return Unauthorized();
            var cartItemExist = userExist.CartItems.FirstOrDefault(c => c.CartItemId == cartId);
            if (cartItemExist == null) return NotFound(new Response("Error", $"The cart item with id = {cartId} was not found"));
            var modelExist = await _dbContext.Models.FindAsync(model.ModelId);
            if (modelExist == null) return NotFound(new Response("Error", $"The model with id = {model.ModelId} was not found"));

            try
            {

                cartItemExist.Quantity = model.Quantity;
                _dbContext.Update(cartItemExist);
                await _dbContext.SaveChangesAsync();
                return Ok(new Response("Success", "update cart item successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
              new Response("Error", "An error occurs when updating cart item"));
            }
        }
        public async Task<IActionResult> GetAllCarts()
        {
            var userExist = await _authService.GetCurrentLoggedInUser();
            if (userExist == null) return Unauthorized();
            var carts = userExist.CartItems.ToList();
            if (carts.IsNullOrEmpty()) return Ok(new List<CartItem>());
            return Ok(carts.Select(c => new
            {
                cartItemId = c.CartItemId,
                UserId = c.UserId,
                ModelId = new
                {
                    ModelId = c.Model.ModelId,
                    ModelName = c.Model?.ModelName,
                    color = c.Model?.Color?.ColorName,
                    Specificaiton = c.Model?.Specification,
                    Price = c.Model?.Price,
                    Available = c.Model?.Available,
                    Image = c.Model.ModelAttachments.Any() ? c.Model.ModelAttachments.First().Path : "",
                    Description = c.Model.Description
                },
                Quantity = c.Quantity,
                Total = c.Quantity * c.Model.Price
            }));
        }
        public async Task<IActionResult> RemoveCartItem(int cartId)
        {
            var userExist = await _authService.GetCurrentLoggedInUser();
            if (userExist == null) return Unauthorized();
            CartItem deleteCartItem = await _dbContext.CartItems.FindAsync(cartId);
            if (deleteCartItem == null) return NotFound(new Response("Error", $"The cart with id = {cartId} was not found"));
            try
            {
                _dbContext.Remove(deleteCartItem);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status204NoContent,
                    new Response("Success", $"remove cart item with id = {cartId} successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when removing cart item"));
            }
        }

    }
}
