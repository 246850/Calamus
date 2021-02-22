using Calamus.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Youle.Sys.WebApi.Startups
{
    public class DependenciesStartup : IHostStartup
    {
        public int Order => 1;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            // Services
            var types = typeFinder.FindClassesOfType<IDependency>(false);
            var interfaces = types.Where(t => t.IsInterface && t != typeof(IDependency)).ToList();
            var impements = types.Where(t => !t.IsAbstract).ToList();
            var didatas = interfaces
                .Select(t =>
                {
                    return new
                    {
                        serviceType = t,
                        implementationType = impements.FirstOrDefault(c => t.IsAssignableFrom(c))
                    };
                }
                ).ToList();

            didatas.ForEach(t =>
            {
                if (t.implementationType != null)
                    services.AddScoped(t.serviceType, t.implementationType);
            });
        }
    }
}
