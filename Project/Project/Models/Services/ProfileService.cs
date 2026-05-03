using System;
using System.Threading.Tasks;
using Project.Models.Entities;
using Project.Models.Services.Interfaces;

namespace Project.Models.Services;

public class ProfileService : IProfileService
{
    public Task<UserDto> GetProfileAsync(int userId)
        => throw new NotImplementedException();

    public Task UpdateUsernameAsync(int userId, string username)
        => throw new NotImplementedException();

    public Task UpdateEmailAsync(int userId, string email)
        => throw new NotImplementedException();

    public Task ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        => throw new NotImplementedException();
}
