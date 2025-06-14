using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloggingApp.Application.Interfaces;
using BloggingApp.Domain.Entities;
using BloggingApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BloggingApp.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetAllAsync(string? sortBy = null, string? search = null)
        {
            var query = _context.Posts
                        .Where(p => !p.IsDeleted)
                        .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Title.Contains(search));

            return sortBy switch
            {
                "title" => await query.OrderBy(p => p.Title).ToListAsync(),
                "date" => await query.OrderByDescending(p => p.CreatedAt).ToListAsync(),
                _ => await query.OrderByDescending(p => p.CreatedAt).ToListAsync()
            };
        }

        public async Task<Post?> GetByIdAsync(Guid id) =>
            await _context.Posts.FirstOrDefaultAsync(p => p.Id == id.ToString() && !p.IsDeleted);

        public async Task CreateAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                post.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}