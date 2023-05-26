using IFermerAnalyticsService.Data.Dto.Response;
using IFermerAnalyticsService.RabbitMqService.Messages.Inrefaces;
using System.Diagnostics;

namespace IFermerAnalyticsService.RabbitMqService.Messages.Dto.Response
{
    public class TicketRabbitDto : IMessageObject
    {
        public string TypeMessageName { get; set; }
        public ProductDto Product { get; set; }
        public double Count { get; set; }
        public DateTime DeliveryDate { get; set; }
        public TypeCount DeliveryType { get; set; }

       
        public TicketRabbitDto()
        {
            TypeMessageName = this.GetType().Name;
        }

        public void Add()
        {
            Debug.Write("Get");
        }

        public void Remove()
        {
            Debug.Write("Remove");
        }

        public void Update()
        {
            Debug.Write("Update");
        }
    }
}
