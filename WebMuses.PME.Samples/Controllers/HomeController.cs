using System.Collections.Generic;
using System.Web.Mvc;
using WebMuses.PME.Core;
using WebMuses.PME.Samples.Helpers;

namespace WebMuses.PME.Samples.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Task 1";
            ViewBag.Text = GetCoffeeShopName(13);
            return View();
        }

        public ActionResult IndexId(int id)
        {
            ViewBag.Message = "Task 2";
            ViewBag.Text = GetCoffeeShopName(id);
            return View("Index");
        }

        public ActionResult IndexWifi(int id)
        {
            ViewBag.Message = "Task 3";
            ViewBag.Text = GetCoffeeShopNameAndMoreDetails(id);
            return View("Index");
        }

        public ActionResult IndexMulti(double lat, double lon, double r)
        {
            ViewBag.Message = "Task 4";
            BsdsAccess bsdsAccess = new BsdsAccess();
            CoffeeShop[] shops = bsdsAccess.FindByAreaRadius(lat, lon, r);

            //List<CoffeeShop> openShops = new List<CoffeeShop>();
            //foreach (CoffeeShop shop in shops)
            //{
            //    if (shop.Open == 800)
            //    {
            //        openShops.Add(shop);
            //    }
            //}
            //shops = openShops.ToArray();

            PrintHelper printHelper = new PrintHelper();
            ViewBag.Text = printHelper.PrintTable(shops);
            return View("Index");
        }


        private string GetCoffeeShopName(int id)
        {
            BsdsAccess bsdsAccess = new BsdsAccess();
            CoffeeShop result = bsdsAccess.GetCoffeeShopById(id);
            string displayName = result.DisplayName;
            return displayName;
        }

        private string GetCoffeeShopNameAndMoreDetails(int id)
        {
            BsdsAccess bsdsAccess = new BsdsAccess();
            CoffeeShop shop = bsdsAccess.GetCoffeeShopById(id);
            string result = shop.DisplayName;
            if (shop.IsWiFiHotSpot)
            {
                result = result + " and it has a wifi connection!";
            }
            else
            {
                result = result + " but there is no wifi :(";
            }
            if (shop.SeatingCapacity < 10)
            {
                result = result + " This is quite a small shop.";
            } 
            else if (shop.SeatingCapacity < 20)
            {
                result = result + " This is a medium shop.";
            } 
            else
            {
                result = result + " This is a huge shop!";
            }
            if (shop.SeatingCapacity > 20 && shop.AcceptsCoffeeCards)
            {
                result = result + "It's a good place for our party!";
            }
            return result;
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
