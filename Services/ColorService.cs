using Castle.Core.Internal;
using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlassECommerce.Services
{
    public class ColorService : ControllerBase, IColorService
    {
        private readonly ApplicationDbContext _dbContext;

        public ColorService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> GetAllColors()
        {
            var colors = await _dbContext.Colors.ToListAsync();
            if (colors.IsNullOrEmpty()) return Ok(new List<Color>());
            var response = colors.Select(c => new
            {
                ColorId = c.ColorId,
                ColorName = c.ColorName,
                Image = c?.Image
            }).ToList();
            return Ok(response);
        }

        public async Task<IActionResult> GetColorById(int colorId)
        {
            Color colorExist = await _dbContext.Colors.FindAsync(colorId);
            if (colorExist == null) return NotFound();
            return Ok(new
            {
                ColorId = colorExist.ColorId,
                ColorName = colorExist.ColorName,
                Image = colorExist?.Image
            });
        }

        public async Task<IActionResult> AddColor(ColorDTO model)
        {
            var colorExist = await _dbContext.Colors.FirstOrDefaultAsync(c => c.ColorName.ToUpper().Equals(model.ColorName.ToUpper()));
            if (colorExist != null) return BadRequest(new Response("Error", "This color name already exists"));
            Color newColor = new Color()
            {
                ColorName = model.ColorName,
                Image = model.Image.IsNullOrEmpty() ? "" : model.Image
            };
            try
            {
                await _dbContext.AddAsync(newColor);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created, new Response("Success", "Create new color successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when creating new color"));
            }
        }

        public async Task<IActionResult> EditColor(int colorId, ColorDTO model)
        {
            Color editColor = await _dbContext.Colors.FindAsync(colorId);
            if (editColor == null) return NotFound(new Response("Error", $"The color with id = {colorId} was not found"));
            var colorDuplicate = await _dbContext.Colors.FirstOrDefaultAsync(u => u.ColorName.ToUpper().Equals(model.ColorName.ToUpper()));
            if (colorDuplicate != null && colorDuplicate.ColorId != editColor.ColorId) return BadRequest(new Response("Error", "This color name already exists"));
            editColor.ColorName = model.ColorName;
            if (!model.Image.IsNullOrEmpty()) editColor.Image = model.Image;
            try
            {
                _dbContext.Update(editColor);
                await _dbContext.SaveChangesAsync();
                return Ok($"update the color with id = {colorId} successfully");

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when updating color"));
            }
        }

        public async Task<IActionResult> RemoveColor(int colorId)
        {
            Color deleteColor = await _dbContext.Colors.FindAsync(colorId);
            if (deleteColor == null) return NotFound(new Response("Error", $"The color with id = {colorId} was not found"));
            if (!deleteColor.Models.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response("Error", $"The color with id = {colorId} cannot be deleted because there is model using this color"));
            }
            try
            {
                _dbContext.Remove(deleteColor);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status204NoContent,
                    new Response("Success", $"remove the color with id = {colorId} successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when remove color"));
            }
        }

    }
}
