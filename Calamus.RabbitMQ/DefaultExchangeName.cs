namespace Calamus.RabbitMQ
{
    /// <summary>
    /// RabbitMQ 默认交换器名称
    /// </summary>
    public sealed class DefaultExchangeName
    {
        /// <summary>
        /// amq.direct - ExchangeType:direct
        /// </summary>
        public static readonly string AMQ_DIRECT = "amq.direct";
        /// <summary>
        /// amq.fanout - ExchangeType:fanout
        /// </summary>
        public static readonly string AMQ_FANOUT = "amq.fanout";
        /// <summary>
        /// amq.headers - ExchangeType:headers
        /// </summary>
        public static readonly string AMQ_HEADERS = "amq.headers";
        /// <summary>
        /// amq.match - ExchangeType:headers 
        /// </summary>
        public static readonly string AMQ_MATCH = "amq.match";
        /// <summary>
        /// amq.rabbitmq.trace - ExchangeType:topic 
        /// </summary>
        public static readonly string AMQ_RABBITMQ_TRACE = "amq.rabbitmq.trace";
        /// <summary>
        /// amq.topic - ExchangeType:topic 
        /// </summary>
        public static readonly string AMQ_TOPIC = "amq.topic";
    }
}
