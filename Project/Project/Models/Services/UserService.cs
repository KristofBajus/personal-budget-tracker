using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Models.Entities;
using Project.Models.Services.Interfaces;

namespace Project.Models.Services;

public class UserService : IUserService
{
    public Task<List<UserDto>> GetAllAsync()
        => throw new NotImplementedException();

    public Task BanAsync(int userId)
        => throw new NotImplementedException();

    public Task UnbanAsync(int userId)
        => throw new NotImplementedException();

    public Task DeleteAsync(int userId)
        => throw new NotImplementedException();
}
