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
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IAmazonCloudWatch _amazonCloudWatch;
        Dictionary<string, int> pathpairs = new Dictionary<string, int>();


        public CloudWatchExecutionTimeService(RequestDelegate next, ILogger<CloudWatchExecutionTimeService> logger, IAmazonCloudWatch amazonCloudWatch)
        {
            _next = next;
            _logger = logger;
            _amazonCloudWatch = amazonCloudWatch;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await _next(context);
            stopWatch.Stop();

            try
            {
                if(pathpairs.ContainsKey(context.Request.Path)) {
                    pathpairs[context.Request.Path]++;
                }
                else{
                    pathpairs.Add(context.Request.Path, 1);
                }

                var md1 = new MetricDatum() 
                {
                    MetricName = context.Request.Path + " Counter",
                    Value = pathpairs[context.Request.Path],
                    Unit = StandardUnit.Count,
                    TimestampUtc = DateTime.UtcNow
                   
                };

                var md2 = new MetricDatum()
                {
                    MetricName = context.Request.Path + " Timer",
                    Value = stopWatch.ElapsedMilliseconds,
                    Unit = StandardUnit.Milliseconds,
                    TimestampUtc = DateTime.UtcNow,
                };

                var metricDatum = new List<MetricDatum>() { md1, md2 };
                var request = new PutMetricDataRequest() 
                {
                    Namespace = "CSYE6225-Webapp-1",
                    MetricData = metricDatum
                };

                await _amazonCloudWatch.PutMetricDataAsync(request);

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to send CloudWatch Metric");
            } 
        }
    }
}