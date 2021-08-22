using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Azure.Config
{
    public class CosmosDBInfo
    {
        public string DatabaseId { get; set; }
        public List<ContainerDetail> Containers { get; set; }
    }
}
