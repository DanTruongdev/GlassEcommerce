using Castle.Core.Internal;
using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlassECommerce.Services
{
    public class CollectionService : ControllerBase, ICollectionService
    {
        private readonly ApplicationDbContext _dbContext;

        public CollectionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> GetAllCollections()
        {
            var collections = await _dbContext.Collections.ToListAsync();
            if (collections.IsNullOrEmpty()) return Ok(new List<Collection>());
            var response = collections.Select(c => new
            {
                c.CollectionId,
                c.CollectionName,
                c.Description,
                c.Thumbnail,
            }).ToList();
            return Ok(response);
        }

        public async Task<IActionResult> GetCollectionById(int collectionId)
        {
            Collection collectionExist = await _dbContext.Collections.FindAsync(collectionId);
            if (collectionExist == null) return NotFound();
            return Ok(new
            {
                collectionExist.CollectionId,
                collectionExist.CollectionName,
                collectionExist.Description,
                collectionExist.Thumbnail
            });
        }

        public async Task<IActionResult> AddCollection(AddCollectionDTO model)
        {
            var collectionExist = await _dbContext.Collections.FirstOrDefaultAsync(c => c.CollectionName.ToUpper().Equals(model.CollectionName.ToUpper()));
            if (collectionExist != null) return BadRequest(new Response("Error", "This collection name already exists"));
            Collection newCollection = new Collection()
            {
                CollectionName = model.CollectionName,
                Description = model.Description.IsNullOrEmpty() ? "" : model.Description,
                Thumbnail = model.Thumbnail.IsNullOrEmpty() ? "" : model.Thumbnail
            };
            try
            {
                await _dbContext.AddAsync(newCollection);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created, new
                {
                    CollectionId = newCollection.CollectionId,
                    CollectionName = newCollection.CollectionName,
                    Description = newCollection.Description,
                    Thumbnail = newCollection.Thumbnail
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when creating new collection"));
            }
        }

        public async Task<IActionResult> EditCollection(int collectionId, AddCollectionDTO model)
        {
            Collection editCollection = await _dbContext.Collections.FindAsync(collectionId);
            if (editCollection == null) return NotFound(new Response("Error", $"The collection with id = {collectionId} was not found"));
            var collectionDuplicate = await _dbContext.Collections.FirstOrDefaultAsync(u => u.CollectionName.ToUpper().Equals(model.CollectionName.ToUpper()));
            if (collectionDuplicate != null && collectionDuplicate.CollectionId != editCollection.CollectionId)
                return BadRequest(new Response("Error", "This collection name already exists"));
            editCollection.CollectionName = model.CollectionName;
            editCollection.Description = model.Description.IsNullOrEmpty() ? editCollection.Description : model.Description;
            editCollection.Thumbnail = model.Thumbnail.IsNullOrEmpty() ? editCollection.Description : model.Thumbnail;

            try
            {
                _dbContext.Update(editCollection);
                await _dbContext.SaveChangesAsync();
                return Ok($"update the collection with id = {collectionId} successfully");

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when updating collection"));
            }
        }

        public async Task<IActionResult> RemoveCollection(int collectionId)
        {
            Collection deleteCollection = await _dbContext.Collections.FindAsync(collectionId);
            if (deleteCollection == null) return NotFound(new Response("Error", $"The collection with id = {collectionId} was not found"));
            if (!deleteCollection.CollectionProducts.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response("Error", $"The collection with id = {collectionId} cannot be deleted because there is product using this collection"));
            }
            try
            {
                _dbContext.Remove(deleteCollection);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status204NoContent,
                    new Response("Success", $"remove the collection with id = {collectionId} successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when remove collection"));
            }
        }

    }
}
