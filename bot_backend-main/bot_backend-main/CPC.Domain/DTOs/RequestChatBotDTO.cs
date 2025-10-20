using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Domain.DTOs
{
    public class RequestChatBotDTO
    {
        public string user_id { get; set; }
        public string question { get; set; }
        public string activity_id { get; set; }
    }

    public class RequestSaveActivityDTO
    {
        public string user_id { get; set; }
        public string activity_id { get; set; }
    }
}
