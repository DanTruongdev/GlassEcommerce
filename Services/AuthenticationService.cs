using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using GlassECommerce.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GlassECommerce.Services
{
    public class AuthenticationService : ControllerBase, IAuthenticationService
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                //generate a reset password token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = $"http://localhost:8080/newpassword?email={email}&token={token}";
                var message = new Message(new string[] { user.Email }, "Reset password link:", $"Please click this url to reset your password:" + forgotPasswordLink);
                _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK,
                   new Response("Success", $"Reset password link has sent to  {user.Email} successfully"));
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                   new Response("Error", "This email is not linked to any account"));
        }

        public async Task<IActionResult> Login(LoginDTO model)
        {
            var loggedInUser = await _userManager.FindByEmailAsync(model.Email);
            var passwordChecker = await _userManager.CheckPasswordAsync(loggedInUser, model.Password);
            if (loggedInUser == null || !passwordChecker) return Unauthorized(new Response("Error", "Invalid email or password"));
            if (!loggedInUser.IsActivated) return StatusCode(StatusCodes.Status405MethodNotAllowed, new Response("Error", "Inactive accounts are not allowed to log in"));
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(loggedInUser, model.Password, true, true);
            var role = await _userManager.GetRolesAsync(loggedInUser);
            var jwtToken = GenerateJWTToken(loggedInUser, role);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = jwtToken.ValidTo,
                role = role.First(),
                FullName = loggedInUser.ToString()
            });
        }

        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return BadRequest(new Response("Error", "This email is not linked to any account"));
            var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (resetPassResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK,
                       new Response("Success", "Reset password successfully"));
            }
            else return BadRequest(new Response("Error", "Invalid token"));
        }

        public async Task<IActionResult> SignUp(SignUpDTO model)
        {
            User userExist = await _userManager.FindByEmailAsync(model.Email);
            if (userExist != null) return BadRequest(new Response("Error", "The email already exist"));
            User newUser = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                IsActivated = true
            };
            var addUserResult = await _userManager.CreateAsync(newUser, model.Password);
            if (!addUserResult.Succeeded) return BadRequest(new Response("Error", addUserResult.ToString()));
            var addRoleResult = await _userManager.AddToRolesAsync(newUser, new[] { "CUSTOMER" });
            if (!addUserResult.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error", "An error occurs when adding role to new account"));
            return Ok(new Response("Success", $"Create new account with id = {newUser.Id} successfull"));
        }

        public async Task<User> GetCurrentLoggedInUser()
        {
            try
            {
                ClaimsPrincipal currentUser = _httpContextAccessor.HttpContext.User;
                string userEmail = currentUser.FindFirst(ClaimTypes.Email)?.Value;
                var loggedInUser = await _userManager.FindByEmailAsync(userEmail);
                return loggedInUser;
            }
            catch
            {
                return null;
            }

        }
        private JwtSecurityToken GenerateJWTToken(User user, IList<string> roles)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

    }
}
