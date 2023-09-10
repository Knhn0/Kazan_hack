using System.Security.Claims;
using Hack.Services.Interfaces;
using Hack.Domain.Entities;
using Hack.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hack.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public UserManager<User> GetUserManager() => _userManager;

    public async Task ChangeFirstname(Guid id, string firstname)
    {
        var candidate = await _userManager.FindByIdAsync(id.ToString());
        if (candidate == null) throw new Exception("User not found.");
        candidate.Firstname = firstname;
        await _userManager.UpdateAsync(candidate);
    }

    public async Task ChangeLastname(Guid id, string lastname)
    {
        var candidate = await _userManager.FindByIdAsync(id.ToString());
        if (candidate == null) throw new Exception("User not found.");
        candidate.Firstname = lastname;
        await _userManager.UpdateAsync(candidate);
    }

    public async Task<string> GetFirstName(Guid id)
    {
        var candidate = await _userManager.FindByIdAsync(id.ToString());
        if (candidate == null) throw new Exception("User not found.");
        return candidate.Firstname;
    }

    public async Task<string> GetLastName(Guid id)
    {
        var candidate = await _userManager.FindByIdAsync(id.ToString());
        if (candidate == null) throw new Exception("User not found.");
        return candidate.Lastname;
    }

    public async Task<int> GetPoints(Guid id)
    {
        var candidate = await _userManager.FindByIdAsync(id.ToString());
        if (candidate == null) throw new Exception("User not found.");
        return candidate.Points;
    }

    public async Task SetPoints(Guid id, int points)
    {
        var candidate = await _userManager.FindByIdAsync(id.ToString());
        if (candidate == null) throw new Exception("User not found.");
        candidate.Points = points;
        await _userManager.UpdateAsync(candidate);
    }

    public async Task<List<int>?> GetMarksDiscovered(Guid id)
    {
        var candidate = await _userManager.FindByIdAsync(id.ToString());
        if (candidate == null) throw new Exception("User not found.");
        return candidate.MarksDiscovered;
    }

    public async Task<User> FullyUserUpdate(Guid id, User user)
    {
        var candidate = await _userManager.FindByIdAsync(id.ToString());
        if (candidate == null) throw new Exception("User not found.");
        
        if (!user.Firstname.IsNullOrEmpty()) candidate.Firstname = user.Firstname;

        if (!user.Lastname.IsNullOrEmpty()) candidate.Lastname = user.Lastname;
        await _userManager.UpdateAsync(candidate);
        return candidate;
    }

    public async Task<List<User>> OrderByHighestPoints(int amount, int offset = 0)
    {
        var list = await _userManager.Users.ToListAsync();
        offset = Math.Max(0, offset);
        amount = Math.Max(1, amount);
        var ordered = list.OrderByDescending(user => user.Points).Skip(offset).Take(amount).ToList();
        return ordered;
    }
    
    public async Task<List<User>> OrderByMostDiscoveries(int amount, int offset = 0)
    {
        var list = await _userManager.Users.ToListAsync();
        offset = Math.Max(0, offset);
        amount = Math.Max(1, amount);
        var ordered = list.OrderByDescending(user => user.MarksDiscovered!.Count).Skip(offset).Take(amount).ToList();
        return ordered;
    }

    public async Task<bool> IsMarkDiscovered(Guid userId, int markId)
    {
        var candidate = await _userManager.FindByIdAsync(userId.ToString());
        if (candidate == null) throw new Exception("User not found.");
        return candidate.MarksDiscovered!.Contains(markId);
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        var candidate = await _userManager.FindByNameAsync(username);
        if (candidate == null) throw new Exception("User not found.");
        return candidate;
    }

}