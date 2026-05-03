using System.Threading.Tasks;
using Project.Models.Entities;

namespace Project.Models.Services.Interfaces;

public interface IProfileService
{
    Task<UserDto> GetProfileAsync(int userId);
    Task UpdateUsernameAsync(int userId, string username);
    Task UpdateEmailAsync(int userId, string email);
    Task ChangePasswordAsync(int userId, string currentPassword, string newPassword);
}
