using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMusesAgile.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string name = GetCoffeShopName(17);
            ViewData["Message"] = name;
            return View();
        }

        public ActionResult IndexId(int id)
        {
            string name = GetCoffeShopName(id); 
            ViewData["Message"] = name;
            return View("Index");
        }

        public string GetCoffeShopName(int numer)
        {
            DataAccess dataccess = new DataAccess();
            CoffeeShop coffeeshop = dataccess.GetCoffeeShopById(numer);
            string name = coffeeshop.DisplayName;
            return name;
        }


        public ActionResult IndexWifi(int id)
        {
            string name = GetMoreDetails(id);
            ViewData["Message"] = name;
            return View("Index");
        }

        public string GetMoreDetails (int id)
        {
            DataAccess dataccess = new DataAccess();
            CoffeeShop coffeeshop = dataccess.GetCoffeeShopById(id);
            string name = coffeeshop.DisplayName;
            if (coffeeshop.IsWiFiHotSpot)
            {
                name = name + "Tu jest internet";
            }
            else
            {
                name = name + " Tu nie ma internetu";
            }

            if (coffeeshop.SeatingCapacity<10)
            {
                name = name + "; Mala kawawiarnia";
            }
            else if (coffeeshop.SeatingCapacity<20)
            {
                name = name + "; Srednia kawiarnia";
            }
            else
            {
                name = name + "; Duza kawiarnia";
            }

            if (coffeeshop.SeatingCapacity>20 && coffeeshop.AcceptsCoffeeCards)
            {
                name = name + " Kawiarnia dla WebMuses";
            }
            return name;
            
        }
        public ActionResult IndexMulti(double lat, double lon, double r)
        {
            ViewData["Message"] = "Task 4";
            return View("Index");
        }
    }
}
