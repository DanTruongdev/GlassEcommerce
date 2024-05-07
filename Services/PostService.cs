using Castle.Core.Internal;
using GlassECommerce.Common;
using GlassECommerce.Data;
using GlassECommerce.DTOs;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Encodings.Web;
using System.Web;

namespace GlassECommerce.Services
{
    public class PostService : ControllerBase, IPostService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly IAuthenticationService _authService;

        public PostService(ApplicationDbContext dbContext, IAuthenticationService authService, HtmlEncoder htmlEncoder)
        {
            _dbContext = dbContext;
            _authService = authService;
            _htmlEncoder = htmlEncoder;
        }

        public async Task<IActionResult> GetAllPosts(int page)
        {
            int pageSize = 10;
            var skipAmount = pageSize * (page - 1);

            var postsQuery = _dbContext.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreationDate)
                .AsQueryable();

            if (page > 0)
            {
                postsQuery = postsQuery.Skip(skipAmount).Take(pageSize);
            }

            var posts = await postsQuery.ToListAsync();

            if (posts.IsNullOrEmpty())
                return Ok(new List<Post>());

            var response = posts.Select(p => new
            {
                PostId = p.PostId,
                CreatedBy = p.User.ToString(),
                CreationDate = p.CreationDate,
                LastestUpdate = p.LatestUpdate,
                Title = p.Title,
                Content = CompressionHelper.DecompressString(p.Content),
                Thumbnail = p?.Thumbnail
            });

            return Ok(response);
        }

        public async Task<IActionResult> GetPostById(int postId)
        {
            var post = await _dbContext.Posts.FindAsync(postId);
            if (post == null) return NotFound(new Response("Error", $"The post with id = {postId} was not found"));
            return Ok(new
            {
                PostId = post.PostId,
                CreatedBy = post.User.ToString(),
                CreationDate = post.CreationDate,
                LastestUpdate = post.LatestUpdate,
                Title = post.Title,
                Content = CompressionHelper.DecompressString(post.Content),
                Thumbnail = post?.Thumbnail

            });
        }

        public async Task<IActionResult> AddPost(string userId, PostDTO model)
        {
            try
            {
                string encodedContent = CompressionHelper.CompressString(model.Description);
                Post newPost = new Post()
                {
                    UserId = userId,
                    Title = model.Title,
                    Content = encodedContent,
                    Thumbnail = model.Thumbnail == null || model.Thumbnail.Count() > 0 ? model.Thumbnail : "",
                    CreationDate = DateTime.Now,
                    LatestUpdate = DateTime.Now
                };
                await _dbContext.AddAsync(newPost);
                await _dbContext.SaveChangesAsync();
                return Created("New post created", new
                {
                    PostId = newPost.PostId,
                    CreateBy = newPost.User.ToString(),
                    Thumbnail = newPost.Thumbnail,
                    Title = newPost.Title,
                    Content = CompressionHelper.DecompressString(newPost.Content),
                    CreationDate = newPost.CreationDate,
                    LatestUpdate = newPost.LatestUpdate
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   new Response("Error", "An error occurs when adding new post"));
            }
        }

        public async Task<IActionResult> EditPost(Post postExist, PostDTO model)
        {

            try
            {
                postExist.Thumbnail = model.Thumbnail == null || model.Thumbnail.Count() > 0 ? model.Thumbnail : "";
                postExist.Title = model.Title;
                postExist.Content = CompressionHelper.CompressString(model.Description); 
                postExist.LatestUpdate = DateTime.Now;
                _dbContext.Update(postExist);
                await _dbContext.SaveChangesAsync();
                return Ok(new
                {
                    PostId = postExist.PostId,
                    CreatedBy = postExist.User.ToString(),
                    Title = postExist.Title,
                    Content = CompressionHelper.DecompressString(postExist.Content),
                    CreationDate = postExist.CreationDate,
                    LatestUpdate = postExist.LatestUpdate
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   new Response("Error", "An error occurs when editing post"));
            }
        }

        public async Task<IActionResult> RemovePost(int postId)
        {
            try
            {  
                Post postExist = await _dbContext.Posts.FindAsync(postId);
                if (postExist == null) return NotFound(new Response("Error", $"The post with id = {postId} was not found"));
                _dbContext.Remove(postExist);
              
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  new Response("Error", "An error occurs when removing post"));
            }
        }

    }
}
