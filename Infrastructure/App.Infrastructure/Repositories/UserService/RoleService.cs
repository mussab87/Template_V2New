using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace App.Infrastructure.Repositories.UserService
{ }
public class RoleService : IRoleService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly AppDbContext _dbContext;

    public RoleService(RoleManager<Role> roleManager, AppDbContext dbContext)
    {
        _roleManager = roleManager;
        _dbContext = dbContext;
    }

    public async Task<Role?> FindByNameAsync(string roleName)
    {
        return await _roleManager.FindByNameAsync(roleName);
    }

    public async Task<IdentityResult> CreateRoleAsync(RoleDto role)
    {
        if (await _roleManager.RoleExistsAsync(role.Name))
        {
            return IdentityResult.Failed(new IdentityError { Description = "Role already exists." });
        }

        var newRole = new Role()
        {
            Id = role.Id,
            Name = role.Name,
            RoleNameArabic = role.Name,
            CreatedById = role.CreatedById
        };
        return await _roleManager.CreateAsync(newRole);
    }

    public async Task<IdentityResult> DeleteRoleAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
            return IdentityResult.Failed(new IdentityError { Description = "Role not found." });

        return await _roleManager.DeleteAsync(role);
    }

    public async Task<IList<Role>> GetAllRolesAsync()
    {
        return await Task.FromResult(_roleManager.Roles.ToList());
    }

    public async Task AddClaimsToRole(User user, Role role, List<Claim> claims)
    {
        var roleClaims = new List<RoleClaim>();
        foreach (var clm in claims)
        {
            var roleClaim = new RoleClaim
            {
                RoleId = role.Id,
                ClaimType = clm.Value,
                ClaimValue = clm.Value,
                CreatedById = user.Id
            };
            roleClaims.Add(roleClaim);
        }
        // Add list of claims to role
        if (roleClaims.Count > 0)
        {
            await _dbContext.RoleClaims.AddRangeAsync(roleClaims);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    public async Task<IEnumerable<Claim>> GetRoleClaimsAsync(string roleName)
    {
        // First get the role ID
        var role = await FindByNameAsync(roleName);

        var roleId = role?.Id;

        if (roleId == null)
            return new List<Claim>();

        // Then get claims for this role
        var roleClaims = await _dbContext.RoleClaims.AsNoTracking().Where(r => r.RoleId == roleId).ToListAsync();

        return roleClaims
            .Select(rc => new Claim(rc.ClaimType, rc.ClaimValue))
            .ToList();
    }

    public async Task<IEnumerable<Claim>> GetAllUserClaimsAsync(string userId)
    {
        var claims = new List<Claim>();

        // Get user roles
        var roles = _dbContext.UserRoles.AsNoTracking()
                        .Where(ur => ur.UserId == userId)
                        .Join(_dbContext.Roles,
                            userRole => userRole.RoleId,
                            role => role.Id,
                            (userRole, role) => role.Name)
                        .ToList();

        // Get claims from each role
        foreach (var role in roles)
        {
            var roleClaims = await GetRoleClaimsAsync(role);
            claims.AddRange(roleClaims);
        }

        return claims;
    }
}

