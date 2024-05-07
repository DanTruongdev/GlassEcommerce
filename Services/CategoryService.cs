using Castle.Core.Internal;
using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlassECommerce.Services
{
    public class CategoryService : ControllerBase, ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> AddCategory(string categoryName)
        {
            var categoryExist = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName.ToUpper().Equals(categoryName.ToUpper()));
            if (categoryExist != null) return BadRequest(new Response("Error", "This category name already exists"));
            Category newCategory = new Category()
            {
                CategoryName = categoryName
            };
            try
            {
                await _dbContext.AddAsync(newCategory);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created, new Response("Success", "Create category successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when creating new category"));
            }
        }

        public async Task<IActionResult> EditCategory(int categoryId, string categoryName)
        {
            Category editCategory = await _dbContext.Categories.FindAsync(categoryId);
            if (editCategory == null) return NotFound(new Response("Error", $"The category with id = {categoryId} was not found"));
            var categoryDuplicate = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName.ToUpper().Equals(categoryName.ToUpper()));
            if (categoryDuplicate != null && categoryDuplicate.CategoryId != editCategory.CategoryId) return BadRequest(new Response("Error", "This category name already exists"));
            editCategory.CategoryName = categoryName;
            try
            {
                _dbContext.Update(editCategory);
                await _dbContext.SaveChangesAsync();
                return Ok($"update category with id = {categoryId} successfully");

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when updating category"));
            }
        }

        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _dbContext.Categories
                .Include(c => c.Products)
                .ThenInclude(p => p.Models)
                .ToListAsync();
            if (categories.IsNullOrEmpty()) return Ok(new List<Category>());
            var response = categories.Select(c => new
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Products = c.Products.Select(p => new
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    VoteStar = p.VoteStar,
                    Sold = p.Sold,
                    Models = p.Models.Select(m => new
                    {
                        ModelId = m.ModelId,
                        modelname = m.ModelName,
                        unit = m.Unit.UnitName,
                        color = m.Color?.ColorName,
                        specification = m?.Specification,
                        price = m.Price,
                        available = m?.Available,
                        description = m?.Description
                    })
                })
            }).ToList();
            return Ok(response);
        }

        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            Category categoryExist = await _dbContext.Categories.FindAsync(categoryId);
            if (categoryExist == null) return NotFound();
            return Ok(new
            {
                CategoryId = categoryExist.CategoryId,
                CategoryName = categoryExist.CategoryName
            });
        }

        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            Category deleteCategory = await _dbContext.Categories.FindAsync(categoryId);
            if (deleteCategory == null) return NotFound(new Response("Error", $"The category with id = {categoryId} was not found"));
            if (!deleteCategory.Products.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response("Error", $"The category with id = {categoryId} cannot be deleted because there is product using this category"));
            }
            try
            {
                _dbContext.Remove(deleteCategory);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status204NoContent,
                    new Response("Success", $"remove category with id = {categoryId} successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when remove category"));
            }

        }

    }
}
