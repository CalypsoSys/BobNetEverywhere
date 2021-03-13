using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bobdomain
{
    public class GraphData
    {
        public static string GetRandomColor()
        {
            string letters = "0123456789ABCDEF";
            StringBuilder color = new StringBuilder("#");
            Random rnd = new Random();
            for (int i = 0; i < 6; i++)
            {
                color.Append(letters[rnd.Next(0, 15)]);
            }
            return color.ToString();
        }
       
        public object BuildGraphData(string chartType)
        {
            string[] labels = new string[] { "Item1", "Item2", "Item3", "Item4", "Item5", "Item6" };
            List<object> datasets = new List<object>();
            int[] chartData = new int[] { 12, 19, 3, 5, 2, 3 };
            string[] colors = new string[] { GetRandomColor(), GetRandomColor(), GetRandomColor(), GetRandomColor(), GetRandomColor(), GetRandomColor() };
            datasets.Add(new { label = "# of Votes", data = chartData, borderWidth = 1, backgroundColor = colors, borderColor = colors });
            var data = new { labels = labels, datasets = datasets };

            /*
            List<object> ticks = new List<object>();
            var zero = new { beginAtZero = true };
            ticks.Add(new { tick = zero });
            var yAxes = new { yAxes = ticks };
            var options = new { scales = yAxes };
            */
            return data;
        }
    }
}
