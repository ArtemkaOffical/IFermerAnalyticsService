using IFermerAnalyticsService.RabbitMqService.Messages.Inrefaces;

namespace IFermerAnalyticsService.Data.Dto.Response
{
    public class TicketDto
    {
        public ProductDto Product { get; set; }
        public double Count { get; set; }   
        public double Price { get; set; }   
        public DateTime DeliveryDate { get; set; }   
        public TypeCount DeliveryType { get; set; }
    }

    public enum TypeCount { KG, UNIT }
}
