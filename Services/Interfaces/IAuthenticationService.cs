using GlassECommerce.DTOs;
using GlassECommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<IActionResult> Login(LoginDTO model);
        public Task<IActionResult> SignUp(SignUpDTO model);
        public Task<IActionResult> ForgetPassword(string email);
        public Task<IActionResult> ResetPassword(ResetPasswordDTO model);
        public Task<User> GetCurrentLoggedInUser();


        //public Task<ServiceResponse<string>> ResetPassword(ResetPassword model);



    }
}
