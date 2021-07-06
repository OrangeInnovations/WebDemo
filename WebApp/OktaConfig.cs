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

        

        /// <summary>
        /// okta security=>Default Authorization server IssuerURI
        /// </summary>
        public string IssuerURI { get; set; }

        /// <summary>
        /// okta security=>Default Authorization server Audience
        /// </summary>
        public string Audience { get; set; }
    }
}
