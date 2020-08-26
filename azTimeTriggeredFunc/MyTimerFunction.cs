using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using AzFunctionsDomain;
using Microsoft.Extensions.DependencyInjection;

namespace azTimeTriggeredFunc
{
    public static class MyTimerFunction
    {
        [FunctionName("MyTimerFunction")]
        public static void Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            using var scope = HostBuilderFactory.Host.Services.CreateScope();
            var myService = scope.ServiceProvider.GetService<IMyService>();
            //var myService = ActivatorUtilities.CreateInstance<IMyService>(scope.ServiceProvider);
            myService.DoSomething();
        }
    }
}
