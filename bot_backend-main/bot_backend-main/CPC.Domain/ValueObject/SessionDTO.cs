using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Domain.DTO.ValueObjects
{
    public class SessionDTO
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }        
        public System.DateTime IssuedOn { get; set; }
        public System.DateTime ExpiresOn { get; set; }
        public string IP { get; set; }
        public string Token { get; set; }
    }
}
