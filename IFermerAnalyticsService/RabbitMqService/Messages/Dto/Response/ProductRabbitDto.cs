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

        public void Add()
        {
            Debug.Write("Add");
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
