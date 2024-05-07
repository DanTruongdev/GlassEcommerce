using GlassECommerce.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface IEnquiryService
    {
        public Task<IActionResult> GetAllEnquiries(int page);
        public Task<IActionResult> GetEnquiryById(string enquiryId);
        public Task<IActionResult> AddEnquiry(EnquiryDTO model);
        public Task<IActionResult> RemoveEnquiry(string enquiryId);

    }
}
