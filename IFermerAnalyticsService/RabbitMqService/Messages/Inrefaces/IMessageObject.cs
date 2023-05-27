using IFermerAnalyticsService.Data;

namespace IFermerAnalyticsService.RabbitMqService.Messages.Inrefaces
{
    public interface IMessageObject
    {
        public string TypeMessageName { get; set; }
        public void Update(AnalyticsDbContext analyticsDbContext);
        public void Add(AnalyticsDbContext analyticsDbContext);
        public void Remove(AnalyticsDbContext analyticsDbContext);
    }
}
