using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloggingApp.Domain.Entities;

namespace BloggingApp.Application.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetAllAsync(string? sortBy = null, string? search = null);
        Task<Post?> GetByIdAsync(Guid id);
        Task CreateAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(Guid id);
    }
}