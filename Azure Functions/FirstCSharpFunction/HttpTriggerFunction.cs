using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class HttpTriggerFunction
    {
        [FunctionName("HttpTriggerFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, TextWriter outputBlob)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string result = "low";

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            decimal inputValue = data?.rating;

            if(inputValue > 0.3m && inputValue < .6m)
            {
                result = "average";
            }
            else if(inputValue >= .6m)
            {
                result = "high";
            }

            outputBlob.WriteLine($"Calculated a result of {result} for the input of {inputValue}");

            return inputValue > 0
                ? (ActionResult)new OkObjectResult($@"{{'result':'{result}'}}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");

//ORIGINAL - START
            // string name = req.Query["name"];

            // string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // dynamic data = JsonConvert.DeserializeObject(requestBody);
            // name = name ?? data?.name;

            // string responseMessage = string.IsNullOrEmpty(name)
            //     ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //     : $"Hello, {name}. This HTTP triggered function executed successfully.";

            // return new OkObjectResult(responseMessage);
//ORIGINAL - END

        }
    }
}
