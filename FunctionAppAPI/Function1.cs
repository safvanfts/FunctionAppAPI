using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FunctionAppAPI
{
    public static class Function1
    {
        static List<Employee> lstEmployee = new List<Employee>();
        [FunctionName("TimeTrigger1")]
        public static void TimeTrigger1([TimerTrigger("0 57 15 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
        [FunctionName("TimeTrigger2")]
        public static void TimeTrigger2([TimerTrigger("0 57 15 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
        [FunctionName("CreateEmployee")]
        public static async Task<IActionResult> CreateEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "create")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Employee create request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var reqData = JsonConvert.DeserializeObject<Employee>(requestBody);
            lstEmployee.Add(reqData);
            string name = req.Query["name"];

            return new OkObjectResult(reqData);
        }

        [FunctionName("GetEmployee")]
        public static async Task<IActionResult> GetEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getemployee/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Employee get request.");
            if (string.IsNullOrEmpty(id))
            {
                return new OkObjectResult(lstEmployee);
            }

            var data = lstEmployee.FirstOrDefault(x => x.EmployeeId == id);
            if (data == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(lstEmployee);
        }
    }
}
