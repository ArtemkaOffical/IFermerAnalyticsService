using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using IFermerAnalyticsService.RabbitMqService.Messages.Inrefaces;
using IFermerAnalyticsService.RabbitMqService.Messages.Dto.Response;
using System.Diagnostics;

namespace IFermerAnalyticsService.Utils
{
    public class JsonConverter
    {
        public class MessageConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanWrite => false;
            public override bool CanRead => true;

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
               
            }

            public override object? ReadJson(JsonReader reader, System.Type objectType, object? existingValue, JsonSerializer serializer)
            {
                var jsonObject = JObject.Load(reader);
                
                var message = default(IMessageObject);
                var className = jsonObject["typeMessageName"];
                if (className == null)
                    return null;

                switch (className.ToString())
                {
                    case "ProductAnalyzeDTO":
                        message = new ProductRabbitDto();
                        break;
                    case "DeliveryAnalyzeDTO":
                        message = new TicketRabbitDto();
                        break;
                }
                serializer.Populate(jsonObject.CreateReader(), new object());
                return message;
            }

            public override bool CanConvert(System.Type objectType)
            {
                return objectType == typeof(IMessageObject);
            }
        }
    }
}
