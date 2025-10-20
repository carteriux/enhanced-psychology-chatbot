using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Domain.DTO.ValueObjects
{
    public class LocalSession
    {
        //public SessionDTO Session { get; set; }

        public int IdUser { get; set; }
        public string Email { get; set; } = null!;
        public string EnrollmentNumber { get; set; } = null!;
        public DateTime FechaUTC { get; set; }
       
    }    
}
