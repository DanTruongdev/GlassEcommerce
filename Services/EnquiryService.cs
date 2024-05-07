using Castle.Core.Internal;
using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlassECommerce.Services
{
    public class EnquiryService : ControllerBase, IEnquiryService
    {
        private readonly ApplicationDbContext _dbContext;

        public EnquiryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IActionResult> AddEnquiry(EnquiryDTO model)
        {
            try
            {
                if (model.ProductId != null)
                {
                    var productExist = await _dbContext.Products.FindAsync(model.ProductId);
                    if (productExist == null) return BadRequest(new Response("Error", $"The product with id = {model.ProductId} was not found"));
                }
                if (model.ModelId != null)
                {
                    var modelExist = await _dbContext.Models.FindAsync(model.ModelId);
                    if (modelExist == null) return BadRequest(new Response("Error", $"The model with id = {model.ModelId} was not found"));
                }

                if (model.CollectionId != null)
                {
                    var collectionExist = await _dbContext.Collections.FindAsync(model.CollectionId);
                    if (collectionExist == null) return BadRequest(new Response("Error", $"The colleciton with id = {model.CollectionId} was not found"));
                }
                Enquiry newEnquiry = new Enquiry()
                {
                    EnquiryId = Guid.NewGuid().ToString(),
                    GuestName = model.GuestName,
                    ProducId = model.ProductId,
                    ModelId = model.ModelId,
                    CollectionId = model.CollectionId,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Comment = model.Comment,
                    Status = "Not seen",
                    CreationDate = DateTime.Now,
                    Product = await _dbContext.Products.FindAsync(model.ProductId)
                };
                await _dbContext.AddAsync(newEnquiry);
                await _dbContext.SaveChangesAsync();
                return Created("New enquiry created", new
                {
                    EnquiryId = newEnquiry.EnquiryId,
                    GuestName = newEnquiry.GuestName,
                    ProducId = newEnquiry.ProducId,
                    ModelId = newEnquiry.ModelId,
                    CollectionId = newEnquiry.CollectionId,
                    Email = newEnquiry.Email,
                    PhoneNumber = newEnquiry.PhoneNumber,
                    Comment = newEnquiry.Comment,
                    Status = newEnquiry.Status,
                    CreationDate = newEnquiry.CreationDate
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when adding new enquiry"));
            }
        }

        public async Task<IActionResult> GetAllEnquiries(int page)
        {
            var enquiries = page > 0 ? await _dbContext.Enquiries
                .Include(e => e.Product)
                .Include(e => e.Model)
                .Include(e => e.Collection)
                .OrderByDescending(e => e.CreationDate)
                .Skip(10 * (page - 1)).Take(10)
                .ToListAsync()
                :
                await _dbContext.Enquiries
                .Include(e => e.Model)
                .Include(e => e.Product)
                .Include(e => e.Collection)
                .OrderByDescending(e => e.CreationDate)
                .ToListAsync();
            if (enquiries.IsNullOrEmpty()) return Ok(new List<Enquiry>());

            return Ok(enquiries.Select(e => new
            {
                EnquiryId = e.EnquiryId,
                GuestName = e.GuestName,
                Collection = e.Collection != null ? new
                {
                    CollectionId = e.CollectionId,
                    CollectionName = e.Collection.CollectionName,
                    Products = e.Collection.CollectionProducts.Select(cp => new
                    {
                        ProductId = cp.Product.ProductId,
                        ProductCode = cp.Product.ProductCode,
                        ProductName = cp.Product.ProductName,
                        Category = cp.Product?.Category.CategoryName,
                        Model = cp?.Product?.Models.Select(m => new
                        {
                            ModelId = m.ModelId,
                            ModelName = m.ModelName,
                            Unit = m.Unit.UnitName,
                            Color = m.Color?.ColorName,
                            Specification = m.Specification,
                            Price = m.Price,
                            Available = m.Available,
                            Description = m?.Description
                        })
                    })
                } : null,
                Product = e.Product != null ? new
                {
                    ProductId = e.ProducId,
                    ProductCode = e.Product.ProductCode,
                    ProductName = e.Product.ProductName,
                    Category = e.Product?.Category.CategoryName,
                    Model = e?.Product?.Models.Select(m => new
                    {
                        ModelId = m.ModelId,
                        ModelName = m.ModelName,
                        Unit = m.Unit.UnitName,
                        Color = m.Color?.ColorName,
                        Specification = m.Specification,
                        Price = m.Price,
                        Available = m.Available,
                        Description = m?.Description
                    })
                } : null,
                Model = e.Model != null ? new
                {
                    ModelId = e.ModelId,
                    ModelName = e.Model?.ModelName,
                    Unit = e.Model.Unit.UnitName,
                    Color = e.Model.Color?.ColorName,
                    Specification = e.Model.Specification,
                    Price = e.Model.Price,
                    Available = e.Model.Available,
                    Description = e.Model?.Description
                } : null,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                Comment = e?.Comment,
                Status = e.Status,
                CreationDate = e.CreationDate
            }));
        }

        public async Task<IActionResult> GetEnquiryById(string enquiryId)
        {
            try
            {
                var enquiry = await _dbContext.Enquiries.FindAsync(enquiryId);
                if (enquiry == null) return NotFound(new Response("Error", $"The enquiry with id = {enquiryId} was not found"));
                enquiry.Status = "Seen";
                _dbContext.Update(enquiry);
                await _dbContext.SaveChangesAsync();
                return Ok(new
                {
                    EnquiryId = enquiry.EnquiryId,
                    GuestName = enquiry.GuestName,
                    Model = enquiry.Model != null ? new
                    {
                        ModelId = enquiry.ModelId,
                        ModelName = enquiry.Model?.ModelName,
                        Unit = enquiry.Model.Unit.UnitName,
                        Color = enquiry.Model.Color.ColorName,
                        Specification = enquiry.Model.Specification,
                        Price = enquiry.Model.Price,
                        Available = enquiry.Model.Available,
                        Description = enquiry.Model?.Description

                    } : null,
                    Email = enquiry.Email,
                    PhoneNumber = enquiry.PhoneNumber,
                    Comment = enquiry?.Comment,
                    Status = enquiry.Status,
                    CreationDate = enquiry.CreationDate
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", $"An error occurs when retriving enquiry with id = {enquiryId}"));
            }

        }

        public async Task<IActionResult> RemoveEnquiry(string enquiryId)
        {
            try
            {
                Enquiry enquiryExist = await _dbContext.Enquiries.FindAsync(enquiryId);
                if (enquiryExist == null) return NotFound(new Response("Error", $"The enquiry with id = {enquiryId} was not found"));
                _dbContext.Remove(enquiryExist);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when adding new enquiry"));
            }

        }
    }
}
