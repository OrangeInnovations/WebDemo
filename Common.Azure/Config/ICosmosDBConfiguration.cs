using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Azure.Config
{
    public interface ICosmosDBConfiguration
    {
        public string EndPointURI { get; set; }
        public string SecureKey { get; set; }
        public string DatbaseId { get; set; }
    }
}
