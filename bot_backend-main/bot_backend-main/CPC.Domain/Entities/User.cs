using System;
using System.Collections.Generic;

namespace CPC.Domain.Entities;

public partial class User
{
    public int IdUser { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string EnrollmentNumber { get; set; } = null!;    

    public bool? IsFirstTime { get; set; }

    public DateTime? LastAccessDate { get; set; }

    public bool? IsAdmin { get; set; }

    public string? Cohort { get; set; }

    public List<Useractivity> Useractivity { get; set; } = new List<Useractivity>();
    public string Password { get; set; }
}
