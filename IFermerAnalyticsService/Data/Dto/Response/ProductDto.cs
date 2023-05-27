namespace IFermerAnalyticsService.Data.Dto.Response
{
    public class ProductDto
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public int IdUser { get; set; }
        public int StartSales { get; set; }
        public int EndSales { get; set; }
        public double Price { get; set; }
        public double TradePrice { get; set; }
        public double PriceBoard { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Unit { get; set; }
        public long DateRegistration { get; set; }
    }
    public enum TypeCount { KG, UNIT }
}
