using GlassECommerce.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface ICollectionService
    {
        public Task<IActionResult> GetAllCollections();
        public Task<IActionResult> GetCollectionById(int collectionId);
        public Task<IActionResult> AddCollection(AddCollectionDTO model);
        public Task<IActionResult> EditCollection(int collectionId, AddCollectionDTO model);
        public Task<IActionResult> RemoveCollection(int collectionId);

    }
}
