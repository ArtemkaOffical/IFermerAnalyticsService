using IFermerAnalyticsService.Data;
using IFermerAnalyticsService.Data.Dto.Response;
using IFermerAnalyticsService.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace IFermerAnalyticsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnaliticsDataController
    {
        private AnalyticsDbContext _analyticsDbContext;
        private IHttpClientFactory _httpClientFactory;

        public AnaliticsDataController(AnalyticsDbContext analyticsDb, IHttpClientFactory httpClientFactory)
        {
            _analyticsDbContext = analyticsDb;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public ActionResult<object> GetSalesData()
        {
            List<TicketDto> tickets = new List<TicketDto>()
            {
                new TicketDto
                {
                    Product = new ProductDto(){
                        DateRegistration = DateTime.Now,
                        Category = "Da",
                        Name = "gd",
                        Position = "a"
                    },
                    DeliveryDate = DateTime.Now.AddDays(-2),
                    Count = 50,
                    DeliveryType = TypeCount.KG,
                    Price = 800,
                },
                new TicketDto
                {
                    Product = new ProductDto(){
                        DateRegistration = DateTime.Now,
                        Category = "Da",
                        Name = "gd",
                        Position = "a"
                    },
                    DeliveryDate = DateTime.Now.AddDays(-3),
                    Count = 510,
                    DeliveryType = TypeCount.KG,
                    Price = 4647,
                },
                new TicketDto
                {
                    Product = new ProductDto(){
                        DateRegistration = DateTime.Now,
                        Category = "Da",
                        Name = "gd",
                        Position = "a"
                    },
                    DeliveryDate = DateTime.Now.AddDays(-6),
                    Count = 65,
                    DeliveryType = TypeCount.KG,
                    Price = 324,
                },
                new TicketDto
                {
                    Product = new ProductDto(){
                        DateRegistration = DateTime.Now,
                        Category = "Da",
                        Name = "gd",
                        Position = "a"
                    },
                    DeliveryDate = DateTime.Now.AddDays(-4),
                    Count = 200,
                    DeliveryType = TypeCount.KG,
                    Price = 1200,
                },
                new TicketDto
                {
                    Product = new ProductDto(){
                        DateRegistration = DateTime.Now,
                        Category = "Da",
                        Name = "gd",
                        Position = "a"
                    },
                    DeliveryDate = DateTime.Now.AddDays(-5),
                    Count = 300,
                    DeliveryType = TypeCount.KG,
                    Price = 1800,
                },
            };

            var bitmap = BitMapCreator.CreateProductBitMap(tickets);

            bitmap.Save("products.png", System.Drawing.Imaging.ImageFormat.Png);
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return new { bytes = ms.ToArray(), text = JsonConvert.SerializeObject(tickets, Formatting.Indented) };
        }

        private DateTime GetDateFromUnix(long sec)
        {
            return DateTimeOffset.FromUnixTimeSeconds(sec).DateTime.Date;
        }

        [HttpGet]
        [Route("GetUsers")]
        public ActionResult<object> GetUsersData(string? startTime, string? endTime)
        {
            Users users = new Users();
            List<UserDto> userResponses = new List<UserDto>();

            WebRequest.Get(_httpClientFactory, $"http://{Environment.GetEnvironmentVariable("BACK_HOST")}:8080/api/auth/all", (statusCode, response) =>
            {
                if (statusCode == 200)
                {
                    users = JsonConvert.DeserializeObject<Users>(response);
                }
            });

            string title = string.Empty;

            if (startTime == null && endTime == null)
            {
                userResponses = users.UsersList;
                title = $"Кол-во пользователей за всё время";
            }
            else
            {
                DateTime startTimes = DateTime.Parse(startTime);
                DateTime endTimes = DateTime.Parse(endTime);

                userResponses = users.UsersList
                   .Where(user => GetDateFromUnix(user.DateRegistration) >= startTimes && GetDateFromUnix(user.DateRegistration) <= endTimes)
                    .ToList();
                title = $"Кол-во пользователей за период с {startTime} по {endTime}";
            }

           var bitmap = BitMapCreator.CreateUsersBitMap(userResponses, title);

            bitmap.Save("users.png", System.Drawing.Imaging.ImageFormat.Png);
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return new { bytes = ms.ToArray(), text = JsonConvert.SerializeObject(userResponses, Formatting.Indented) };
        }

    }
}




