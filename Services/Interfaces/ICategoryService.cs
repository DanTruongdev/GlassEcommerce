using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<IActionResult> AddCategory(string categoryName);
        public Task<IActionResult> EditCategory(int categoryId, string categoryName);
        public Task<IActionResult> GetAllCategories();
        public Task<IActionResult> GetCategoryById(int categoryId);
        public Task<IActionResult> RemoveCategory(int categoryId);
    }
}
