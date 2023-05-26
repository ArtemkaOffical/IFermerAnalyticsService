namespace IFermerAnalyticsService.RabbitMqService.Messages.Inrefaces
{
    public interface IMessageObject
    {
        public string TypeMessageName { get; set; }
        public void Update();
        public void Add();
        public void Remove();
    }
}
