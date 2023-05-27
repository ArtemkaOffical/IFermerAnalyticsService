using IFermerAnalyticsService.Data.Dto.Response;
using System;
using System.Diagnostics;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IFermerAnalyticsService.Utils
{
    public static class BitMapCreator
    {
        public static Bitmap CreateUsersBitMap(List<UserDto> userResponses, string title)
        {
            Dictionary<DateTime, int> userCountByDate = new Dictionary<DateTime, int>();

            foreach (var user in userResponses)
            {
                DateTime registrationDate = DateTimeOffset.FromUnixTimeSeconds(user.DateRegistration).Date;
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
                earliestDeliveryDate = DateTimeOffset.FromUnixTimeSeconds(userResponses.Min(t => t.DateRegistration)).Date;
                latestDeliveryDate = DateTimeOffset.FromUnixTimeSeconds(userResponses.Max(t => t.DateRegistration)).Date;


                if (earliestDeliveryDate == latestDeliveryDate)
                {
                    earliestDeliveryDate = earliestDeliveryDate.AddDays(-1);
                    latestDeliveryDate = latestDeliveryDate.AddDays(1);
                }
            }
            else
            {
                return null;
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

                for (int i = 0; i < userCountByDate.Count; i++)
                {
                    int x = chartX + (int)((DateTimeOffset.FromUnixTimeSeconds(userResponses[i].DateRegistration).Date - earliestDeliveryDate).TotalDays * chartWidth / (latestDeliveryDate - earliestDeliveryDate).TotalDays);
                    int yCount = (int)(chartY + chartHeight - userCountByDate[DateTimeOffset.FromUnixTimeSeconds(userResponses[i].DateRegistration).Date] * chartHeight / maxCount);

                    graphics.FillEllipse(Brushes.Blue, x - 3, yCount - 3, 6, 6);

                    string countLabel = userCountByDate[DateTimeOffset.FromUnixTimeSeconds(userResponses[i].DateRegistration).Date].ToString();

                    SizeF countLabelSize = graphics.MeasureString(countLabel, axisLabelFont);

                    graphics.DrawString(countLabel, axisLabelFont, Brushes.Black, x - countLabelSize.Width / 2, yCount - countLabelSize.Height - 5);

                
                }

                int maxDivisions = 5; // Максимальное количество делений на оси X

                // Вычисление интервала деления на оси X
                TimeSpan xInterval = (latestDeliveryDate - earliestDeliveryDate) / maxDivisions;

                for (int i = 0; i <= maxDivisions; i++)
                {
                    DateTime divisionDate = earliestDeliveryDate + (xInterval * i);
                    int x = chartX + (int)((divisionDate - earliestDeliveryDate).TotalDays * chartWidth / (latestDeliveryDate - earliestDeliveryDate).TotalDays);

                    graphics.DrawLine(Pens.Black, x, chartY + chartHeight, x, chartY + chartHeight + 5);

                    string divisionLabel = divisionDate.ToString("d");
                    SizeF divisionLabelSize = graphics.MeasureString(divisionLabel, axisLabelFont);

                    graphics.DrawString(divisionLabel, axisLabelFont, Brushes.Black, x - divisionLabelSize.Width / 2, chartY + chartHeight + 5);
                }

                for (int i = 1; i < userCountByDate.Count; i++)
                {
                    int x1 = chartX + (int)((DateTimeOffset.FromUnixTimeSeconds(userResponses[i-1].DateRegistration).Date - earliestDeliveryDate).TotalDays * chartWidth / (latestDeliveryDate - earliestDeliveryDate).TotalDays);
                    int yCount1 = (int)(chartY + chartHeight - userCountByDate[DateTimeOffset.FromUnixTimeSeconds(userResponses[i-1].DateRegistration).Date] * chartHeight / maxCount);

                    int x2 = chartX + (int)((DateTimeOffset.FromUnixTimeSeconds(userResponses[i].DateRegistration).Date - earliestDeliveryDate).TotalDays * chartWidth / (latestDeliveryDate - earliestDeliveryDate).TotalDays);
                    int yCount2 = (int)(chartY + chartHeight - userCountByDate[DateTimeOffset.FromUnixTimeSeconds(userResponses[i].DateRegistration).Date] * chartHeight / maxCount);

                    graphics.DrawLine(Pens.Blue, x1, yCount1, x2, yCount2);
                }


                SizeF titleSize = graphics.MeasureString(title, titleFont);
                int titleX = chartX + (chartWidth - (int)titleSize.Width) / 2;
                int titleY = chartY - (int)titleSize.Height - 10;

                graphics.DrawString(title, titleFont, Brushes.Black, titleX, titleY);
                return bitmap;
            }
        }

        public static Bitmap CreateCategoryBitMap(List<TicketDto> tickets,string title)
        {
          

            // Определение размеров графика
            int width = 800;
            int height = 500;
            int axisPadding = 50;
            int chartWidth = width - 2 * axisPadding;
            int chartHeight = height - 2 * axisPadding;

            // Создание Bitmap для рисования графика
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphicsContext = Graphics.FromImage(bitmap);
            // Установка фона графика
            graphicsContext.Clear(Color.White);

            // Определение значений по оси X (названия категорий)
            var jsonList = tickets;
            var categories = jsonList.Select(item => (string)item.Product.Category).Distinct().ToList();
            int categoryCount = Math.Min(10, categories.Count); // Выбор 10 самых популярных категорий
            List<string> selectedCategories = categories.OrderByDescending(category => jsonList.Sum(item => (int)item.Count)).Take(categoryCount).ToList();

            // Определение значений по оси Y (сумма count каждой категории)
            var dataPoints = new List<int>();
            foreach (string category in selectedCategories)
            {
                int count = jsonList.Where(item => (string)item.Product.Category == category).Sum(item => (int)item.Count);
                dataPoints.Add(count);
            }

            // Определение максимального значения по оси Y
            int maxYValue = dataPoints.Count > 0 ? dataPoints.Max() : 0;

            // Рисование осей X и Y
            graphicsContext.DrawLine(Pens.Black, axisPadding, height - axisPadding, axisPadding, axisPadding); // Ось Y
            graphicsContext.DrawLine(Pens.Black, axisPadding, height - axisPadding, width - axisPadding, height - axisPadding); // Ось X

            // Рисование значений по оси X
            Font axisFont = new Font("Arial", 8);
            float xStep = (float)chartWidth / categoryCount;
            for (int i = 0; i < categoryCount; i++)
            {
                float x = axisPadding + i * xStep;
                float y = height - axisPadding + 5;
                string category = selectedCategories[i];
                graphicsContext.DrawString(category, axisFont, Brushes.Black, x, y, new StringFormat() { Alignment = StringAlignment.Center });
            }

            // Рисование значений по оси Y
            int yTickCount = 5; // Количество делений по оси Y
            float yStep = (float)chartHeight / (yTickCount - 1);
            for (int i = 0; i < yTickCount; i++)
            {
                float x = axisPadding - 30;
                float y = height - axisPadding - i * yStep;
                int value = (int)Math.Round((float)i / (yTickCount - 1) * maxYValue);
                graphicsContext.DrawString(value.ToString(), axisFont, Brushes.Black, x, y, new StringFormat() { Alignment = StringAlignment.Far });
            }

            // Отображение данных в виде графика
            Pen lineColor = new Pen(Color.Blue, 2); // Использование более широкого пера
            SolidBrush pointColor = new SolidBrush(Color.Blue); // Цвет точек
            int pointSize = 6; // Размер точек

            for (int i = 1; i < categoryCount; i++)
            {
                float x1 = axisPadding + (i - 1) * xStep;
                float y1 = height - axisPadding - (float)dataPoints[i - 1] / maxYValue * chartHeight;
                float x2 = axisPadding + i * xStep;
                float y2 = height - axisPadding - (float)dataPoints[i] / maxYValue * chartHeight;

                // Рисование линии
                graphicsContext.DrawLine(lineColor, x1, y1, x2, y2);

                // Отображение точек
                graphicsContext.FillEllipse(pointColor, x1 - pointSize / 2, y1 - pointSize / 2, pointSize, pointSize);
                graphicsContext.FillEllipse(pointColor, x2 - pointSize / 2, y2 - pointSize / 2, pointSize, pointSize);
            }
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            SolidBrush titleColor = new SolidBrush(Color.Black);
            float titleX = width / 2 - graphicsContext.MeasureString(title, titleFont).Width / 2;
            float titleY = axisPadding / 2 - graphicsContext.MeasureString(title, titleFont).Height / 2;

            // Отображение заголовка
            graphicsContext.DrawString(title, titleFont, titleColor, titleX, titleY);
            return bitmap;
        }

    }
}
