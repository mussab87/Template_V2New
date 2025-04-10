using System.ComponentModel.DataAnnotations;

namespace App.Helper.Dto
{ }
public class    ResetPasswordDto
{
    [Display(Name = "اسم المستخدم")]
    public string Username { get; set; }
    public string Token { get; set; }    
    public bool IsExpired { get; set; }
    public bool AdminResetUserPassword { get; set; }

    [Required(ErrorMessage = "حقل اجباري")]
    [DataType(DataType.Password)]
    [Display(Name = "كلمة المرور الحالية")]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "حقل اجباري")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "كلمة المرور الجديدة")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "تأكيد كلمة المرور الجديدة")]
    [Compare("NewPassword", ErrorMessage = "تأكيد كلمة المرور غير مطابقة")]
    public string ConfirmPassword { get; set; }
}

