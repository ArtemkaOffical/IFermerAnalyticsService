using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using IFermerAnalyticsService.RabbitMqService.Messages.Inrefaces;

namespace IFermerAnalyticsService.RabbitMqService.Messages
{
    public enum TypeMessage
    {
        ADD,
        UPDATE,
        REMOVE,
    }

    public class MessageRabbit
    {
        
        public TypeMessage typeMessage { get; set; }

        [JsonConverter(typeof(Utils.JsonConverter.MessageConverter))]
        public IMessageObject Object { get; set; }
               
    }
}
