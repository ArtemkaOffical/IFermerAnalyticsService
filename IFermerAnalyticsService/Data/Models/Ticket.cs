using IFermerAnalyticsService.Data.Dto.Response;

namespace IFermerAnalyticsService.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        
        
        public int DeliveryType { get; set; }
        public long Date { get; set; }
        public string PaymentType { get; set; }
        public ProductDto Product { get; set; }
        public double Count { get; set; }
    }
}
