using System;
using System.Collections.Generic;

namespace CPC.Domain.Entities;

public partial class Activity
{
    public int IdActivity { get; set; }

    public string ActivityName { get; set; } = null!;

    public int IdSubject { get; set; }

    public Subject IdSubjectNavigation { get; set; } = null!;

    public List<Useractivity> Useractivity { get; set; } = new List<Useractivity>();
}
