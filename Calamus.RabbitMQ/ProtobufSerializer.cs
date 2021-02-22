using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Calamus.RabbitMQ
{
    public class ProtobufSerializer : IMessageSerializer
    {
        public T Deserialize<T>(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }
        public byte[] Serialize(object source)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, source);

                byte[] bytes = stream.ToArray();
                return bytes;
            }
        }
    }
}
