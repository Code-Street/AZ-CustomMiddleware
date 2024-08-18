using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomMiddleware
{
    public class CustomExceptionHandler : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
                Console.WriteLine("No Exception occured");
            }
            catch (Exception ex) 
            {
                var request = await context.GetHttpRequestDataAsync();
                var response = request!.CreateResponse();
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;

                var errorMessage = new {Message = "An unhandled exception occured. Please try again later", Exception=ex.Message};
                string responseBody = JsonSerializer.Serialize(errorMessage);

                await response.WriteStringAsync(responseBody);

                Console.WriteLine("Exception occured");
                context.GetInvocationResult().Value = response;
            }
        }
    }
}
