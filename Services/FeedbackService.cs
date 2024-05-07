using Castle.Core.Internal;
using GlassECommerce.Common;
using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlassECommerce.Services
{
    public class FeedbackService : ControllerBase, IFeedbackService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthenticationService _authService;

        public FeedbackService(ApplicationDbContext dbContext, IAuthenticationService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<IActionResult> GetFeedbackByProductId(int productId)
        {
            var feedback = await _dbContext.Feedbacks.Where(f => f.ProductId == productId).AsNoTracking().OrderByDescending(f => f.CreationDate).ToListAsync();
            if (!feedback.Any()) return Ok(new List<Feedback>());
            return Ok(feedback.Select(f => new
            {
                FeedbackId = f.FeedbackId,
                User = f.IsAnonymous ? "Ẩn danh" : f.User.ToString() ,
                Title = f.Title,
                Content = f.Content,
                Star = f.Star,
                CreationDate = f.CreationDate,
            }));
        }

        public async Task<IActionResult> AddFeedback(FeedbackDTO model)
        {
            User userExist = await _authService.GetCurrentLoggedInUser();
            if (userExist == null) return Unauthorized();
            Order userOrder = userExist?.Orders?.FirstOrDefault(o => o.OrderId == model.OrderId);
            if (userOrder == null) return BadRequest(new Response("Error", $"There is no order with id = {model.OrderId} in user's order list"));
            if (!userOrder.OrderStatus.Equals(OrderStatus.DELIVERED)) return BadRequest(new Response("Error", "The order status must be \"Delivered\" to give feedback"));
            if (userOrder.DeliveredDate?.AddMonths(1) < DateTime.Now) return BadRequest(new Response("Error", $"Feedback can only be added after 1 month from the date of delivery"));
            var feedbackExist = await _dbContext.Feedbacks.FirstOrDefaultAsync(f => f.OrderId == model.OrderId && f.ProductId == model.ProductId);
            //if (feedbackExist != null) return BadRequest(new Response("Error", "This product was added feedback"));
            var orderItemExist = userOrder.OrderItems.FirstOrDefault(oi => oi.Model.Product.ProductId == model.ProductId);
            if (orderItemExist == null) return BadRequest(new Response("Error", $"There is no product with id = {model.ProductId} in user's order list"));
            Product productExist = orderItemExist.Model.Product;
            try
            {
                Feedback newFeedback = new Feedback()
                {
                    UserId = userExist!.Id,
                    OrderId = model.OrderId,
                    ProductId = model.ProductId,
                    Title = model.Title,
                    Content = model.Content,
                    Star = model.Star,
                    IsAnonymous = model.IsAnonymous,
                    AllowEdit = true,
                    CreationDate = DateTime.Now
                };
                await _dbContext.AddAsync(newFeedback);
                await _dbContext.SaveChangesAsync();
               
                var relatedFeedbacks = await _dbContext.Feedbacks.Where(f => f.ProductId == model.ProductId).ToListAsync();
                if (relatedFeedbacks.IsNullOrEmpty()) productExist.VoteStar = model.Star;
                else
                {
                    var totalStar = relatedFeedbacks.Sum(f => f.Star);
                    var totalVote = relatedFeedbacks.Count();
                    productExist.VoteStar = Math.Round((double)totalStar / totalVote, 1);
                }
                _dbContext.Update(productExist);
                await _dbContext.SaveChangesAsync();
                return Created("new feedback created", new
                {   
                    FeedbackId = newFeedback.FeedbackId,
                    User = newFeedback.IsAnonymous ? "Anonymous" : newFeedback.User.ToString(),
                    OrderId = newFeedback.OrderId,
                    ProductId = newFeedback.ProductId,
                    Title = newFeedback.Title,
                    Content = newFeedback.Content,
                    Star = newFeedback.Star,
                    CreationDate = newFeedback.CreationDate,
                });

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                 new Response("Error", "An error occurs when adding feedback"));
            }
        }

        public async Task<IActionResult> EditFeedback(int feedbackId, EditFeedbackDTO model)
        {
            User userExist = await _authService.GetCurrentLoggedInUser();
            if (userExist == null) return Unauthorized();
            //if (userExist.Feedbacks.Any()) return BadRequest(new Response("Error", "The user has no any feedback"));
            Feedback userFeedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(f => f.FeedbackId == feedbackId && f.UserId.Equals(userExist.Id));
            if (userFeedback == null) return NotFound(new Response("Error", $"The user has no feedback with {feedbackId}"));
            //if (!userFeedback.AllowEdit) return BadRequest(new Response("Error", "The feedback can be edited one time only"));
            if (userFeedback.CreationDate.AddMonths(1) < DateTime.Now) return BadRequest(new Response("Error", $"Feedback can only be edited after 1 month from creation date"));
            Product productExist = userFeedback.Product;
            try
            {
                userFeedback.Title = model.Title;
                userFeedback.Content = model.Content;
                userFeedback.Star = model.Star;
                userFeedback.IsAnonymous = model.IsAnonymous;
                userFeedback.AllowEdit = false;

                _dbContext.Update(userFeedback);
                await _dbContext.SaveChangesAsync();
 
                var relatedFeedbacks = await _dbContext.Feedbacks.Where(f => f.ProductId == productExist.ProductId).ToListAsync();
                if (relatedFeedbacks.IsNullOrEmpty()) productExist.VoteStar = model.Star;
                else
                {
                    var totalStart = relatedFeedbacks.Sum(f => f.Star);
                    var totalVote = relatedFeedbacks.Count();
                    productExist.VoteStar = Math.Round((double)totalStart / totalVote, 1);
                }
                _dbContext.Update(productExist);
                await _dbContext.SaveChangesAsync();
                return Ok(new
                {
                    FeedbackId = userFeedback.FeedbackId,
                    User = userFeedback.IsAnonymous ? "Anonymous" : userFeedback.User.ToString(),
                    OrderId = userFeedback.OrderId,
                    ProductId = userFeedback.ProductId,
                    Title = userFeedback.Title,
                    Content = userFeedback.Content,
                    Star = userFeedback.Star,
                    CreationDate = userFeedback.CreationDate,
                });

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                 new Response("Error", "An error occurs when adding feedback"));
            }
        }
        //feedback
        public async Task<IActionResult> RemoveFeedback(int feedbackId)
        {
            try
            {
                Feedback feedbackExist = await _dbContext.Feedbacks.FindAsync(feedbackId);
                if (feedbackExist == null) return NotFound(new Response("Error", $"The feedback with id = {feedbackId} was not found"));
                var productExist = feedbackExist.Product;
                var relatedFeedbacks = await _dbContext.Feedbacks.Where(f => f.ProductId == productExist.ProductId).ToListAsync();
                var totalStart = relatedFeedbacks.Sum(f => f.Star) - feedbackExist.Star;
                var totalVote = relatedFeedbacks.Count() - 1;
                productExist.VoteStar = Math.Round((double)totalStart / totalVote, 1);
                _dbContext.Update(productExist);
                _dbContext.Remove(feedbackExist);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response("Error", "An error occurs when removing feedback"));
            }
        }
    }
}
