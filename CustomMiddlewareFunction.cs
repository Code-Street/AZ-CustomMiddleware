using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CustomMiddleware
{
    public class CustomMiddlewareFunction
    {
        private readonly ILogger<CustomMiddlewareFunction> _logger;

        public CustomMiddlewareFunction(ILogger<CustomMiddlewareFunction> logger)
        {
            _logger = logger;
        }

        [Function("Custom-Middleware")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            int.TryParse(req.Query["number"], out int number);
            int res = 10 / number;

            return new OkObjectResult(new {result = res});
        }
    }
}
