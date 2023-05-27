using IFermerAnalyticsService.Data;
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
        public long DeliveryDate { get; set; }
        public TypeCount DeliveryType { get; set; }

       
        public TicketRabbitDto()
        {
            TypeMessageName = this.GetType().Name;
        }

        public void Add(AnalyticsDbContext analyticsDbContext)
        {
            analyticsDbContext.Tickets.Add(new Data.Models.Ticket()
            {
                Product = this.Product,
                Count = this.Count,
                Date = this.DeliveryDate,
                DeliveryType = (int)this.DeliveryType
            });

            analyticsDbContext.SaveChanges();
        }

        public void Remove(AnalyticsDbContext analyticsDbContext)
        {
            var ticket = analyticsDbContext.Tickets.FirstOrDefault(x => x.Product.Id == this.Product.Id && x.Count == this.Count);

            if (ticket == null) 
                return;

            analyticsDbContext.Tickets.Remove(ticket);
            analyticsDbContext.SaveChanges();
        }

        public void Update(AnalyticsDbContext analyticsDbContext)
        {
            var ticket = analyticsDbContext.Tickets.FirstOrDefault(x => x.Product.Id == this.Product.Id && x.Count == this.Count);

            if (ticket == null)
                return;

            ticket.Count = this.Count;
            ticket.Product = this.Product;
            ticket.Date = this.DeliveryDate;
            ticket.DeliveryType = (int)this.DeliveryType;

            analyticsDbContext.SaveChanges();
        }
    }
}
