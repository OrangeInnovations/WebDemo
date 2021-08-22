using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Azure.CosmosDb
{
    public interface ICosmosDbClientFactory
    {
        ICosmosDbClient GetClient(string containerId);
    }
}
