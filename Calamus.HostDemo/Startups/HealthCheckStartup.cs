using Calamus.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Youle.Sys.WebApi.Startups
{
    public class HealthCheckStartup : IHostStartup
    {
        public int Order => 2;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = (httpContext, result) =>
                {
                    httpContext.Response.ContentType = "text/plain";
                    return httpContext.Response.WriteAsync(result.Status.ToString() + " " + DateTime.Now.ToString());
                }
            });
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            //services.AddHealthChecks()
            //    .AddDbContextCheck<YouleDbContext>();   // EF Core 连接检查
        }
    }
}
