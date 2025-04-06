using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Application.Contracts.Repositories.IUserService
{ }
public interface IRoleService
{
    Task<Role?> FindByNameAsync(string roleName);
    Task<IdentityResult> CreateRoleAsync(RoleDto role);
    Task<IdentityResult> DeleteRoleAsync(string roleName);
    Task<IList<Role>> GetAllRolesAsync();
    Task<bool> RoleExistsAsync(string roleName);
    Task AddClaimsToRole(User user, Role role, List<Claim> claims);

    Task<IEnumerable<Claim>> GetRoleClaimsAsync(string roleName);

    Task<IEnumerable<Claim>> GetAllUserClaimsAsync(string userId);
}

