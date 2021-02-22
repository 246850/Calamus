using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calamus.RabbitMQ
{
    /// <summary>
    /// 消息内容序列化 接口
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        byte[] Serialize(object source);
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        T Deserialize<T>(byte[] bytes);
    }
}
