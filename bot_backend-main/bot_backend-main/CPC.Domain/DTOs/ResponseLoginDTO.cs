using CPC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Domain.DTOs
{
    public class ResponseLoginDTO
    {
        public string Token { get; set; }

        public UserDTO User { get; set; }
    }
}
