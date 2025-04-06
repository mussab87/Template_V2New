
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace App.Helper.Dto { }

public class CreateRoleDto
{
    [Required(ErrorMessage = "Role Name Field Required")]
    [Display(Name = "Role Name")]
    public required string RoleName { get; set; }

    public List<IdentityRole>? Roles { get; set; }
}

