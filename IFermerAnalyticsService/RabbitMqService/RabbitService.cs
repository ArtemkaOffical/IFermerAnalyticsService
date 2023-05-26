using IFermerAnalyticsService.RabbitMqService.Messages;
using IFermerAnalyticsService.RabbitMqService.Messages.Inrefaces;

using Newtonsoft.Json;
using Npgsql.Replication.PgOutput.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace IFermerAnalyticsService.RabbitMqService
{
    public class RabbitService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private ConnectionFactory _factory;

        private string _qeueName = "queue";

        public RabbitService() 
        {
            _factory = new ConnectionFactory() 
            {
                HostName =Environment.GetEnvironmentVariable("RABBIT_HOST"),
                UserName = "username", 
                Password = "password" 
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(_qeueName, true, false, false, null);
        }

        public void Send(string message)
        {
            
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = false;
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish("", _qeueName, properties, body);
        }

        public void Send(object  message)
        {
            string dataMessage = JsonConvert.SerializeObject(message);

            Send(dataMessage);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += Received;

            _channel.BasicConsume(_qeueName, false, consumer);

            return Task.CompletedTask;
        }

        private void Received(object? sender, BasicDeliverEventArgs e)
        {
                var content = Encoding.UTF8.GetString(e.Body.ToArray());
                var message = JsonConvert.DeserializeObject<MessageRabbit>(content);
                if (message.Object != null)
                    ProcessingObject(message.typeMessage, message.Object);

                _channel.BasicAck(e.DeliveryTag, false);
            
        }

        private void ProcessingObject(Messages.TypeMessage typeMessage, IMessageObject obj)
        {
            switch (typeMessage)
            {
                case Messages.TypeMessage.UPDATE:
                    {
                        obj.Update();
                        break;
                    }
                case Messages.TypeMessage.ADD:
                    {
                        obj.Add();
                        break;
                    }
                case Messages.TypeMessage.REMOVE:
                    {
                        obj.Remove();
                        break;
                    }
            }
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
