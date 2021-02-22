namespace Calamus.RabbitMQ
{
    /// <summary>
    /// RabbitMQ 配置项
    /// </summary>
    public class RabbitConnectionOptions
    {
        public RabbitConnectionOptions()
        {
            HostName = "localhost";
            Port = 5672;
            UserName = "guest";
            Password = "guest";
            VirtualHost = "/";
        }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientProvidedName { get; set; }
        /// <summary>
        /// 主机IP，默认localhost
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 连接端口，默认5672
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 虚拟域，默认 /，可看作分组
        /// </summary>
        public string VirtualHost { get; set; }
        /// <summary>
        /// 授权用户，默认 guest
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 授权密码，默认 guest
        /// </summary>
        public string Password { get; set; }
    }
}
