using System.Text;
using WebMuses.PME.Core;

namespace WebMuses.PME.Samples.Helpers
{
    public class PrintHelper
    {
        public string PrintTable(CoffeeShop[] shops)
        {
            StringBuilder result = new StringBuilder();
            result.Append("Count = " + shops.Length);

            result.Append("<table>");
            result.Append("<tr><th>Id</th><th>Name</th><th>Phone</th></tr>");
            //foreach (CoffeeShop shop in shops)
            for (int i = 0; i < shops.Length; i++)
            {
                string backgroundColor;
                switch (shops[i].Open)
                {
                    case 600:
                        backgroundColor = "lawngreen";
                        break;
                    case 700:
                        backgroundColor = "lightgreen";
                        break;
                    case 730:
                        backgroundColor = "forestgreen";
                        break;
                    case 800:
                        backgroundColor = "darkgreen";
                        break;
                    default:
                        backgroundColor = "white";
                        break;
                }
                result.Append("<tr style='background-color:" + backgroundColor + "'>" 
                    + "<td>" + (i+1) + "</td><td>"
                    + shops[i].DisplayName + "</td><td>" + shops[i].Phone + "</td></tr>");
            }
            result.Append("</table>");

            return result.ToString();
        }
    }
}