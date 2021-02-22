using Calamus.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Youle.Sys.WebApi.Startups
{
    public class DbContextStartup : IHostStartup
    {
        public int Order => -99;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            //services.AddDbContext<YouleDbContext>(options =>
            //{
            //    options.UseSqlServer(calamusOptions.Databases["YouleDb"].Master);
            //});
        }
    }
}
