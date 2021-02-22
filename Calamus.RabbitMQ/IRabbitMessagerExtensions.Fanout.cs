using System.Diagnostics.CodeAnalysis;
using System.Text;
using RabbitMQ.Client;

namespace Calamus.RabbitMQ
{
    /// <summary>
    /// IRabbitMessager Fanout 扩展
    /// </summary>
    public static partial class IRabbitMessagerExtensions
    {
        /// <summary>
        /// 广播/发布消息 ExchangeType = Fanout，忽略 RoutingKey 路由规则
        /// </summary>
        /// <param name="messager"></param>
        /// <param name="exchange"></param>
        /// <param name="body"></param>
        /// <param name="properties"></param>
        public static void Fanout(this IRabbitMessager messager, string exchange, string body, IBasicProperties properties = null)
            => messager.Publish(exchange, ExchangeType.Fanout, string.Empty, Encoding.UTF8.GetBytes(body), properties);
        /// <summary>
        /// 广播/发布消息 ExchangeType = Fanout，忽略 RoutingKey 路由规则
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messager"></param>
        /// <param name="exchange"></param>
        /// <param name="body"></param>
        /// <param name="properties"></param>
        public static void Fanout<T>(this IRabbitMessager messager, string exchange, [NotNull] T body, IBasicProperties properties = null) where T : class
        {
            byte[] bytes = messager.Serializer.Serialize(body);
            messager.Publish(exchange, ExchangeType.Fanout, string.Empty, bytes, properties);
        }
    }
}
