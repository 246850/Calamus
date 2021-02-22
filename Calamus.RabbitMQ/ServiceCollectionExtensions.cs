using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;

namespace Calamus.RabbitMQ
{
    /// <summary>
    /// IRabbitMessager 注册 扩展
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册 IRabbitMessager
        /// </summary>
        /// <param name="services"></param>
        /// <param name="url">RabbitMq 连接地址 格式：amqp://user:password@host:port/virtualHost</param>
        /// <param name="clientName">客户端连接名称</param>
        /// <param name="bindQueues">队列定义/绑定 委托</param>
        public static void AddRabbitMessager(this IServiceCollection services, string url, string clientName, Action<List<RabbitQueueBindingEntry>> bindQueues = null)
        {
            AddConnection(services, url, clientName);
            AddModel(services, bindQueues);
            AddMessager(services);
        }

        /// <summary>
        /// 注册 IRabbitMessager - 使用序列化
        /// </summary>
        /// <typeparam name="TSerializer">消息序列化 泛型 T</typeparam>
        /// <param name="services"></param>
        /// <param name="url">RabbitMq 连接地址 格式：amqp://user:password@host:port/virtualHost</param>
        /// <param name="clientName">客户端连接名称</param>
        /// <param name="bindQueues">队列定义/绑定 委托</param>
        public static void AddRabbitMessager<TSerializer>(this IServiceCollection services, string url, string clientName, Action<List<RabbitQueueBindingEntry>> bindQueues = null)
            where TSerializer : IMessageSerializer, new()
        {
            AddConnection(services, url, clientName);
            AddModel(services, bindQueues);
            AddSerializer<TSerializer>(services);
            AddMessager(services);
        }

        /// <summary>
        /// 注册 IRabbitMessager
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setup">连接配置项</param>
        /// <param name="bindQueues">队列定义/绑定 委托</param>
        public static void AddRabbitMessager(this IServiceCollection services, [NotNull] Action<RabbitConnectionOptions> setup, Action<List<RabbitQueueBindingEntry>> bindQueues = null)
        {
            AddConnection(services, setup);
            AddModel(services, bindQueues);
            AddMessager(services);
        }

        /// <summary>
        /// 注册 IRabbitMessager - 使用序列化
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setup">连接配置项</param>
        /// <param name="bindQueues">队列定义/绑定 委托</param>
        public static void AddRabbitMessager<TSerializer>(this IServiceCollection services, [NotNull] Action<RabbitConnectionOptions> setup, Action<List<RabbitQueueBindingEntry>> bindQueues = null)
            where TSerializer : IMessageSerializer, new()
        {
            AddConnection(services, setup);
            AddModel(services, bindQueues);
            AddSerializer<TSerializer>(services);
            AddMessager(services);
        }

        /// <summary>
        /// 注册 IConnection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="url"></param>
        /// <param name="clientName"></param>
        static void AddConnection(IServiceCollection services, string url, string clientName)
        {
            services.TryAddSingleton<IConnection>(serviceProvider =>
            {
                IConnectionFactory factory = new ConnectionFactory();
                factory.Uri = new Uri(url);
                IConnection connection = factory.CreateConnection(clientName);

                return connection;
            });
        }
        /// <summary>
        /// 注册 IConnection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setup"></param>
        static void AddConnection(IServiceCollection services, [NotNull] Action<RabbitConnectionOptions> setup)
        {
            RabbitConnectionOptions options = new RabbitConnectionOptions();
            setup(options);
            services.TryAddSingleton<IConnection>(serviceProvider =>
            {
                IConnectionFactory factory = new ConnectionFactory
                {
                    UserName = options.UserName,
                    Password = options.Password,
                    HostName = options.HostName,
                    Port = options.Port,
                    VirtualHost = options.VirtualHost
                };
                IConnection connection = factory.CreateConnection(options.ClientProvidedName);

                return connection;
            });
        }
        /// <summary>
        /// 注册 IModel
        /// </summary>
        /// <param name="services"></param>
        /// <param name="bindQueues"></param>
        static void AddModel(IServiceCollection services, Action<List<RabbitQueueBindingEntry>> bindQueues)
        {
            services.TryAddSingleton<IModel>(serviceProvider =>
            {
                IConnection connection = serviceProvider.GetRequiredService<IConnection>();
                IModel channel = connection.CreateModel();

                List<RabbitQueueBindingEntry> binds = new List<RabbitQueueBindingEntry>();
                bindQueues?.Invoke(binds);
                if (binds != null && binds.Any())
                {
                    foreach (var item in binds)
                    {
                        // 定义交换机
                        if (!string.IsNullOrWhiteSpace(item.Exchange))
                        {
                            channel.ExchangeDeclare(item.Exchange, item.ExchangeType, true, false, null);
                        }

                        // 定义队列
                        QueueDeclareOk queueOk = channel.QueueDeclare(item.Queue, true, false, false, null);
                        if (queueOk != null && !string.IsNullOrWhiteSpace(queueOk.QueueName) && !string.IsNullOrWhiteSpace(item.Exchange))
                        {
                            // 绑定队列
                            channel.QueueBind(queueOk.QueueName,item.Exchange, item.RoutingKey, null);
                        }
                    }
                }

                return channel;
            });
        }
        static void AddSerializer<TSerializer>(IServiceCollection services) where TSerializer : IMessageSerializer
        {
            services.TryAddSingleton(typeof(IMessageSerializer), typeof(TSerializer));
        }
        static void AddMessager(IServiceCollection services)
        {
            services.TryAddSingleton<IRabbitMessager, DefaultRabbitMessager>();
        }
    }
}
