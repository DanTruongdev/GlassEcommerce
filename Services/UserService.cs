using Castle.Core.Internal;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GlassECommerce.Services
{
    public class UserService : ControllerBase, IUserService
    {
        private readonly UserManager<User> _userManager;
        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;

        }

        public async Task<IActionResult> GetAllUsers([Required] int page)
        {

            var pageSize = 20;
            var skipCount = page > 0 ? (page - 1) * pageSize : 0;

            var usersQuery = _userManager.Users
                .OrderBy(u => u.Id)  
                .Skip(skipCount)
                .Take(pageSize)
                .Select(u => new
                {
                    UserId = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault(),
                    Gender = u.Gender,
                    Dob = u.Dob,
                    Avatar = u.Avatar,
                    Address = u.Address,
                    IsActivated = u.IsActivated
                });
            var users = await usersQuery.ToListAsync();
            if (users.Count() == 0) return Ok(new List<User>());
            return Ok(users);
        }

        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new Response("Error", $"The user with id = {userId} was not found"));
            return Ok(new
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                Dob = user.Dob,
                Avatar = user.Avatar,
                Address = user.Address,
                IsActivated = user.IsActivated
            });
        }

        public async Task<IActionResult> EditUser(string userId, UserDTO model)
        {
            var userExist = await _userManager.FindByIdAsync(userId);
            if (userExist == null) return NotFound(new Response("Error", $"The user with id = {userId} was not found"));
            try
            {

                userExist.FirstName = model.FirstName;
                userExist.LastName = model.LastName;
                userExist.PhoneNumber = model?.PhoneNumber;
                userExist.Gender = model?.Gender;
                userExist.Dob = model?.Dob;
                userExist.Avatar = model?.Avatar;
                userExist.Address = model?.Address;
                await _userManager.UpdateAsync(userExist);
                return Ok(new
                {
                    UserId = userExist.Id,
                    FirstName = userExist.FirstName,
                    LastName = userExist.LastName,
                    Email = userExist.Email,
                    PhoneNumber = userExist.PhoneNumber,
                    Gender = userExist.Gender,
                    Dob = userExist.Dob,
                    Avatar = userExist.Avatar,
                    Address = userExist.Address,
                    IsActivated = userExist.IsActivated
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when update user"));
            }
        }

        public async Task<IActionResult> ChangePassword(User userExist, PasswordDTO model)
        {
            try
            {
                var checkPassResult = await _userManager.CheckPasswordAsync(userExist, model.OldPassword);
                if (!checkPassResult) return BadRequest(new Response("Error", "The old password is incorrect"));
                var changePassResult = await _userManager.ChangePasswordAsync(userExist, model.OldPassword, model.NewPassword);
                if (!changePassResult.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", changePassResult.ToString()));
                return Ok(new Response("Success", $"The user with id = {userExist.Id} was changed password successfully"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new Response("Error", "An error occurs when update user"));
            }
        }

        public async Task<IActionResult> ToggleUserStatus(string userId)
        {
            var userExist = await _userManager.FindByIdAsync(userId);
            if (userExist == null) return NotFound(new Response("Error", $"The user with id = {userId} was not found"));
            try
            {

                userExist.IsActivated = !userExist.IsActivated;
                string status = userExist.IsActivated ? "active" : "deactive";
                await _userManager.UpdateAsync(userExist);
                return Ok(new Response("Success", $"The user with id = {userId} was {status}"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response("Error", "An error occurs when update user"));
            }
        }
        public async Task<IActionResult> ChangeUserRole(ChangeRoleDTO model)
        {
            if (!model.RoleName.Equals("ADMIN") && !model.RoleName.Equals("CUSTOMER")) return BadRequest(new Response("Error", "The role must be \"ADMIN\" or \"CUSTOMER\""));
            var curentUser = await _userManager.FindByEmailAsync(model.UserEmail);
            if (curentUser == null) return NotFound(new Response("Error", $"{model.UserEmail} does not math any account"));
            var currentRoles = await _userManager.GetRolesAsync(curentUser);
            await _userManager.RemoveFromRolesAsync(curentUser, currentRoles);
            var result = await _userManager.AddToRoleAsync(curentUser, model.RoleName);
            return Ok(new Response("success","Change user role successfully"));
        }
    }
}
