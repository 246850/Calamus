using System;
using System.Text;

namespace Calamus.RabbitMQ
{
    /// <summary>
    /// IRabbitMessager Subscribe 扩展
    /// </summary>
    public static partial class IRabbitMessagerExtensions
    {
        /// <summary>
        /// 订阅/消费消息
        /// </summary>
        /// <param name="messager"></param>
        /// <param name="queueName"></param>
        /// <param name="func"></param>
        public static void Subscribe(this IRabbitMessager messager, string queueName, Action<string> func)
            => messager.Subscribe(queueName, bytes => func(Encoding.UTF8.GetString(bytes.ToArray())));

        /// <summary>
        /// 订阅/消费消息
        /// </summary>
        /// <typeparam name="T">泛型 T</typeparam>
        /// <param name="messager"></param>
        /// <param name="queueName"></param>
        /// <param name="func"></param>
        public static void Subscribe<T>(this IRabbitMessager messager, string queueName, Action<T> func) where T : class
            => messager.Subscribe(queueName, bytes => func(messager.Serializer.Deserialize<T>(bytes.ToArray())));
    }
}
