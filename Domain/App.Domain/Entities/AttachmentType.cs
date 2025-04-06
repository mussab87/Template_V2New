
namespace App.Domain.Entities { }

public class AttachmentType : EntityBase
{
    /// <summary>
    /// Personal Passport Photo
    /// Poster Photo
    /// Passport Copy
    /// </summary>
    public string TypeName { get; set; }
    public int size { get; set; }
}

