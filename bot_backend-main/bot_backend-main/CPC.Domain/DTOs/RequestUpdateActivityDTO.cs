using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Domain.DTOs
{
    public class RequestUpdateActivityDTO
    {
        public int id { get; set; }
        public int idUser { get; set; }        
        public string question { get; set; }                
    }

    public class RequestEndActivityDTO
    {
        public int id { get; set; }
        public int idUser { get; set; }        
    }

    public class RequestGetActivityDTO
    {
        public int id { get; set; }
        public int idUser { get; set; }

        public string FileName { get; set; }
    }
}
