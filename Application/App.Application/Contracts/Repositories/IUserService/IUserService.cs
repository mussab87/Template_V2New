using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Security.Claims;

namespace App.Application.Contracts.Repositories.IUserService
{ }
public interface IUserService
{
    Task<User?> GetUserByIdAsync(string userId);
    Task<User?> FindByNameAsync(string username);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<SignInResult> SignInAsync(User user, bool rememberMe, List<Claim> claims);
    Task SignOutAsync();
    Task<User> CreateUser(UserDto user, string password, string roleName);

    Task<List<UserPasswordLog>> GetUserPasswordLogsAsync(string userId);
    Task<UserPasswordLog> GetLastUserPasswordLogsAsync(string userId);

    Task<List<UserLoginLog>> GetUserLoginLogAsync(string userId);
    Task<bool> CreateUserLoginLogAsync(UserLoginLogDto userLoginLogDto);

    Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);

    Task<IdentityResult> UpdateUserAsync(User user);

    Task<bool> IsPasswordInRecentHistoryAsync(string userId, string oldPassword, string newPassword, int historyCount = 3);

    Task<LoginResult> ValidateLoginAsync(string username, string password);
    Task<bool> UnlockAccountAsync(string userId);

    Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    Task<IReadOnlyList<UserDto>> GetAllUsers();
    Task<PaginatedResult<UserDto>> GetPaginatedUsers(
        int pageNumber = 1,
        int pageSize = 10,
        string searchString = "",
        int sortColumn = 0,
        string sortDirection = "asc");
}

