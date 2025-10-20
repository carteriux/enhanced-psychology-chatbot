using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPC.Domain.Models;

[Table("Users")]
public partial class User
{
    [Key]
    public int IdUser { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? MiddleName { get; set; }

    public string EnrollmentNumber { get; set; }

    public string Password { get; set; }

    public bool? IsFirstTime { get; set; }

    public DateTime? LastAccessDate { get; set; }

    public bool? IsAdmin { get; set; }

    public virtual ICollection<Useractivity> Useractivity { get; set; } = new List<Useractivity>();
}
