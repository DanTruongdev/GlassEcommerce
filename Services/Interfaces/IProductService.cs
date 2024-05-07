using GlassECommerce.DTOs;
using GlassECommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface IProductService
    {




        public Task<IActionResult> GetAllProducts(int page);
        public Task<IActionResult> GetProductById(int productId);
        public Task<IActionResult> AddProduct(ProductDTO model);
        public Task<IActionResult> EditProduct(Product product, ProductDTO model);
        public Task<IActionResult> RemoveProduct(int productId);


        public Task<IActionResult> GetAllModelsByProductId(int productId);
        public Task<IActionResult> GetModelByIds(int productId, int modelId);
        public Task<IActionResult> AddModel(int productId, ModelDTO model);
        public Task<IActionResult> EditModel(int productId, Model modelExist, ModelDTO model);
        public Task<IActionResult> RemoveModel(int productId, int modelId);






    }
}
