using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BloggingApp.Application.Interfaces;
using BloggingApp.Domain.Entities;
using BloggingApp.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BloggingApp.Web.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly IPostService _postService;
        private readonly UserManager<AppUser> _userManager;

        public PostController(ILogger<PostController> logger, IPostService postService, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _postService = postService;
            _userManager = userManager;
        }

        // GET: /Posts
        [HttpGet]
        public async Task<IActionResult> Index(string? sortBy, string? search)
        {
            var posts = await _postService.GetAllAsync(sortBy, search);
            return View(posts);
        }

        // GET: /Posts/Details/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            return View(post);
        }

        // GET: /Posts/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Posts/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (!ModelState.IsValid)
                return View(post);

            var user = await _userManager.GetUserAsync(User);
            post.UserId = user.Id;

            await _postService.CreateAsync(post);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Posts/Edit/{id}
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            return View(post);
        }

        // POST: /Posts/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(post);

            post.UpdatedAt = DateTime.UtcNow;
            await _postService.UpdateAsync(post);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Posts/Delete/{id}
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            return View(post);
        }

        // POST: /Posts/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}