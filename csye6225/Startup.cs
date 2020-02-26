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

namespace csye6225
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors();
            services.AddControllers();
            services.Configure<Parameters>(Configuration.GetSection("Config"));
            //services.AddEntityFrameworkNpgsql();

            //services.AddScoped<IPasswordHasher<ApplicationUser>, BCryptPasswordHasher<ApplicationUser>>();

            //services.AddIdentity<ApplicationUser, IdentityRole>();
            // var connectionString = Configuration.GetConnectionString("DBConnection");
            // services.AddDbContext<dbContext> (
            //     options => options.UseNpgsql(connectionString)
            // );

            services.AddDbContext<dbContext>();
            
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddMvc(options => { 
                options.Filters.Add(typeof(ModelValidationFilterAttribute)); 
            });

            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddAutoMapper(typeof(Startup));

            // configure DI for application services

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBillService, BillService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else 
            {
                app.UseHsts();
            }

            app.UseCors(builder =>builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
