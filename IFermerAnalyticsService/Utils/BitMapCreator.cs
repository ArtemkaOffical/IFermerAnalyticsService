using IFermerAnalyticsService.Data.Dto.Response;
using System.Drawing;

namespace IFermerAnalyticsService.Utils
{
    public static class BitMapCreator
    {
        public static Bitmap CreateUsersBitMap(List<UserDto> userResponses, string title)
        {
            Dictionary<DateTime, int> userCountByDate = new Dictionary<DateTime, int>();

            foreach (var user in userResponses)
            {
                DateTime registrationDate = new DateTime(user.DateRegistration);
                if (userCountByDate.ContainsKey(registrationDate))
                {
                    userCountByDate[registrationDate]++;
                }
                else
                {
                    userCountByDate[registrationDate] = 1;
                }
            }
            int width = 800;
            int height = 500;

            Bitmap bitmap = new Bitmap(width, height);
            DateTime earliestDeliveryDate = DateTime.MinValue;
            DateTime latestDeliveryDate = DateTime.MinValue;
            if (userResponses.Count != 0)
            {
                earliestDeliveryDate = DateTimeOffset.FromUnixTimeSeconds(userResponses.Min(t => t.DateRegistration)).DateTime.Date;
                latestDeliveryDate = DateTimeOffset.FromUnixTimeSeconds(userResponses.Max(t => t.DateRegistration)).DateTime.Date;

                if (earliestDeliveryDate == latestDeliveryDate)
                {
                    earliestDeliveryDate = earliestDeliveryDate.AddDays(-1);
                    latestDeliveryDate = latestDeliveryDate.AddDays(1);
                }
            }
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);

                Font axisLabelFont = new Font("Arial", 12, FontStyle.Regular);
                Font titleFont = new Font("Arial", 16, FontStyle.Bold);

                int maxCount = userResponses.Count;

                int chartX = 80;
                int chartY = 50;
                int chartWidth = width - 2 * chartX;
                int chartHeight = height - 2 * chartY;

                graphics.DrawLine(Pens.Black, chartX, chartY, chartX, chartY + chartHeight);
                graphics.DrawLine(Pens.Black, chartX, chartY + chartHeight, chartX + chartWidth, chartY + chartHeight);

                userResponses = userResponses.OrderBy(t => t.DateRegistration).ToList();
                Dictionary<long, int> cc = new Dictionary<long, int>();

                foreach (var item in userResponses)
                {
                    if (cc.ContainsKey(item.DateRegistration))
                        cc[item.DateRegistration]++;
                    else
                        cc.Add(item.DateRegistration, 1);
                }

                for (int i = 0; i < userResponses.Count; i++)
                {
                    int x = chartX + (int)((DateTimeOffset.FromUnixTimeSeconds(userResponses[i].DateRegistration).DateTime - earliestDeliveryDate).TotalDays * chartWidth / (latestDeliveryDate - earliestDeliveryDate).TotalDays);
                    int yCount = (int)(chartY + chartHeight - cc[userResponses[i].DateRegistration] * chartHeight / maxCount);

                    graphics.FillEllipse(Brushes.Blue, x - 3, yCount - 3, 6, 6);

                    string countLabel = cc[userResponses[i].DateRegistration].ToString();
                    string dateLabel = DateTimeOffset.FromUnixTimeSeconds(userResponses[i].DateRegistration).DateTime.ToString("d");

                    SizeF countLabelSize = graphics.MeasureString(countLabel, axisLabelFont);
                    SizeF dateLabelSize = graphics.MeasureString(dateLabel, axisLabelFont);

                    graphics.DrawString(countLabel, axisLabelFont, Brushes.Black, x - countLabelSize.Width / 2, yCount - countLabelSize.Height - 5);

                    graphics.DrawString(dateLabel, axisLabelFont, Brushes.Black, x - dateLabelSize.Width / 2, chartY + chartHeight + 5);
                }

                for (int i = 1; i < userResponses.Count; i++)
                {
                    int x1 = chartX + (int)((DateTimeOffset.FromUnixTimeSeconds(userResponses[i - 1].DateRegistration).DateTime - earliestDeliveryDate).TotalDays * chartWidth / (latestDeliveryDate - earliestDeliveryDate).TotalDays);
                    int yCount1 = (int)(chartY + chartHeight - cc[userResponses[i - 1].DateRegistration] * chartHeight / maxCount);

                    int x2 = chartX + (int)((DateTimeOffset.FromUnixTimeSeconds(userResponses[i].DateRegistration).DateTime - earliestDeliveryDate).TotalDays * chartWidth / (latestDeliveryDate - earliestDeliveryDate).TotalDays);
                    int yCount2 = (int)(chartY + chartHeight - cc[userResponses[i].DateRegistration] * chartHeight / maxCount);

                    graphics.DrawLine(Pens.Blue, x1, yCount1, x2, yCount2);
                }


                SizeF titleSize = graphics.MeasureString(title, titleFont);
                int titleX = chartX + (chartWidth - (int)titleSize.Width) / 2;
                int titleY = chartY - (int)titleSize.Height - 10;

                graphics.DrawString(title, titleFont, Brushes.Black, titleX, titleY);
                return bitmap;
            }
        }

        public static Bitmap CreateProductBitMap(List<TicketDto> tickets)
        {
            DateTime earliestDeliveryDate = tickets.Min(t => t.DeliveryDate);

            int width = 800;
            int height = 500;

            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);

                Font axisLabelFont = new Font("Arial", 12, FontStyle.Regular);
                Font titleFont = new Font("Arial", 16, FontStyle.Bold);

                int maxCount = (int)tickets.Max(t => t.Count);
                int maxPrice = (int)tickets.Max(t => t.Price);

                int chartX = 80;
                int chartY = 50;
                int chartWidth = width - 2 * chartX;
                int chartHeight = height - 2 * chartY;

                graphics.DrawLine(Pens.Black, chartX, chartY, chartX, chartY + chartHeight);
                graphics.DrawLine(Pens.Black, chartX, chartY + chartHeight, chartX + chartWidth, chartY + chartHeight);

                tickets = tickets.OrderBy(t => t.DeliveryDate).ToList();
                for (int i = 0; i < tickets.Count; i++)
                {
                    int x = chartX + (int)((tickets[i].DeliveryDate - earliestDeliveryDate).TotalDays * chartWidth / (DateTime.Now - earliestDeliveryDate).TotalDays);
                    int yCount = (int)(chartY + chartHeight - tickets[i].Count * chartHeight / maxCount);
                    int yPrice = (int)(chartY + chartHeight - tickets[i].Price * chartHeight / maxPrice);

                    graphics.FillEllipse(Brushes.Blue, x - 3, yCount - 3, 6, 6);
                    graphics.FillEllipse(Brushes.Red, x - 3, yPrice - 3, 6, 6);

                    string countLabel = tickets[i].Count.ToString();
                    string priceLabel = tickets[i].Price.ToString();
                    string dateLabel = tickets[i].DeliveryDate.ToString("d");

                    SizeF countLabelSize = graphics.MeasureString(countLabel, axisLabelFont);
                    SizeF priceLabelSize = graphics.MeasureString(priceLabel, axisLabelFont);
                    SizeF dateLabelSize = graphics.MeasureString(dateLabel, axisLabelFont);

                    graphics.DrawString(countLabel, axisLabelFont, Brushes.Black, x - countLabelSize.Width / 2, yCount - countLabelSize.Height - 5);
                    graphics.DrawString(priceLabel, axisLabelFont, Brushes.Black, x - priceLabelSize.Width / 2, yPrice + 5);
                    graphics.DrawString(dateLabel, axisLabelFont, Brushes.Black, x - dateLabelSize.Width / 2, chartY + chartHeight + 5);
                }

                for (int i = 1; i < tickets.Count; i++)
                {
                    int x1 = chartX + (int)((tickets[i - 1].DeliveryDate - earliestDeliveryDate).TotalDays * chartWidth / (DateTime.Now - earliestDeliveryDate).TotalDays);
                    int yCount1 = (int)(chartY + chartHeight - tickets[i - 1].Count * chartHeight / maxCount);
                    int yPrice1 = (int)(chartY + chartHeight - tickets[i - 1].Price * chartHeight / maxPrice);

                    int x2 = chartX + (int)((tickets[i].DeliveryDate - earliestDeliveryDate).TotalDays * chartWidth / (DateTime.Now - earliestDeliveryDate).TotalDays);
                    int yCount2 = (int)(chartY + chartHeight - tickets[i].Count * chartHeight / maxCount);
                    int yPrice2 = (int)(chartY + chartHeight - tickets[i].Price * chartHeight / maxPrice);

                    graphics.DrawLine(Pens.Blue, x1, yCount1, x2, yCount2);
                    graphics.DrawLine(Pens.Red, x1, yPrice1, x2, yPrice2);
                }


                string title = "Продажи товаров по датам доставки";
                SizeF titleSize = graphics.MeasureString(title, titleFont);
                int titleX = chartX + (chartWidth - (int)titleSize.Width) / 2;
                int titleY = chartY - (int)titleSize.Height - 10;

                graphics.DrawString(title, titleFont, Brushes.Black, titleX, titleY);

                int squareSize = 10;
                int squareSpacing = 5;
                int squareY = chartY - squareSize - 20;


                int descX = chartX - 20;
                int descSpacing = 15;
                int descYBlue = squareY + squareSize + descSpacing;
                int descYRed = descYBlue + descSpacing;


                Rectangle blueSquareRect = new Rectangle(descX, squareY, squareSize, squareSize);
                graphics.FillRectangle(Brushes.Blue, blueSquareRect);
                graphics.DrawString("Количество", axisLabelFont, Brushes.Black, descX + squareSize + squareSpacing, squareY);


                Rectangle redSquareRect = new Rectangle(descX, descYBlue, squareSize, squareSize);
                graphics.FillRectangle(Brushes.Red, redSquareRect);
                graphics.DrawString("Цена", axisLabelFont, Brushes.Black, descX + squareSize + squareSpacing, descYBlue);

                return bitmap;
            }
        }
    }
}
