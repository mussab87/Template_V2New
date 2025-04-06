using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Common { }
public abstract class EntityBase()
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key, Column(Order = 0)]
    public int Id { get; set; }
    public string CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string LastModifiedById { get; set; }
    public User LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; } = DateTime.Now;
}

