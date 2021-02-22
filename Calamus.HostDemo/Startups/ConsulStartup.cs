using Calamus.Ioc;
using Consul;
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
    public class ConsulStartup : IHostStartup
    {
        public ConsulStartup()
        {

        }
        public int Order => 2;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            //services.AddConsul(options =>
            //{
            //    options.Address = new Uri(calamusOptions.Consul.Address);
            //    options.Datacenter = calamusOptions.Consul.Datacenter;
            //    options.Token = calamusOptions.Consul.Token;
            //    options.WaitTime = calamusOptions.Consul.WaitTime > 0 ? TimeSpan.FromSeconds(calamusOptions.Consul.WaitTime) : (TimeSpan?)null;
            //})
            //.AddConsulServiceRegistration(options =>
            //{
            //    options.ID = calamusOptions.Consul.ServiceId;
            //    options.Name = calamusOptions.Consul.ServiceName;
            //    options.Address = calamusOptions.Consul.ServiceIP;
            //    options.Port = calamusOptions.Consul.ServicePort;
            //    options.Tags = new string[] { "SysApi" };
            //    options.Check = new AgentServiceCheck
            //    {
            //        HTTP = $"http://{calamusOptions.Consul.ServiceIP}:{calamusOptions.Consul.ServicePort}{calamusOptions.Consul.HealthCheckPath}",
            //        Interval = TimeSpan.FromSeconds(5),
            //        Timeout = TimeSpan.FromSeconds(60),
            //        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(300)
            //    };
            //});
        }
    }
}
