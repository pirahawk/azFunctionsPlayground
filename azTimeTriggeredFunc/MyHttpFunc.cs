using System;
using AzCosmosDb;
using AzFunctionsDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace azTimeTriggeredFunc
{
    public static class MyHttpFunc
    {
        [FunctionName("Allusers")]
        public static async Task<IActionResult> ListAllUsers(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "allusers/all")] HttpRequest req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //return name != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {name}")
            //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");

            var host = new HostBuilderFactory().BuildHost();
            using (var scope = host.Services.CreateScope())
            {
                var userReader = scope.ServiceProvider.GetService<IUserDtoReader>();
                var users = await userReader.GetUsers();

                return new OkObjectResult(users) ;
            }
        }

        [FunctionName("AddUser")]
        public static async Task<IActionResult> AddUser(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "/add/{id:guid}")] HttpRequest req,
            ILogger log, Guid id)
        {
            var host = new HostBuilderFactory().BuildHost();
            using (var scope = host.Services.CreateScope())
            {
                var userReader = scope.ServiceProvider.GetService<IUserDtoReader>();
                var users = await userReader.GetUsers();

                return new OkObjectResult(users);
            }
        }
    }
}
