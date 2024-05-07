using GlassECommerce.DTOs;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;


        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authService = authenticationService;

        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO model)
        {
            try
            {
                var res = await _authService.SignUp(model);
                return res;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {

            try
            {
                var res = await _authService.Login(model);
                return res;

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([Required] string email)
        {

            try
            {
                var res = await _authService.ForgetPassword(email);
                return res;

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {

            try
            {
                var res = await _authService.ResetPassword(model);
                return res;

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occur when handle request"));
            }
        }



    }
}
