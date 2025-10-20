using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Domain.DTOs
{
    public class ResponseMessageDTO
    {
        public string Message { get; set; }

        public ResultMessage Result { get; set; } = new ResultMessage();
    }

    public class ResultMessage
    {
        public bool Success { get; set; }

        public string Warning_Message { get; set; }
    }

}
