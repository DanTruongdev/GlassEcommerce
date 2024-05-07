using Castle.Core.Internal;
using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Roles = "ADMIN")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;
        private readonly ICategoryService _categoryService;
        private readonly IUnitService _unitService;
        private readonly ICollectionService _collectionService;
        private readonly IColorService _colorService;
        private readonly IPostService _postService;
        private readonly IWarehouseLogService _warehouseLogService;
        private readonly IOrderService _orderService;
        private readonly IEnquiryService _enquiryService;
        private readonly IFeedbackService _feedbackService;



        public AdminController(IProductService productService, IUserService userService,
            ApplicationDbContext dbContext, IAuthenticationService authService,
            ICategoryService categoryService,
            IUnitService unitService, ICollectionService collectionService,
            IColorService colorService, IPostService postService,
            IWarehouseLogService warehouseLogService, IOrderService orderService, IEnquiryService enquiryService, IFeedbackService feedbackService)
        {
            _productService = productService;
            _userService = userService;
            _dbContext = dbContext;
            _authService = authService;
            _categoryService = categoryService;
            _unitService = unitService;
            _collectionService = collectionService;
            _colorService = colorService;
            _postService = postService;
            _warehouseLogService = warehouseLogService;
            _orderService = orderService;
            _enquiryService = enquiryService;
            _feedbackService = feedbackService;
        }



        [AllowAnonymous]

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var res = await _categoryService.GetAllCategories();
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }

        [AllowAnonymous]
        [HttpGet("categories/{categoryId}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int categoryId)
        {
            try
            {
                var res = await _categoryService.GetCategoryById(categoryId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }


        [HttpPost("categories/add")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO model)
        {
            if (model.CategoryName.Length < 2) return BadRequest("Category name must be greater than 1 character");
            try
            {
                var res = await _categoryService.AddCategory(model.CategoryName);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }


        [HttpPut("categories/edit/{categoryId}")]
        public async Task<IActionResult> EditCategory([FromRoute] int categoryId, [FromBody] CategoryDTO model)
        {
            if (model.CategoryName.Length < 2) return BadRequest("Category name must be greater than 1 character");
            try
            {
                var res = await _categoryService.EditCategory(categoryId, model.CategoryName);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }

        [HttpDelete("categories/remove/{categoryId}")]
        public async Task<IActionResult> EditCategory([FromRoute] int categoryId)
        {
            try
            {
                var res = await _categoryService.RemoveCategory(categoryId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }
        [AllowAnonymous]
        //Unit
        [HttpGet("units")]
        public async Task<IActionResult> GetAllUnits()
        {
            try
            {
                var res = await _unitService.GetAllUnits();
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [AllowAnonymous]
        [HttpGet("units/{unitId}")]
        public async Task<IActionResult> GetUnitById([FromRoute] int unitId)
        {
            try
            {
                var res = await _unitService.GetUnitById(unitId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }

        [HttpPost("units/add")]
        public async Task<IActionResult> AddUnit([FromBody] UnitDTO model)
        {
            if (model.UnitName.Length < 2) return BadRequest("Unit name must be greater than 1 character");
            try
            {
                var res = await _unitService.AddUnit(model.UnitName);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }


        [HttpPut("units/edit/{unitId}")]
        public async Task<IActionResult> EditUnit([FromRoute] int unitId, [FromBody] UnitDTO model)
        {
            if (model.UnitName.Length < 2) return BadRequest("Unit name must be greater than 1 character");
            try
            {
                var res = await _unitService.EditUnit(unitId, model.UnitName);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }


        [HttpDelete("units/remove/{unitId}")]
        public async Task<IActionResult> RemoveUnit([FromRoute] int unitId)
        {
            try
            {
                var res = await _unitService.RemoveUnit(unitId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        //Collection
        [AllowAnonymous]
        [HttpGet("collections")]
        public async Task<IActionResult> GetAllCollections()
        {
            try
            {
                var res = await _collectionService.GetAllCollections();
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }
        [AllowAnonymous]
        [HttpGet("collections/{collectionId}")]
        public async Task<IActionResult> GetCollectionById([FromRoute] int collectionId)
        {
            try
            {
                var res = await _collectionService.GetCollectionById(collectionId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }


        [HttpPost("collections/add")]
        public async Task<IActionResult> AddCollection([FromBody] AddCollectionDTO model)
        {
            if (model.CollectionName.Length < 2) return BadRequest("Collection name must be greater than 1 character");
            try
            {
                var res = await _collectionService.AddCollection(model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }

        [HttpPut("collections/edit/{collectionId}")]
        public async Task<IActionResult> EditCollection([FromRoute] int collectionId, [FromBody] AddCollectionDTO model)
        {
            if (model.CollectionName.Length < 2) return BadRequest("Collection name must be greater than 1 character");
            try
            {
                var res = await _collectionService.EditCollection(collectionId, model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }


        [HttpDelete("collections/remove/{collectionId}")]
        public async Task<IActionResult> RemoveCollection([FromRoute] int collectionId)
        {
            try
            {
                var res = await _collectionService.RemoveCollection(collectionId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        //Color
        [AllowAnonymous]
        [HttpGet("colors")]
        public async Task<IActionResult> GetAllColors()
        {
            try
            {
                var res = await _colorService.GetAllColors();
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [AllowAnonymous]
        [HttpGet("colors/{colorId}")]
        public async Task<IActionResult> GetColorById([FromRoute] int colorId)
        {
            try
            {
                var res = await _colorService.GetColorById(colorId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }

        [HttpPost("colors/add")]
        public async Task<IActionResult> AddColor([FromBody] ColorDTO model)
        {
            if (model.ColorName.Length < 2) return BadRequest("Color name must be greater than 1 character");
            try
            {
                var res = await _colorService.AddColor(model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }


        [HttpPut("colors/edit/{colorId}")]
        public async Task<IActionResult> EditColor([FromRoute] int colorId, [FromBody] ColorDTO model)
        {
            if (model.ColorName.Length < 2) return BadRequest("Color name must be greater than 1 character");
            try
            {
                var res = await _colorService.EditColor(colorId, model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }


        [HttpDelete("colors/remove/{colorId}")]
        public async Task<IActionResult> RemoveColor([FromRoute] int colorId)
        {
            try
            {
                var res = await _colorService.RemoveColor(colorId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        //Products
        [AllowAnonymous]
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts([Required] int page)
        {
            try
            {
                var res = await _productService.GetAllProducts(page);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }
        [AllowAnonymous]
        [HttpGet("products/{productId}")]
        public async Task<IActionResult> GetProductById([FromRoute] int productId)
        {
            try
            {
                var res = await _productService.GetProductById(productId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpPost("products/add")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO model)
        {
            Category categoryExist = await _dbContext.Categories.FindAsync(model.CategoryId);
            if (categoryExist == null) return NotFound(new Response("Error", $"The category with = {model.CategoryId} was not found"));
            foreach (int collectionId in model.CollectionIdList)
            {
                Collection collectionExist = await _dbContext.Collections.FindAsync(collectionId);
                if (collectionExist == null && collectionId != 0) return NotFound(new Response("Error", $"The collect with = {collectionId} was not found"));
            }
       
            try
            {
                var res = await _productService.AddProduct(model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }

        [HttpPut("products/edit/{productId}")]
        public async Task<IActionResult> EditProduct([FromRoute] int productId, [FromBody] ProductDTO model)
        {
            Product productExist = await _dbContext.Products.FindAsync(productId);
            if (productExist == null) return NotFound(new Response("Error", $"The product with = {productId} was not found"));
            Category categoryExist = await _dbContext.Categories.FindAsync(model.CategoryId);
            if (categoryExist == null) return NotFound(new Response("Error", $"The category with = {model.CategoryId} was not found"));
            foreach (int collectionId in model.CollectionIdList)
            {
                Collection collectionExist = await _dbContext.Collections.FindAsync(collectionId);
                if (collectionExist == null && collectionId != 0) return NotFound(new Response("Error", $"The collect with = {collectionId} was not found"));
            }
            if (!model.ProductCode.IsNullOrEmpty())
            {
                var productCodeExist = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode.ToUpper().Equals(model.ProductCode.ToUpper()));
                if (productCodeExist != null && !productCodeExist.ProductCode.Equals(model.ProductCode)) return BadRequest(new Response("Error", $"The product code already exist"));
            }
            try
            {
                var res = await _productService.EditProduct(productExist, model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }

        [HttpDelete("products/remove/{productId}")]
        public async Task<IActionResult> RemoveProduct([FromRoute] int productId)
        {
            try
            {
                var res = await _productService.RemoveProduct(productId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        //models
        [AllowAnonymous]
        [HttpGet("products/{productId}/models")]
        public async Task<IActionResult> GetAllModelsByProductId([FromRoute] int productId)
        {
            try
            {
                var res = await _productService.GetAllModelsByProductId(productId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [AllowAnonymous]
        [HttpGet("products/{productId}/models/{modelId}")]
        public async Task<IActionResult> GetModelByIds([FromRoute] int productId, [FromRoute] int modelId)
        {

            try
            {
                var res = await _productService.GetModelByIds(productId, modelId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }


        [HttpPost("products/{productId}/models/add")]
        public async Task<IActionResult> AddModel([FromRoute] int productId, [FromBody] ModelDTO model)
        {
            Product productExist = await _dbContext.Products.FindAsync(productId);
            if (productExist == null) return NotFound(new Response("Error", $"The product with = {productId} was not found"));
            Unit unitExist = await _dbContext.Units.FindAsync(model.UnitId);
            if (unitExist == null) return NotFound(new Response("Error", $"The unit with = {model.UnitId} was not found"));
            if (model.ColorId != null && model.ColorId != 0)
            {
                Color colorExist = await _dbContext.Colors.FindAsync(model.ColorId);
                if (colorExist == null) return NotFound(new Response("Error", $"The color with = {model.ColorId} was not found"));
            }
            try
            {
                var res = await _productService.AddModel(productId, model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }


        [HttpPut("products/{productId}/models/edit/{modelId}")]
        public async Task<IActionResult> EditModel([FromRoute] int productId, [FromRoute] int modelId, [FromBody] ModelDTO model)
        {

            Product productExist = await _dbContext.Products.FindAsync(productId);
            if (productExist == null) return NotFound(new Response("Error", $"The product with = {productId} was not found"));
            Model modelExist = productExist.Models.FirstOrDefault(m => m.ModelId == modelId);
            if (modelExist == null) return NotFound(new Response("Error", $"The product with = {productId} does not contain any model with id = {modelId}"));
            Unit unitExist = await _dbContext.Units.FindAsync(model.UnitId);
            if (unitExist == null) return NotFound(new Response("Error", $"The unit with = {model.UnitId} was not found"));
            if (model.ColorId != null && model.ColorId != 0)
            {
                Color colorExist = await _dbContext.Colors.FindAsync(model.ColorId);
                if (colorExist == null) return NotFound(new Response("Error", $"The color with = {model.ColorId} was not found"));
            }
            try
            {
                var res = await _productService.EditModel(productId, modelExist, model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }


        [HttpDelete("products/{productId}/models/{modelId}/remove")]
        public async Task<IActionResult> RemoveModel([FromRoute] int productId, [FromRoute] int modelId)
        {

            try
            {
                var res = await _productService.RemoveModel(productId, modelId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        //Post
        [AllowAnonymous]
        [HttpGet("blogs")]
        public async Task<IActionResult> GetAllPosts([Required] int page)
        {
            try
            {
                var res = await _postService.GetAllPosts(page);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }
        [AllowAnonymous]
        [HttpGet("blogs/{postId}")]
        public async Task<IActionResult> GetPostById([FromRoute] int postId)
        {
            try
            {
                var res = await _postService.GetPostById(postId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpPost("blogs/add")]
        public async Task<IActionResult> AddPost([FromBody] PostDTO model)
        {
            try
            {
                var loggedInUser = await _authService.GetCurrentLoggedInUser();
                if (loggedInUser == null) return Unauthorized();
                var res = await _postService.AddPost(loggedInUser.Id, model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpPut("blogs/edit/{postId}")]
        public async Task<IActionResult> EditPost([FromRoute] int postId, [FromBody] PostDTO model)
        {
            var postExist = await _dbContext.Posts.FindAsync(postId);
            if (postExist == null) return NotFound(new Response("Error", $"The post with id = {postId} was not found"));
            try
            {
                var res = await _postService.EditPost(postExist, model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpDelete("blogs/remove/{postId}")]
        public async Task<IActionResult> RemovePost([FromRoute] int postId)
        {
            try
            {
                var res = await _postService.RemovePost(postId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        ////warehouse
        [HttpGet("warehouse-logs")]
        public async Task<IActionResult> GetAllWarehouseLogs([Required] int page)
        {
            try
            {
                var res = await _warehouseLogService.GetAllWarehouseLogs(page);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpGet("warehouse-logs/{warehouseLogId}")]
        public async Task<IActionResult> GetWarehouseLogById([FromRoute] int warehouseLogId)
        {
            try
            {
                var res = await _warehouseLogService.GetWarehouseLogById(warehouseLogId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpPost("warehouse-logs/add")]
        public async Task<IActionResult> AddWarehouse([FromBody] WarehouseLogDTO model)
        {
            Model modelExist = await _dbContext.Models.FindAsync(model.ModelId);
            if (modelExist == null) return NotFound(new Response("Error", $"The model with id = {model.ModelId} was not found"));
            try
            {
                var loggedInUser = await _authService.GetCurrentLoggedInUser();
                if (loggedInUser == null) return Unauthorized();
                var res = await _warehouseLogService.AddWarehouseLog(loggedInUser.Id, modelExist, model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        //order
        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var res = await _orderService.GetAllUserOrders();
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpPut("orders/change-status")]
        public async Task<IActionResult> ChangeOrderStatus([FromBody] ChangeOrderStatusDTO model)
        {
            try
            {
                var res = await _orderService.UpdateOrderStatus(model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occur when handle request"));
            }
        }

        //Users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUser(int page)
        {
            try
            {
                var res = await _userService.GetAllUsers(page);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute] string userId)
        {
            try
            {
                var res = await _userService.GetUserById(userId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpPut("users/edit/{userId}")]
        public async Task<IActionResult> EditUser([FromRoute] string userId, [FromBody] UserDTO model)
        {

            try
            {
                var res = await _userService.EditUser(userId, model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [Authorize(Roles = "ADMIN,CUSTOMER")]
        [HttpPut("users/change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordDTO model)
        {


            try
            {
                var currentUser = await _authService.GetCurrentLoggedInUser();
                if (currentUser == null) return Unauthorized();
                var res = await _userService.ChangePassword(currentUser, model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpPut("users/toggle-user-status/{userId}")]
        public async Task<IActionResult> ToggleUserStatus([FromRoute] string userId)
        {
            try
            {

                var res = await _userService.ToggleUserStatus(userId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpPut("users/change-role")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeRoleDTO request)
        {
            try
            {

                var res = await _userService.ChangeUserRole(request);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        //Enqiuries
        [HttpGet("enquiries")]
        public async Task<IActionResult> GetAllEnquiries(int page)
        {
            try
            {
                var res = await _enquiryService.GetAllEnquiries(page);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpGet("enquiries/{enquiryId}")]
        public async Task<IActionResult> GetEnquiryById(string enquiryId)
        {
            try
            {
                var res = await _enquiryService.GetEnquiryById(enquiryId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpDelete("enquiries/remove/{enquiryId}")]
        public async Task<IActionResult> RemoveEnquiry([FromRoute] string enquiryId)
        {
            try
            {
                var res = await _enquiryService.RemoveEnquiry(enquiryId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        //feedback
        [HttpDelete("feedbacks/remove/{feedbackId}")]
        public async Task<IActionResult> RemoveFeedback([FromRoute] int feedbackId)
        {
            try
            {
                var res = await _feedbackService.RemoveFeedback(feedbackId);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }
    }
}
