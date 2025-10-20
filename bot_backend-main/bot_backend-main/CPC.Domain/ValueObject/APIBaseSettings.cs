using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPC.Domain.ValueObject
{
    public class APIBaseSettings
    {
        public Jwt Jwt { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }

        public ExternalAPIs ExternalAPIs { get; set; }
    }

    public class ConnectionStrings
    {
        public string CPConnection { get; set; }
    }

    public class Jwt
    {
        public string Key { get; set; }
    }

    public class ExternalAPIs
    {
        public string ChatBot { get; set; }
    }
}
