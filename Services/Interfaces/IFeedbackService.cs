using GlassECommerce.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface IFeedbackService
    {
        public Task<IActionResult> GetFeedbackByProductId(int productId);
        public Task<IActionResult> AddFeedback(FeedbackDTO model);
        public Task<IActionResult> EditFeedback(int feedbackId, EditFeedbackDTO model);
        public Task<IActionResult> RemoveFeedback(int feedbackId);

    }
}
