using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.AppDatabase { }

public class AppDbContext : IdentityDbContext<User, Role, string,
                                                        IdentityUserClaim<string>,
                                                        IdentityUserRole<string>,
                                                        IdentityUserLogin<string>,
                                                        RoleClaim, IdentityUserToken<string>>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<RoleClaim> RoleClaims { get; set; }

    public DbSet<UserPasswordLog> UserPasswordLog { get; set; }
    public DbSet<UserLoginLog> UserLoginLog { get; set; }

    public DbSet<AttachmentType> AttachmentTypes { get; set; }

    public DbSet<Attachment> Attachments { get; set; }

    public DbSet<Audit> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Map RoleClaim to AspNetRoleClaims table
        modelBuilder.Entity<RoleClaim>().ToTable("AspNetRoleClaims");

        //Ensure other tables use standard Identity naming
        modelBuilder.Entity<User>().ToTable("AspNetUsers");
        modelBuilder.Entity<Role>().ToTable("AspNetRoles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");

        //change AspNet Users tables names
        var entityTypes = modelBuilder.Model.GetEntityTypes();
        foreach (var entityType in entityTypes)
            modelBuilder.Entity(entityType.ClrType)
                   .ToTable(entityType.GetTableName().Replace("AspNet", ""));
    }


    public virtual async Task<int> SaveChangesAsync(string UserId = "", string IpAddress = "")
    {
        OnBeforeSaveChanges(UserId);
        var result = await base.SaveChangesAsync();
        return result;
    }

    public virtual int SaveChanges(string userId = null)
    {
        OnBeforeSaveChanges(userId);
        var result = base.SaveChanges();
        return result;
    }

    public virtual async Task<int> CreateAsync(string userId = null)
    {
        OnBeforeSaveChanges(userId);
        var result = await base.SaveChangesAsync();
        return result;
    }

    private void OnBeforeSaveChanges(string UserId = "", string IpAddress = "")
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;
            var auditEntry = new AuditEntry(entry);
            auditEntry.TableName = entry.Entity.GetType().Name;
            auditEntry.UserId = UserId;
            auditEntry.IpAddress = IpAddress;
            auditEntries.Add(auditEntry);
            foreach (var property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = AuditEntry.TypeAudit.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditEntry.TypeAudit.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = AuditEntry.TypeAudit.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }
        foreach (var auditEntry in auditEntries)
        {
            AuditLogs.Add(auditEntry.ToAudit());
        }
    }
}

