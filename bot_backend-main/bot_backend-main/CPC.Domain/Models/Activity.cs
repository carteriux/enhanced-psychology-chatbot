using CPC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPC.Domain.Models;

[Table("Activities")]
public partial class Activity
{
    [Key]
    public int IdActivity { get; set; }

    public string ActivityName { get; set; } = null!;

    [ForeignKey("IdSubject")]
    public int IdSubject { get; set; }

    [ForeignKey(nameof(IdSubject))]
    public virtual Subject IdSubjectNavigation { get; set; } = null!;

    public virtual ICollection<Useractivity> Useractivity { get; set; } = new List<Useractivity>();
}
