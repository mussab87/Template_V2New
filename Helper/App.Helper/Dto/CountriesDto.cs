
using System.ComponentModel.DataAnnotations;

namespace App.Helper.Dto { }
public class CountriesDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name English Field Required")]
    [Display(Name = "Country Name English")]
    public string NameEnglish { get; set; }

    [Display(Name = "Country Name Arabic")]
    public string? NameArabic { get; set; }

    [Display(Name = "Country Status")]
    public bool? Status { get; set; }

    public string? CreatedById { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? LastModifiedById { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}

