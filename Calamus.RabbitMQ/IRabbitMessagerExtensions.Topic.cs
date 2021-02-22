using System.Diagnostics.CodeAnalysis;
using System.Text;
using RabbitMQ.Client;

namespace Calamus.RabbitMQ
{
    /// <summary>
    /// IRabbitMessager Topic 扩展
    /// </summary>
    public static partial class IRabbitMessagerExtensions
    {
        /// <summary>
        /// 发布消息 ExchangeType = Topic，根据 RoutingKey 模式匹配 转发到 队列
        /// </summary>
        /// <param name="messager"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey">key.* | key.#，*：匹配一个单词，#：匹配一个/多个单词</param>
        /// <param name="body"></param>
        /// <param name="properties"></param>
        public static void Topic(this IRabbitMessager messager, string exchange, string routingKey, string body, IBasicProperties properties = null)
            => messager.Publish(exchange, ExchangeType.Topic, routingKey, Encoding.UTF8.GetBytes(body), properties);
        /// <summary>
        /// 发布消息 ExchangeType = Topic，根据 RoutingKey 模式匹配 转发到 队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messager"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey">key.* | key.#，*：匹配一个单词，#：匹配一个/多个单词</param>
        /// <param name="body"></param>
        /// <param name="properties"></param>
        public static void Topic<T>(this IRabbitMessager messager, string exchange, string routingKey, [NotNull] T body, IBasicProperties properties = null) where T : class
        {
            byte[] bytes = messager.Serializer.Serialize(body);
            messager.Publish(exchange, ExchangeType.Topic, routingKey, bytes, properties);
        }
    }
}
