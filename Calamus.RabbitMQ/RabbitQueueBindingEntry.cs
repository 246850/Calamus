namespace Calamus.RabbitMQ
{
    public class RabbitQueueBindingEntry
    {
        /// <summary>
        /// 交换机名
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// 交换机类型 - direct topic fanout headers
        /// </summary>
        public string ExchangeType { get; set; }
        /// <summary>
        /// 队列名 - direct类型时 Queue 应等于 RoutingKey
        /// </summary>
        public string Queue { get; set; }
        /// <summary>
        /// 路由键
        /// </summary>
        public string RoutingKey { get; set; }
    }
}
