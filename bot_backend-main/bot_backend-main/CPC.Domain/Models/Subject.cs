using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPC.Domain.Models;

[Table("Subjects")]
public partial class Subject
{
    [Key]
    public int IdSubject { get; set; }

    public string SubjectName { get; set; } = null!;

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
