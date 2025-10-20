using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Domain.DTOs
{
    public class ResponseFileDTO
    {
        public MemoryStream MemoryStream { get; set; }

        public string FileName { get; set; }

    }
}
