using GlassECommerce.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface IColorService
    {
        public Task<IActionResult> GetAllColors();
        public Task<IActionResult> GetColorById(int colorId);
        public Task<IActionResult> AddColor(ColorDTO model);
        public Task<IActionResult> EditColor(int colorId, ColorDTO model);
        public Task<IActionResult> RemoveColor(int colorId);
    }
}
