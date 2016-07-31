using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RabbitMQ.Client;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace LPush.RabbitMQ
{
    public class QueueSender
    {
        private IConnection connection;
        public QueueSender(string hostName)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            this.connection = factory.CreateConnection();
        }

        private void SendMessage<T>(IModel channel, string queue, T request) where T : new()
        {
            T instance = new T();
            channel.QueueDeclare(queue: queue,
                                   durable: true,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);

            string message = JsonConvert.SerializeObject(request);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = false;
            //properties.Priority = 9;

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: queue,
                                 basicProperties: properties,
                                 body: body);
        }
    }
}
