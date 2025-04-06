

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace App.Helper.Dto { }
public class UserDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    //[Required(ErrorMessage = "Username Field Required")]
    //[Remote(action: "IsUsernameInUse", controller: "SuperAdmin")]
    ////[Display(Name = "Username")]
    public required string Username { get; set; }
    //[Required]
    //[EmailAddress]
    //[Remote(action: "IsEmailInUse", controller: "SuperAdmin")]
    public required string Email { get; set; }

    public bool EmailConfirmed { get; set; } = false;

    public string PhoneNumber { get; set; }

    //[Required(ErrorMessage = "First Name Field Required")]
    ////[Display(Name = "First Name")]
    public string FirstName { get; set; }

    //[Required(ErrorMessage = "Last Name Field Required")]
    ////[Display(Name = "Last Name")]
    public string LastName { get; set; }

    //[Required(ErrorMessage = "User Status Field Required")]
    //[Display(Name = "User Status")]
    public bool? UserStatus { get; set; }

    public bool? FirstLogin { get; set; }
    public bool? IsActive { get; set; }

    public string CreatedBy { get; set; }

}

