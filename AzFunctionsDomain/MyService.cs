using Microsoft.Extensions.Logging;

namespace AzFunctionsDomain
{
    public interface IMyService
    {
        void DoSomething();
    }

    public class MyService : IMyService
    {
        private readonly ILogger<MyService> _logger;

        public MyService(ILogger<MyService> logger)
        {
            _logger = logger;
        }

        public void DoSomething()
        {
            _logger.LogInformation($"Hello Information! :)");
        }
    }
}