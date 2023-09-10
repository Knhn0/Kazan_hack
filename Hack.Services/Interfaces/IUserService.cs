using Hack.Domain.Dto;
using Hack.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hack.Services.Interfaces;

public interface IUserService
{
    UserManager<User> GetUserManager();
    Task ChangeFirstname(Guid id, string firstname);
    Task ChangeLastname(Guid id, string lastname);
    Task<string> GetFirstName(Guid id);
    Task<string> GetLastName(Guid id);
    Task<int> GetPoints(Guid id);
    Task SetPoints(Guid id, int points);
    Task<List<int>?> GetMarksDiscovered(Guid id);
    Task<List<User>> OrderByHighestPoints(int amount, int offset = 0);
    Task<List<User>> OrderByMostDiscoveries(int amount, int offset = 0);
    Task<bool> IsMarkDiscovered(Guid userId, int markId);
    Task<User> FullyUserUpdate(Guid id, User user);
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> DiscoverMark(string username, int markId);
}