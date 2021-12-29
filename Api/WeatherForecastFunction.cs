using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using BlazorApp.Shared;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace BlazorApp.Api
{
    public static class WeatherForecastFunction
    {
        private static string GetSummary(int temp)
        {
            var summary = "Mild";

            if (temp >= 32)
            {
                summary = "Hot";
            }
            else if (temp <= 16 && temp > 0)
            {
                summary = "Cold";
            }
            else if (temp <= 0)
            {
                summary = "Freezing!";
            }

            return summary;
        }

        private static string GetRiskPot(int snowLvl)
        {
            var riskPot = "Low";

            if (snowLvl >= 8)
            {
                riskPot = "High";
            }
            else if (snowLvl >= 4 && snowLvl <= 7)
            {
                riskPot = "Medium";
            }
            else if (snowLvl <= 3 )
            {
                riskPot = "Low";
            }

            return riskPot;
        }

        private static string GetRecommendation(string riskPot)
        {
            var recommendation = "";

            if (riskPot == "Low")
            {
                recommendation = "Xue Hua Piao Piao, Bei Feng Xiao Xiao";
            }
            else if (riskPot == "Medium")
            {
                recommendation = "Might wanna bring a shovel along mate";
            }
            else if (riskPot == "High")
            {
                recommendation = "Stay indoors as if it's corona season.";
            }

            return recommendation;
        }

        [FunctionName("WeatherForecast")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var randomNumber = new Random();
            var temp = 0;

            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = temp = randomNumber.Next(-20, 55),
                Summary = GetSummary(temp)
            }).ToArray();

            return new OkObjectResult(result);
        }

        [FunctionName("SnowLevel")]
        public static IActionResult Run2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var randomNumber = new Random();
            var snowLvl = 0;
            var riskPot = "";

            var result = Enumerable.Range(1, 1).Select(index => new SnowAlert
            {
                SnowLvl = snowLvl = randomNumber.Next(0, 10),
                RiskPot = riskPot = GetRiskPot(snowLvl),
                Recommendation = GetRecommendation(riskPot)
            }).ToArray();

            return new OkObjectResult(result);
        }


        [FunctionName("HttpExample")]
        public static IActionResult Run3(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

            string responseMessage = "Hello from the API :P";


            return new OkObjectResult(responseMessage);
        }
    }
}
