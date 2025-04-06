namespace App.Helper.Dto
{ }
public class RoleDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string RoleNameArabic { get; set; }
    public string Description { get; set; }

    public string CreatedById { get; set; }
}

