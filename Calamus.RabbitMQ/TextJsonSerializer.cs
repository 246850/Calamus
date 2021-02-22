using System.Text.Json;

namespace Calamus.RabbitMQ
{
    public class TextJsonSerializer : IMessageSerializer
    {
        public T Deserialize<T>(byte[] bytes) => JsonSerializer.Deserialize<T>(bytes);
        public byte[] Serialize(object source) => JsonSerializer.SerializeToUtf8Bytes(source);
    }
}
