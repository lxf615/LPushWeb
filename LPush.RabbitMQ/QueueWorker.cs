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
    /// <summary>
    /// 队列接收器
    /// </summary>
    public class QueueWorker
    {
        private IConnection connection;
        public QueueWorker(string hostName)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            this.connection = factory.CreateConnection();
        }

        /// <summary>
        /// 定义一个队列接收处理,处理失败重新入队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="handler"></param>
        public void DeclareQueue<T>(string queue, Func<T, Task<bool>> handler) where T : new()
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    T request = JsonConvert.DeserializeObject<T>(message);

                    if (await handler(request))
                    {
                        //确认收到消息,删除元素
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    else
                    {
                        ////通知队列处理失败,重新入队
                        //ea.BasicProperties.Priority = 8;
                        //channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                        channel.BasicReject(ea.DeliveryTag, true);
                    }
                };
                channel.BasicConsume(queue: queue,
                                     noAck: false,
                                     consumer: consumer);

            }
        }
    }
}

