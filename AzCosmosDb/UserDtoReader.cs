using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzCosmosDb
{
    public interface IUserDtoReader
    {
        Task<IEnumerable<MyUser>> GetUsers();
    }

    public class UserDtoReader : IUserDtoReader
    {
        private readonly ICosmosClientFactory _clientFactory;
        private readonly AzCosmosOptions _azCosmosOptions;

        public UserDtoReader(ICosmosClientFactory clientFactory, IOptions<AzCosmosOptions> azCosmosOptions)
        {
            _clientFactory = clientFactory;
            _azCosmosOptions = azCosmosOptions.Value;
        }

        public async Task<IEnumerable<MyUser>> GetUsers()
        {
            var client = _clientFactory.CreateClient();
            var container = client.GetContainer(_azCosmosOptions.DatabaseId, _azCosmosOptions.ContainerId);
            
            // string query = "select * from c";
            // QueryDefinition queryDefinition = new QueryDefinition(query);
            // var users = new List<MyUser>();
            // FeedIterator<MyUser> itemQueryIterator = container.GetItemQueryIterator<MyUser>(queryDefinition);
            //
            // await foreach (var user in itemQueryIterator)
            // {
            //     users.Add(user);
            // }

            // return users;

            var results = container.GetItemLinqQueryable<MyUser>(allowSynchronousQueryExecution:true).ToArray();
            return await Task.FromResult(results);
        }
    }
}