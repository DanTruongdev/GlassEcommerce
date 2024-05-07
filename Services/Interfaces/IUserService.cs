using GlassECommerce.DTOs;
using GlassECommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface IUserService
    {
        public Task<IActionResult> GetAllUsers(int page);
        public Task<IActionResult> GetUserById(string userId);
        public Task<IActionResult> EditUser(string userId, UserDTO model);
        public Task<IActionResult> ChangePassword(User userExist, PasswordDTO model);
        public Task<IActionResult> ToggleUserStatus(string userId);
        public Task<IActionResult> ChangeUserRole(ChangeRoleDTO model);

    }
}
