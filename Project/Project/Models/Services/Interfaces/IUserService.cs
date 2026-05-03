using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Models.Entities;

namespace Project.Models.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task BanAsync(int userId);
    Task UnbanAsync(int userId);
    Task DeleteAsync(int userId);
}
