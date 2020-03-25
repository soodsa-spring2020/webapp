using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using JustEat.StatsD;
using Microsoft.AspNetCore.Http;

namespace csye6225.Services
{
    public class CloudWatchService
    {
        // private readonly RequestDelegate _next;
        // private readonly IStatsDPublisher _stats;

        // public CloudWatchService(RequestDelegate next, IStatsDPublisher stats )
        // {
        //     _next = next;
        //     _stats = stats;
        // }

        // public async Task InvokeAsync(HttpContext context)
        // {
        //     var statName = context.Request.Path;
        //     var stopWatch = Stopwatch.StartNew();
        //     await _next(context);
        //     stopWatch.Stop();

        //     _stats.Increment(statName);
        //     _stats.Timing(stopWatch.ElapsedMilliseconds, statName);
        // }
    }
}