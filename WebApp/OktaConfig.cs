using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public class OktaConfig
    {
       
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Domain { get; set; }
        public string Issuer { get; set; }
    }
}
