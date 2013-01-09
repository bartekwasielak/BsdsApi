using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMuses.Core;
using WebMuses65.Helpers;

namespace WebMuses65.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = GetCoffeeShopName(13);
            return View();
        }

        public ActionResult IndexId(int id)
        {
            ViewBag.Message = GetCoffeeShopName(id);
            return View("Index");
        }

        public string GetCoffeeShopName(int id)
        {
            DataAccess dataAccess = new DataAccess();
            CoffeeShop shop = dataAccess.GetCoffeeShopById(id);
            string name = shop.DisplayName;
            return name;
        }

        public ActionResult IndexWifi(int id)
        {
            string name = GetMoreDetails(id);
            ViewBag.Message = name;
            return View("Index");
        }

        public string GetMoreDetails(int id)
        {
            DataAccess dataccess = new DataAccess();
            CoffeeShop coffeeshop = dataccess.GetCoffeeShopById(id);
            string text = coffeeshop.DisplayName;
            if (coffeeshop.IsWiFiHotSpot)
            {
                text = text + "Tu jest internet";
            }
            else
            {
                text = text + " Tu nie ma internetu";
            }

            if (coffeeshop.SeatingCapacity < 10)
            {
                text = text + "; Mala kawiarnia";
            }
            else if (coffeeshop.SeatingCapacity < 20)
            {
                text = text + "; Srednia kawiarnia";
            }
            else
            {
                text = text + "; Duza kawiarnia";
            }

            if (coffeeshop.SeatingCapacity > 20 && coffeeshop.AcceptsCoffeeCards)
            {
                text = text + " Kawiarnia dla WebMuses";
            }

            return text;
        }

        public ActionResult IndexMulti(double lat, double lon, double r)
        {
            DataAccess dataAccess = new DataAccess();
            CoffeeShop[] shops = dataAccess.FindByAreaRadius(lat, lon, r);
            PrinterHelper printerHelper = new PrinterHelper();

            List<CoffeeShop> openShops = new List<CoffeeShop>();
            foreach (CoffeeShop shop in shops)
            {
                if (shop.Open == 800)
                {
                    openShops.Add(shop);
                }
            }
            shops = openShops.ToArray();

            ViewBag.Message = printerHelper.Print(shops);
            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
