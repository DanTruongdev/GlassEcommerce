using GlassECommerce.DTOs;
using GlassECommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface IPostService
    {
        public Task<IActionResult> GetAllPosts(int page);

        public Task<IActionResult> GetPostById(int postId);
        public Task<IActionResult> AddPost(string userId, PostDTO model);

        public Task<IActionResult> EditPost(Post postExist, PostDTO model);

        public Task<IActionResult> RemovePost(int postId);
    }
}
