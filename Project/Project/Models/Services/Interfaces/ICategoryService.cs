using DAL.Enums;
using Project.Models.Entities;

namespace Project.Models.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync(int userId);
    Task<CategoryDto> AddAsync(string name, string color, TransactionType type, int userId);
    Task UpdateAsync(int id, string name, string color);
    Task DeleteAsync(int id);
    Task<bool> HasTransactionsAsync(int id);
}