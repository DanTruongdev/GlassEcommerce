using GlassECommerce.DTOs;

using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace GlassECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        public readonly IEnquiryService _enquiryService;

        public GuestController(IEnquiryService enquiryService)
        {
            _enquiryService = enquiryService;
        }

        [HttpPost("enquiries/add")]
        public async Task<IActionResult> AddEnquiry(EnquiryDTO model)
        {
            try
            {
                var res = await _enquiryService.AddEnquiry(model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                 new Response("Error", "An error occur when handle request"));
            }
        }
    }
}
