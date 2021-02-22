using Calamus.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Profiling.Storage;

namespace Youle.Sys.WebApi.Startups
{
    public class MiniProfilerStartup : IHostStartup
    {
        public int Order => 2;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseMiniProfiler();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            // Note .AddMiniProfiler() returns a IMiniProfilerBuilder for easy intellisense
            services.AddMiniProfiler(options=>
            {
                options.RouteBasePath = "/profiler";
                //options.Storage = new RedisStorage(calamusOptions.Redis.Configuration);
            }).AddEntityFramework();
        }
    }
}
