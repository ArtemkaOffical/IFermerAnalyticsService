using IFermerAnalyticsService.Data;
using IFermerAnalyticsService.RabbitMqService.Messages.Inrefaces;
using System.Diagnostics;

namespace IFermerAnalyticsService.RabbitMqService.Messages.Dto.Response
{
    public class ProductRabbitDto : IMessageObject
    {
        public string TypeMessageName { get; set ; }
        public int Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public DateTimeKind DateRegistration { get; set; }

        public ProductRabbitDto()
        {
            TypeMessageName = this.GetType().Name;
        }

        public void Add(AnalyticsDbContext analyticsDbContext)
        {
            Debug.Write("Add");
        }

        public void Remove(AnalyticsDbContext analyticsDbContext)
        {
            Debug.Write("Remove");
        }

        public void Update(AnalyticsDbContext analyticsDbContext)
        {
            Debug.Write("Update");
        }
    }
}
