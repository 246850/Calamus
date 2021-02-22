using System.Diagnostics.CodeAnalysis;
using System.Text;
using RabbitMQ.Client;

namespace Calamus.RabbitMQ
{
    /// <summary>
    /// IRabbitMessager Direct 扩展
    /// </summary>
    public static partial class IRabbitMessagerExtensions
    {
        /// <summary>
        /// 发布消息 - 采用默认交换机 ExchangeType = Direct
        /// </summary>
        /// <param name="messager"></param>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <param name="properties"></param>
        public static void Publish(this IRabbitMessager messager, string routingKey, string body, IBasicProperties properties = null)
            => Publish(messager, string.Empty, routingKey, body, properties);

        /// <summary>
        /// 发布消息 - 采用自定义名称交换机 ExchangeType = Direct
        /// </summary>
        /// <param name="messager"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <param name="properties"></param>
        public static void Publish(this IRabbitMessager messager, string exchange, string routingKey, string body, IBasicProperties properties = null)
            => messager.Publish(exchange, ExchangeType.Direct, routingKey, Encoding.UTF8.GetBytes(body), properties);

        /// <summary>
        /// 发布消息 - 采用默认交换机 ExchangeType = Direct
        /// </summary>
        /// <typeparam name="T">泛型 T</typeparam>
        /// <param name="messager"></param>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <param name="properties"></param>
        public static void Publish<T>(this IRabbitMessager messager, string routingKey, [NotNull] T body, IBasicProperties properties = null) where T : class
            => Publish(messager, string.Empty, routingKey, body, properties);

        /// <summary>
        /// 发布消息 - 采用自定义名称交换机 ExchangeType = Direct
        /// </summary>
        /// <typeparam name="T">泛型 T</typeparam>
        /// <param name="messager"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <param name="properties"></param>
        public static void Publish<T>(this IRabbitMessager messager, string exchange, string routingKey, [NotNull] T body, IBasicProperties properties = null) where T : class
        {
            byte[] bytes = messager.Serializer.Serialize(body);
            messager.Publish(exchange, ExchangeType.Direct, routingKey, bytes, properties);
        }
    }
}
