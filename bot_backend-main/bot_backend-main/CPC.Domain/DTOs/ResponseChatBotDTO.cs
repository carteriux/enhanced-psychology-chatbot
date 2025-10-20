using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Domain.DTOs
{
    public class ResponseChatBotDTO
    {
        public Result result { get; set; }
        public string data { get; set; }
    }

    public class Result
    {
        public int code { get; set; }
        public string message { get; set; }
    }
}
