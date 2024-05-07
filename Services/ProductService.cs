using Castle.Core.Internal;
using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlassECommerce.Services
{
    public class ProductService : ControllerBase, IProductService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        //Products
        public async Task<IActionResult> GetAllProducts(int page)
        {
            int pageSize = 20;
            var skipAmount = pageSize * (page - 1);

            var productsQuery = _dbContext.Products
                .Include(p => p.Models)
                .ThenInclude(m => m.ModelAttachments)
                .Include(p => p.Category)
                .Include(p => p.Feedbacks)
                .Include(p => p.CollectionProducts)
                .ThenInclude(cp => cp.Collection)
                .OrderBy(p => p.ProductId)
                .AsQueryable();

            if (page > 0)
            {
                productsQuery = productsQuery.Skip(skipAmount).Take(pageSize);
            }

            var products = await productsQuery.ToListAsync();

            if (products.IsNullOrEmpty())
                return Ok(new List<Product>());

            var response = products.Select(p => new
            {
                ProductId = p.ProductId,
                ProductCode = p.ProductCode,
                ProductName = p.ProductName,
                Category = new
                {
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                },
                Collection = p.CollectionProducts.Select(cd => new
                {
                    CollectionId = cd.CollectionId,
                    CollectionName = cd.Collection.CollectionName
                }),
                VoteStar = p.VoteStar,
                Sold = p.Sold,
                Price = p.Models.Any() ? p.Models.First().Price : 0,
                Image = p.Models.FirstOrDefault(m => m.ModelAttachments.Any())?.ModelAttachments.Select(ma => new
                {
                    Path = ma.Path,
                    Type = ma.Type
                }),
                Feedbacks = p.Feedbacks?.Select(f => new
                {
                    FeedbackId = f.FeedbackId,
                    User = f.IsAnonymous ? "Ẩn danh" : f.User.ToString(),
                    Title = f.Title,
                    Content = f.Content,
                    Star = f.Star,
                    CreationDate = f.CreationDate
                })
            });

            return Ok(response);
        }
        public async Task<IActionResult> GetProductById(int productId)
        {
            Product productExist = await _dbContext.Products.FindAsync(productId);
            if (productExist == null) return NotFound();
            await _dbContext.Entry(productExist).Collection(p => p.Models).LoadAsync();
            return Ok(new
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
                Image = productExist.Models.FirstOrDefault(m => m.ModelAttachments.Any())?.ModelAttachments.Select(ma => new
                {
                    Path = ma.Path,
                    Type = ma.Type
                }),
                Feedbacks = productExist.Feedbacks?.Select(f => new
                {
                    FeedbackId = f.FeedbackId,
                    User = f.IsAnonymous ? "Ẩn danh" : f.User.ToString(),
                    Title = f.Title,
                    Content = f.Content,
                    Star = f.Star,
                    CreationDate = f.CreationDate
                })

            });
        }
        public async Task<IActionResult> AddProduct(ProductDTO model)
        {
            try
            {
                Product newProduct = new Product()
                {
                    ProductCode = model.ProductCode.IsNullOrEmpty() ? "" : model.ProductCode,
                    ProductName = model.ProductName,
                    CategoryId = model.CategoryId,
                    CollectionProducts = new List<CollectionProduct>(),
                    VoteStar = 0,
                    Sold = 0
                };

                await _dbContext.AddAsync(newProduct);
                await _dbContext.SaveChangesAsync();
                if (model.CollectionIdList != null && model.CollectionIdList.Count > 0 && !model.CollectionIdList.Contains(0))
                {
                    foreach (int collectionId in model.CollectionIdList)
                    {

                        CollectionProduct newCollectionProduct = new CollectionProduct()
                        {
                            CollectionId = collectionId,
                            ProductId = newProduct.ProductId
                        };
                        await _dbContext.AddAsync(newCollectionProduct);
                    }
                }
                await _dbContext.SaveChangesAsync();
                return Created("Create new product successfully", new
                {
                    ProductId = newProduct.ProductId,
                    ProductCode = newProduct.ProductCode,
                    ProductName = newProduct.ProductName,
                    CategoryId = newProduct.CategoryId,
                    Collection = newProduct.CollectionProducts.Select(c => new
                    {
                        CollectionId = c.CollectionId,
                        CollectionName = c.Collection.CollectionName
                    }),
                    VoteStar = 0,
                    Sold = 0
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when create new product"));
            }
        }
        public async Task<IActionResult> EditProduct(Product product, ProductDTO model)
        {
            try
            {

                product.ProductCode = model.ProductCode.IsNullOrEmpty() ? "" : model.ProductCode;
                product.ProductName = model.ProductName;
                product.CategoryId = model.CategoryId;
                _dbContext.Update(product);
                var collectionProductList = await _dbContext.CollectionProducts.Where(c => c.ProductId == product.ProductId).ToListAsync();
                foreach (var collectionProduct in collectionProductList)
                {
                    _dbContext.Remove(collectionProduct);
                }
                await _dbContext.SaveChangesAsync();

                if (model.CollectionIdList != null && model.CollectionIdList.Count > 0 && !model.CollectionIdList.Contains(0))
                {
                    foreach (int collectionId in model.CollectionIdList)
                    {

                        CollectionProduct newCollectionProduct = new CollectionProduct()
                        {
                            CollectionId = collectionId,
                            ProductId = product.ProductId
                        };
                        await _dbContext.AddAsync(newCollectionProduct);
                    }
                }

                await _dbContext.SaveChangesAsync();
                return Ok(new
                {
                    ProductId = product.ProductId,
                    ProductCode = product.ProductCode,
                    ProductName = product.ProductName,
                    CategoryId = product.CategoryId,
                    Collection = product.CollectionProducts.Select(c => new
                    {
                        CollectionId = c.CollectionId,
                        CollectionName = c.Collection.CollectionName
                    }),
                    VoteStar = product.VoteStar,
                    Sold = product.Sold
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when update product"));
            }
        }
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            Product deleteProduct = await _dbContext.Products.FindAsync(productId);
            if (deleteProduct == null) return NotFound(new Response("Error", $"The product with id = {productId} was not found"));

            try
            {
                deleteProduct.Feedbacks.Clear();
                deleteProduct.CollectionProducts.Clear();
                deleteProduct.Models.Clear();
                _dbContext.Remove(deleteProduct);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status204NoContent,
                    new Response("Success", $"remove the product with id = {productId} successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when remove product"));
            }
        }

        //Model
        public async Task<IActionResult> GetAllModelsByProductId(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null) return BadRequest(new Response("Error", $"The product with id = {productId} was not found"));
            var models = _dbContext.Models.Include(m => m.Color).Include(m => m.ModelAttachments).Where(m => m.ProductId == productId).ToList();
            if (models.IsNullOrEmpty()) return Ok(new List<Model>());
            var response = models.Select(m => new
            {
                ModelId = m.ModelId,
                ModelName = m?.ModelName,
                ProductId = m.ProductId,
                Color = new
                {
                    ColorId = m.Color?.ColorId,
                    ColorName = m.Color?.ColorName,
                    Image = m.Color?.Image
                },
                UnitId = m.UnitId,
                Specification = m.Specification,
                Price = m.Price,
                Available = m.Available,
                Description = m.Description,
                Attachments = m.ModelAttachments.Select(a => new
                {
                    Path = a.Path,
                    Type = a.Type
                })
            }).ToList();
            return Ok(response);
        }

        public async Task<IActionResult> AddModel(int productId, ModelDTO model)
        {
            try
            {
                Model newModel = new Model()
                {
                    ModelName = model.ModelName,
                    ProductId = productId,
                    UnitId = model.UnitId,
                    ColorId = model.ColorId == null || model.ColorId == 0 ? null : model.ColorId,
                    Specification = model.Specification.IsNullOrEmpty() ? "" : model.Specification,
                    Price = model.Price,
                    Available = 0,
                    Description = model.Description.IsNullOrEmpty() ? "" : model.Description
                };
                await _dbContext.AddAsync(newModel);
                await _dbContext.SaveChangesAsync();
                if (model.Attachments.Any())
                {
                    foreach (var attachment in model.Attachments)
                    {
                        ModelAttachment newAttachment = new ModelAttachment()
                        {
                            ModelId = newModel.ModelId,
                            Path = attachment.Path,
                            Type = attachment.Type
                        };
                        await _dbContext.AddAsync(newAttachment);
                    }
                }
                else newModel.ModelAttachments = new List<ModelAttachment>();
                await _dbContext.SaveChangesAsync();
                return Created("Create new model successfully", new
                {
                    ModelId = newModel.ModelId,
                    ProductId = newModel.ProductId,
                    UnitId = newModel.UnitId,
                    ColorId = newModel.ColorId,
                    Specification = newModel.Specification,
                    Price = newModel.Price,
                    Available = newModel.Available,
                    Description = newModel.Description,
                    Attachments = newModel.ModelAttachments.Select(a => new
                    {
                        Path = a.Path,
                        Type = a.Type
                    })
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when create new model"));
            }
        }

        public async Task<IActionResult> EditModel(int productId, Model modelExist, ModelDTO model)
        {
            try
            {
                modelExist.ModelName = model.ModelName;
                modelExist.ProductId = productId;
                modelExist.UnitId = model.UnitId;
                modelExist.ColorId = model.ColorId == 0 || model.ColorId == null ? modelExist.ColorId : model.ColorId;
                modelExist.Specification = model.Specification.IsNullOrEmpty() ? "" : model.Specification;
                modelExist.Price = model.Price;
                modelExist.Description = model.Description.IsNullOrEmpty() ? "" : model.Description;

                if (model.Attachments.Any())
                {
                    modelExist.ModelAttachments.Clear();
                    await _dbContext.SaveChangesAsync();
                    foreach (var attachment in model.Attachments)
                    {
                        ModelAttachment newAttachment = new ModelAttachment()
                        {
                            ModelId = modelExist.ModelId,
                            Path = attachment.Path,
                            Type = attachment.Type
                        };
                        await _dbContext.AddAsync(newAttachment);
                    }
                }
                await _dbContext.SaveChangesAsync();
                return Ok(new
                {
                    ModelId = modelExist.ModelId,
                    ProductId = modelExist.ProductId,
                    UnitId = modelExist.UnitId,
                    ColorId = modelExist.ColorId,
                    Specification = modelExist.Specification,
                    Price = model.Price,
                    Available = modelExist.Available,
                    Description = modelExist.Description,
                    Attachments = modelExist.ModelAttachments.Select(a => new
                    {
                        Path = a.Path,
                        Type = a.Type
                    })
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when updating model"));
            }
        }

        public async Task<IActionResult> GetModelByIds(int productId, int modelId)
        {
            Product productExist = await _dbContext.Products.FindAsync(productId);
            if (productExist == null) return NotFound(new Response("Error", $"The product with = {productId} was not found"));
            Model modelExist = productExist.Models.FirstOrDefault(m => m.ModelId == modelId);
            if (modelExist == null) return NotFound(new Response("Error", $"The product with = {productId} does not contain any model with id = {modelId}"));

            var response = new
            {
                ModelId = modelExist.UnitId,
                ModelName = modelExist?.ModelName,
                ProductId = modelExist.ProductId,
                UnitId = modelExist.UnitId,
                ColorId = modelExist?.ColorId,
                Specification = modelExist.Specification,
                Price = modelExist.Price,
                Available = modelExist.Available,
                Description = modelExist.Description,
                Attachments = modelExist.ModelAttachments.Select(a => new
                {
                    Path = a.Path,
                    Type = a.Type
                })
            };
            return Ok(response);
        }

        public async Task<IActionResult> RemoveModel(int productId, int modelId)
        {
            Product productExist = await _dbContext.Products.FindAsync(productId);
            if (productExist == null) return NotFound(new Response("Error", $"The product with = {productId} was not found"));
            Model modelExist = productExist.Models.FirstOrDefault(m => m.ModelId == modelId);
            if (modelExist == null) return NotFound(new Response("Error", $"The product with = {productId} does not contain any model with id = {modelId}"));
            try
            {
                modelExist.WarehouseLogs.Clear();
                modelExist.ModelAttachments.Clear();
                modelExist.CartItems.Clear();
                _dbContext.Remove(modelExist);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when remove model"));
            }
        }





    }
}
