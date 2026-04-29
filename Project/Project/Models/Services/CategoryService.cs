using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Project.Models.Entities;

namespace Project.Models.Services
{
    public class CategoryService
    {
        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            using var db = new AppDbContext();
            return await db.Categories
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Color = c.Color
                })
                .ToListAsync();
        }
    }
}
