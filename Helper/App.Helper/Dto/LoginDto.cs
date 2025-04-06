using System.ComponentModel.DataAnnotations;

namespace App.Helper.Dto { }

public class LoginDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم المستخدم حقل إجباري")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "كلمة المرور حقل إجباري")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Required(ErrorMessage = "حقل إجباري")]
    public string CaptchaInput { get; set; }
    public string? ReturnUrl { get; set; }
}

