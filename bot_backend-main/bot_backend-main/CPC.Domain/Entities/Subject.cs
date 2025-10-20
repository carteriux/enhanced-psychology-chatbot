using System;
using System.Collections.Generic;

namespace CPC.Domain.Entities;

public partial class Subject
{
    public int IdSubject { get; set; }

    public string SubjectName { get; set; } = null!;

    public List<Activity> Activities { get; set; } = new List<Activity>();
}
