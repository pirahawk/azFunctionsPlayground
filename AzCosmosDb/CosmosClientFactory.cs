using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace AzCosmosDb
{
    public interface ICosmosClientFactory
    {
        CosmosClient CreateClient();
    }

    public class CosmosClientFactory : ICosmosClientFactory
    {
        private readonly AzCosmosOptions _azCosmosOptions;

        public CosmosClientFactory(IOptions<AzCosmosOptions> azCosmosOptions)
        {
            _azCosmosOptions = azCosmosOptions.Value;
        }

        public CosmosClient CreateClient()
        {
            var cosmosClient = new CosmosClient(_azCosmosOptions.EndpointUrl, _azCosmosOptions.AuthorizationKey);
            return cosmosClient;
        }
    }
}