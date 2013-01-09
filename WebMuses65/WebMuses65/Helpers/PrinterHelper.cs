using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMuses.Core;

namespace WebMuses65.Helpers
{
    public class PrinterHelper
    {
        public string Print(CoffeeShop[] shops)
        {
            string result;
            result = "Count: " + shops.Length;

            result = result + "<table>";
            result = result + "<tr><th>#</th><th>Name</th><th>Seats</th></tr>";
            for (int i = 0; i < shops.Length; i++)
            {
                string color;
                switch (shops[i].Open)
                {
                    case 600:
                        color = "lawngreen";
                        break;
                    case 700:
                        color = "lightgreen";
                        break;
                    case 730:
                        color = "forestgreen";
                        break;
                    case 800:
                        color = "darkgreen";
                        break;
                    default:
                        color = "lightblue";
                        break;
                }

                result = result + "<tr style='background-color:" + color +
                        "'><td>" + (i+1) + "</td><td>" + shops[i].DisplayName +
                        "</td><td>" + shops[i].SeatingCapacity + "</td></tr>";
            }
            result = result + "</table>";

            return result;
        }
    }
}