using CPC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPC.Domain.Models;

[Table("UserActivity")]
public partial class Useractivity
{
    [Key]
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdActivity { get; set; }

    public int Count { get; set; }

    public decimal? ProgressPercentage { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }

    public string? FilePath { get; set; }

    [ForeignKey(nameof(IdActivity))]
    public virtual Activity IdActivityNavigation { get; set; } = null!;

    [ForeignKey(nameof(IdUser))]
    public virtual User IdUserNavigation { get; set; } = null!;
}
