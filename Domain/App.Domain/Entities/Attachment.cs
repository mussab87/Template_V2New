
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities { }
public class Attachment : EntityBase
{
    public string Path { get; set; }
    public int? AttachmentTypeId { get; set; }
    [ForeignKey("AttachmentTypeId")]
    public AttachmentType AttachmentType { get; set; }
}

