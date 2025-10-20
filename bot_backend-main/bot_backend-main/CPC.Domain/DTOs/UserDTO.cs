using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Domain.DTOs
{
    public class UserDTO
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
    }
}
