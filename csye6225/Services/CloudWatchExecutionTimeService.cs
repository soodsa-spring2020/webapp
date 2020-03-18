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
                await _amazonCloudWatch.PutMetricDataAsync(new PutMetricDataRequest
                {
                    Namespace = "CSYE6225-Webapp",
                    MetricData = new List<MetricDatum>
                    {
                        new MetricDatum
                        {
                            MetricName = "Http Time",
                            Value = stopWatch.ElapsedMilliseconds,
                            Unit = StandardUnit.Milliseconds,
                            TimestampUtc = DateTime.UtcNow,
                            Dimensions = new List<Dimension>
                            {
                                new Dimension
                                {
                                    Name = "Method",
                                    Value = context.Request.Method
                                },
                                new Dimension
                                {
                                    Name = "Path",
                                    Value = context.Request.Path
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to send CloudWatch Metric");
            } 
        }
    }
}