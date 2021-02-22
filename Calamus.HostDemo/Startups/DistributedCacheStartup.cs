using Calamus.Caching.Middleware;
using Calamus.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using StackExchange.Redis.KeyspaceIsolation;
using System.Threading.Channels;

namespace Youle.Sys.WebApi.Startups
{
    /// <summary>
    /// 缓存 启动注册
    /// </summary>
    public class DistributedCacheStartup : IHostStartup
    {
        public int Order => 1;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseRedisInformation();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            services.AddMemoryCache();  // 内存缓存
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = calamusOptions.Redis.Configuration;
                option.InstanceName = calamusOptions.Redis.KeyPrefix;
            });

            services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
            {
                var connection = ConnectionMultiplexer.Connect(calamusOptions.Redis.Configuration, null);
                connection.ConnectionFailed += (sender, args) => { };
                connection.ConnectionRestored += (sender, args) => { };
                connection.InternalError += (sender, args) => { };
                return connection;
            });
            services.AddTransient<IDatabase>(serviceProvider =>
            {
                IConnectionMultiplexer connection = serviceProvider.GetRequiredService<IConnectionMultiplexer>();
                IDatabase db = connection.GetDatabase().WithKeyPrefix(calamusOptions.Redis.KeyPrefix);
                return db;
            });
        }
    }
}
