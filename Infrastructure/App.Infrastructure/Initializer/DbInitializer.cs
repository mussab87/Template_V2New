using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure.Mail.Initializer { }

public class DbInitializer : IDbInitializer
{
    private AppDbContext _dbContext;
    private IUserService _userService;
    private IRoleService _roleService;
    private readonly IServiceProvider _serviceProvider;

    public DbInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async void InitializeAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();

            _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            _roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();

            // Apply pending migrations before seeding data
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                await _dbContext.Database.MigrateAsync();
            }

            //Check role SuperAdmin exist or no
            if (!await _roleService.RoleExistsAsync(Roles.SuperAdmin))
            {
                //In case SuperAdmin role not Exist
                //system run at first time - Create SuperAdmin role and Admin user
                var roleToAdd = new RoleDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Roles.SuperAdmin,
                    RoleNameArabic = "مدير النظام",
                    CreatedById = "System Super Admin"
                };
                var newRole = await _roleService.CreateRoleAsync(roleToAdd);

                //Create User admin and link with the Role was created a top SuperAdmin
                var adminUser = await CreateAdminUser();

                var role = await _roleService.FindByNameAsync(Roles.SuperAdmin);
                if (role != null)
                {
                    var allPermissions = GetAllClaimsPermissions.GetAllControllerActionsUpdated();
                    //UserHelper userHelper = new(_db, _roleManager);
                    await _roleService.AddClaimsToRole(adminUser, role, allPermissions);
                }
            }
            else
            {
                return;
            }
        }
        catch (Exception)
        {
        }
    }

    async Task<User> CreateAdminUser()
    {
        UserDto adminUser = new UserDto()
        {
            Username = "admin",
            Email = "admin@admin.com",
            EmailConfirmed = true,
            PhoneNumber = "111111111111",
            FirstName = "admin",
            LastName = "test",
            UserStatus = true,
            FirstLogin = true,
            IsActive = true,
            CreatedBy = "System Super Admin"
        };
        return await _userService.CreateUser(adminUser, "Aa@123456", Roles.SuperAdmin);
    }
}

