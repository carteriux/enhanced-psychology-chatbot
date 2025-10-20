using System;
using System.Collections.Generic;

namespace CPC.Domain.Entities;

public partial class Useractivity
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdActivity { get; set; }

    public int Count { get; set; }

    public decimal? ProgressPercentage { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime? EndDateTime { get; set; }

    public string? FilePath { get; set; }

    public string ActivityName { get; set; }
}
