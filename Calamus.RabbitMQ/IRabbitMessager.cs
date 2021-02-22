using System;
using RabbitMQ.Client;

namespace Calamus.RabbitMQ
{
    /// <summary>
    /// RabbitMQ 消息处理接口
    /// </summary>
    public interface IRabbitMessager:IDisposable
    {
        /// <summary>
        /// 消息内容序列化
        /// </summary>
        IMessageSerializer Serializer { get; }
        /// <summary>
        /// 发布/生产消息
        /// </summary>
        /// <param name="exchange">交换机名称</param>
        /// <param name="exchangeType">交换机类型 - direct topic fanout headers</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="body">消息内容</param>
        /// <param name="properties">消息附加属性</param>
        void Publish(string exchange, string exchangeType, string routingKey, ReadOnlyMemory<byte> body, IBasicProperties properties = null);

        /// <summary>
        /// 订阅/消费消息
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="func"></param>
        void Subscribe(string queue, Action<ReadOnlyMemory<byte>> func);

        /// <summary>
        /// 主动获取消息
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="autoAck"></param>
        /// <returns></returns>
        BasicGetResult BasicGet(string queue, bool autoAck = true);
    }
}
