using Hack.Domain.Dto;
using Hack.Domain.Entites;
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
}