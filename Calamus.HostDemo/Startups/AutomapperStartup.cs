using AutoMapper;
using Calamus.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Youle.Sys.WebApi.Startups
{
    public class AutomapperStartup : IHostStartup
    {
        public int Order => 1;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            //services.AddAutoMapper(typeof(DemoProfile).Assembly);
        }
    }
}
