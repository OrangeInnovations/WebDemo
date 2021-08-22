using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Azure
{
    public interface IKeyVaultHelper
    {
        Task<string> GetSecret(string keyVaultName, string secretName);
    }
}
