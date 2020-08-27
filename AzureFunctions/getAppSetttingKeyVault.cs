#r "Newtonsoft.Json"

using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");
    var value = Environment.GetEnvironmentVariable("APPLICATIONTEST_SECRET");

    string responseMessage = $"Hello. This HTTP triggered function executed successfully. Secret key {value}";

            return new OkObjectResult(responseMessage);
}
