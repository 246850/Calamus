using Calamus.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Youle.Sys.WebApi.Startups
{
    public class RabbitMQStartup : IHostStartup
    {
        public int Order => 1;

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env, CalamusOptions calamusOptions, ITypeFinder typeFinder)
        {
            //services.AddRabbitMessager<TextJsonSerializer>("amqp://admin:123456@101.132.140.8:5672/oms", "Calamus.Messager", queues =>
            //{
            //    queues.Add(new RabbitQueueBindingEntry { Queue = "queue1", RoutingKey = "queue1" });
            //    queues.Add(new RabbitQueueBindingEntry { Exchange = DefaultExchangeName.AMQ_DIRECT, ExchangeType = "direct", Queue = "queue2", RoutingKey = "queue2" });
            //});
            //services.AddHostedService<RabbitConsumerHostService>();
        }
    }
}
