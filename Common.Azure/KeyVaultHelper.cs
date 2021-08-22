using Azure.Security.KeyVault.Secrets;
using System;
using System.Threading.Tasks;
using Azure.Identity;
namespace Common.Azure
{
    public class KeyVaultHelper : IKeyVaultHelper
    {
        public async Task<string> GetSecret(string keyVaultName, string secretName)
        {
            var kvUri = $"https://{keyVaultName}.vault.azure.net";

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            var secret = await client.GetSecretAsync(secretName);

            return secret.Value.Value;
        }
    }
}
