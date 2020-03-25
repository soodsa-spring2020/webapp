using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using csye6225.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using csye6225.Services;
using csye6225.Helpers;
using csye6225.Filters;
using AutoMapper;
using System;
using Microsoft.Extensions.Options;
using Amazon.S3;
using Microsoft.Extensions.Logging;
using Amazon.CloudWatch;
using Microsoft.AspNetCore.HttpOverrides;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using System.IO;
using JustEat.StatsD;

namespace csye6225
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStatsD("localhost");
            services.AddControllers();
            services.AddDbContext<dbContext> (
                options => options.UseNpgsql(Configuration.GetConnectionString("DBConnection"))
            );

            // Add our Config object so it can be injected
            services.Configure<Parameters>(Configuration.GetSection("S3"));
            services.AddMvc(options => { 
                options.Filters.Add(typeof(ModelValidationFilterAttribute)); 
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = 
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddAutoMapper(typeof(Startup));

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<IFileService, FileService>();
            //services.AddSingleton<CloudWatchService>();

            //services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            //services.AddAWSService<IAmazonCloudWatch>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseFileServer();
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else 
            {
                app.UseDeveloperExceptionPage();
                //app.UseHsts();
            }
         
            var config = this.Configuration.GetAWSLoggingConfigSection();
            loggerFactory.AddAWSProvider(config);

            //app.UseMiddleware<CloudWatchExecutionTimeService>();
            app.UseMiddleware<CloudWatchService>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
