using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Calamus.RabbitMQ
{
    public class DefaultRabbitMessager : IRabbitMessager
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IMessageSerializer _messageSerializer;
        private readonly ILogger<IRabbitMessager> _logger;

        public DefaultRabbitMessager(IConnection connection, IModel channel) : this(connection, channel, null, null)
        {

        }
        public DefaultRabbitMessager(IConnection connection, IModel channel, IMessageSerializer messageSerializer) : this(connection, channel, messageSerializer, null)
        {
        }
        public DefaultRabbitMessager(IConnection connection, IModel channel, ILogger<IRabbitMessager> logger) : this(connection, channel, null, logger)
        {

        }
        public DefaultRabbitMessager(IConnection connection, IModel channel, IMessageSerializer messageSerializer, ILogger<IRabbitMessager> logger)
        {
            //  Connection 事件
            connection.CallbackException += (object sender, CallbackExceptionEventArgs args) =>
            {
                _logger?.LogError(args.Exception, "RabbitMQ连接回调异常CallbackException", args.Detail.ToArray());
            };
            connection.ConnectionShutdown += (object sender, ShutdownEventArgs args) =>
            {
                _logger?.LogError("ConnectionShutdown：" + args.ReplyText);
            };
            connection.ConnectionBlocked += (object sender, ConnectionBlockedEventArgs args) =>
            {
                _logger?.LogError("ConnectionBlocked：" + args.Reason);
            };
            connection.ConnectionUnblocked += (object sender, EventArgs args) =>
            {
                _logger?.LogInformation($"ConnectionUnblocked重新连接：{sender}");
            };
            // Channel 事件
            channel.CallbackException += (object sender, CallbackExceptionEventArgs args) =>
            {
                _logger?.LogError(args.Exception, "RabbitMQ通道回调异常：", args.Detail.ToArray());
            };
            channel.ModelShutdown += (object sender, ShutdownEventArgs args) =>
            {
                _logger?.LogError("ModelShutdown：" + args.ReplyText);
            };

            _messageSerializer = messageSerializer;
            _connection = connection;
            _channel = channel;
            _logger = logger;
        }

        public IMessageSerializer Serializer
        {
            get { return _messageSerializer ?? throw new Exception("未配置IMessageSerializer消息序列化实现"); }
        }

        public void Publish(string exchange, string exchangeType, string routingKey, ReadOnlyMemory<byte> body, IBasicProperties properties = null)
        {
            if (!string.IsNullOrWhiteSpace(exchange))
            {
                _channel.ExchangeDeclare(exchange, exchangeType, true, false, null);    // 定义交换机
            }
            if (string.Equals(exchangeType, ExchangeType.Direct, StringComparison.InvariantCultureIgnoreCase))
            {
                // direct 类型 队列名称 通常定义为 路由键
                QueueDeclareOk declareOk = _channel.QueueDeclare(routingKey, true, false, false, null);
                if (declareOk != null && !string.IsNullOrWhiteSpace(declareOk.QueueName) && !string.IsNullOrWhiteSpace(exchange))
                {
                    _channel.QueueBind(declareOk.QueueName, exchange, routingKey, null);
                }
            }

            if (properties == null)
            {
                properties = _channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;
            }
            _channel.BasicPublish(exchange, routingKey, false, properties, body);
        }

        public void Subscribe(string queue, Action<ReadOnlyMemory<byte>> func)
        {
            _channel.BasicQos(0, 10, false);    // 服务质量，这里设置一次只消费10条消息
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                // Polly 5秒重试机制，消息重新入队
                Policy.Handle<Exception>(ex =>
                {
                    _logger?.LogError($"消息消费异常：{ex.Message}", new { Exchange = args.Exchange, RoutingKey = args.RoutingKey });
                    return true;
                }).WaitAndRetry(1, retryCount => TimeSpan.Zero, (ex, timeSpan) =>
                {
                    _channel.BasicNack(args.DeliveryTag, false, true); // 消费失败，重新入队

                }).Execute(() =>
                {
                    func(args.Body);
                    _channel.BasicAck(args.DeliveryTag, false);
                });
            };
            // this consumer tag identifies the subscription
            // when it has to be cancelled
            string consumerTag = _channel.BasicConsume(queue, false, consumer);
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }

        public BasicGetResult BasicGet(string queue, bool autoAck = true)
        {
            return _channel.BasicGet(queue, autoAck);
        }
    }
}
