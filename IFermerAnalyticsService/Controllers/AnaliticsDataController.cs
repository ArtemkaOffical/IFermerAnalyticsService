using IFermerAnalyticsService.Data;
using IFermerAnalyticsService.Data.Dto.Response;
using IFermerAnalyticsService.Data.Models;
using IFermerAnalyticsService.Settings;
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
        public ActionResult<object> GetSalesData(string position = "Рязань")
        {

            List<TicketDto> tickets = new List<TicketDto>();
            WebRequest.Get(_httpClientFactory, $"{Connections.URL}api/delivery/search/position?position=" + position, (statusCode, response) =>
            {
                if (statusCode == 200)
                {
                    tickets = JsonConvert.DeserializeObject<List<TicketDto>>(response);
                }
            });
            var title = $"Количество товаров по категориям для области {position}";
            var bitmap = BitMapCreator.CreateCategoryBitMap(tickets, title);
            return CreateAndReturnImageData(bitmap, tickets);

        }
        private object CreateAndReturnImageData(Bitmap bitmap, object data)
        {
            bitmap.Save(data.ToString()+".png", System.Drawing.Imaging.ImageFormat.Png);
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return new
            {
                bytes = ms.ToArray(),
                text = JsonConvert.SerializeObject(data, Formatting.Indented)
            };
        }

        private DateTime GetDateFromUnix(long sec)
        {
            return DateTimeOffset.FromUnixTimeSeconds(sec).Date;
        }

        [HttpGet]
        [Route("GetUsers")]
        public ActionResult<object> GetUsersData(string? startTime, string? endTime)
        {
            Users users = new Users();
            List<UserDto> userResponses = new List<UserDto>();

            WebRequest.Get(_httpClientFactory, $"{Connections.URL}api/auth/all", (statusCode, response) =>
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
                DateTime startTimes = DateTime.Parse(startTime).Date;
                DateTime endTimes = DateTime.Parse(endTime).Date;

                userResponses = users.UsersList
                   .Where(user => GetDateFromUnix(user.DateRegistration) >= startTimes && GetDateFromUnix(user.DateRegistration) <= endTimes)
                    .ToList();
                title = $"Кол-во пользователей за период с {startTime} по {endTime}";
            }

            var bitmap = BitMapCreator.CreateUsersBitMap(userResponses, title);
            if (bitmap == null)
            {
                return new { bytes = "", text = "" };
            }

            return CreateAndReturnImageData(bitmap, userResponses);
        }

        [HttpGet]
        [Route("GetProducts")]
        public ActionResult<object> GetProductsData()
        {
            List<TicketDto>? products = new List<TicketDto>();

            WebRequest.Get(_httpClientFactory, $"{Connections.URL}api/delivery/all", (statusCode, response) =>
                {
                    if (statusCode == 200)
                    {
                        products = JsonConvert.DeserializeObject<List<TicketDto>>(response);
                    }
                });

            var bitmap = BitMapCreator.CreateCategoryBitMap(products, "Статистика по продажам товаров всех категорий");
            return CreateAndReturnImageData(bitmap, products);
        }
    }
}




