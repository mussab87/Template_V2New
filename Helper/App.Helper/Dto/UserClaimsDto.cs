
namespace App.Helper.Dto { }
public class UserClaimsDto
{
    public UserClaimsDto()
    {
        Cliams = new List<UserClaim>();
    }

    public string? UserId { get; set; }
    public List<UserClaim> Cliams { get; set; }
}

