using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace csye6225.Services
{
    public class CloudWatchExecutionTimeService
    {
        // private readonly RequestDelegate _next;
        // private readonly ILogger _logger;
        // private readonly IAmazonCloudWatch _amazonCloudWatch;

        // Dictionary<string, int> pathpairs = new Dictionary<string, int>();

        // public CloudWatchExecutionTimeService(RequestDelegate next, ILogger<CloudWatchExecutionTimeService> logger, IAmazonCloudWatch amazonCloudWatch)
        // {
        //     _next = next;
        //     _logger = logger;
        //     _amazonCloudWatch = amazonCloudWatch;
        // }
        
        // public async Task InvokeAsync(HttpContext context)
        // {
        //     var stopWatch = new Stopwatch();
        //     stopWatch.Start();
        //     await _next(context);
        //     stopWatch.Stop();

        //     try
        //     {
        //         if(pathpairs.ContainsKey(context.Request.Path)) {
        //             pathpairs[context.Request.Path]++;
        //         }
        //         else{
        //             pathpairs.Add(context.Request.Path, 1);
        //         }

        //         await _amazonCloudWatch.PutMetricDataAsync(new PutMetricDataRequest
        //         {
        //             Namespace = "CSYE6225-Webapp",
        //             MetricData = new List<MetricDatum>
        //             {
        //                 new MetricDatum
        //                 {
        //                     MetricName = "HttpCounter",
        //                     Value = pathpairs[context.Request.Path],
        //                     Unit = StandardUnit.None,
        //                     TimestampUtc = DateTime.UtcNow,
        //                     Dimensions = new List<Dimension>
        //                     {
        //                         new Dimension
        //                         {
        //                             Name = "Method",
        //                             Value = context.Request.Method
        //                         },
        //                         new Dimension
        //                         {
        //                             Name = "Path",
        //                             Value = context.Request.Path
        //                         },
        //                         new Dimension
        //                         {
        //                             Name = "Counter",
        //                             Value = pathpairs[context.Request.Path].ToString()
        //                         }
        //                     }
        //                 },
        //                 new MetricDatum
        //                 {
        //                     MetricName = "DBTimer",
        //                     Value = stopWatch.ElapsedMilliseconds,
        //                     Unit = StandardUnit.Milliseconds,
        //                     TimestampUtc = DateTime.UtcNow,
        //                     Dimensions = new List<Dimension>
        //                     {
        //                         new Dimension
        //                         {
        //                             Name = "Service",
        //                             Value = context.RequestServices.ToString()
        //                         },
        //                         new Dimension
        //                         {
        //                             Name = "Timer",
        //                             Value = stopWatch.ElapsedMilliseconds.ToString()
        //                         }
        //                     }
        //                 },
        //                 new MetricDatum
        //                 {
        //                     MetricName = "ExecutionTime",
        //                     Value = stopWatch.ElapsedMilliseconds,
        //                     Unit = StandardUnit.Milliseconds,
        //                     TimestampUtc = DateTime.UtcNow,
        //                     Dimensions = new List<Dimension>
        //                     {
        //                         new Dimension
        //                         {
        //                             Name = "Method",
        //                             Value = context.Request.Method
        //                         },
        //                         new Dimension
        //                         {
        //                             Name = "Path",
        //                             Value = context.Request.Path
        //                         },
        //                         new Dimension
        //                         {
        //                             Name = "Timer",
        //                             Value = stopWatch.ElapsedMilliseconds.ToString()
        //                         }
        //                     }
        //                 }
        //             }
        //         });
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogCritical(ex, "Failed to send CloudWatch Metric");
        //     } 
        // }
    }
}