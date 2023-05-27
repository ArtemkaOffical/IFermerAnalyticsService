using IFermerAnalyticsService.RabbitMqService.Messages.Inrefaces;

namespace IFermerAnalyticsService.Data.Dto.Response
{
    public class TicketDto
    {
        public int Id { get; set; }
        public int Period { get; set; }
        public int ConsumerId { get; set; }
        public int FarmerId { get; set; }
        public string DeliveryType { get; set; }
        public long Date { get; set; }
        public string AdressFrom { get; set; }
        public string AdressTo { get; set; }
        public string PaymentType { get; set; }
        public ProductDto Product { get; set; }
        public double Count { get; set; }   
    }
}
