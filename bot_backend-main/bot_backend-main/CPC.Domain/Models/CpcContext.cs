using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CPC.Domain.Models;

public class CpcContext : DbContext
{    

    public CpcContext(DbContextOptions<CpcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Useractivity> Useractivity { get; set; }

}
