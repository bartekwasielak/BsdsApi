using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace WebMuses.PME.Samples2.Controllers
{
    public class CoffeeShop
    {
        public string EntityId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AddressLine { get; set; }
        public string PrimaryCity { get; set; }
        public string SecondaryCity { get; set; }
        public string Subdivision { get; set; }
        public string CountryRegion { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Manager { get; set; }
        public string StoreOpen { get; set; }
        public string StoreType { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsWiFiHotSpot { get; set; }
        public int SeatingCapacity { get; set; }
        public bool IsWheelChairAccessible { get; set; }
        public bool AcceptsOnlineOrders { get; set; }
        public bool AcceptsCoffeeCards { get; set; }
        public int Open { get; set; }
        public int Close { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

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
                    + "<td>" + (i + 1) + "</td><td>"
                    + shops[i].DisplayName + "</td><td>" + shops[i].Phone + "</td></tr>");
            }
            result.Append("</table>");

            return result.ToString();
        }
    }

    public class HomeController : Controller
    {
        private readonly string _bingMapsKey = "Av1Pxhxmw1q2Pa8yYeRoO6nRSQttINrDGcmvmPfHzAfokdT0alyVHecHDPNC0oAO";
        private const string DataSourceId = "20181f26d9e94c81acdf9496133d4f23";
        private const string DataSourceName = "FourthCoffeeSample";
        private const string DataEntityName = "FourthCoffeeShops";
        private const string UrlBase = "http://spatial.virtualearth.net/REST/v1/data";
        private const string RadiusSearchTemplate = "{0}/{1}/{2}/{3}?spatialFilter=nearby({4},{5},{6})&key={7}";
        private const string GetIdTemplate = "{0}/{1}/{2}/{3}({4})?key={5}";

        public CoffeeShop[] FindByAreaRadius(double latitude, double longitude, double radiusInKms)
        {
            string requestUrl = string.Format(RadiusSearchTemplate, UrlBase, DataSourceId,
                DataSourceName, DataEntityName, latitude.ToString(CultureInfo.InvariantCulture),
                longitude.ToString(CultureInfo.InvariantCulture), radiusInKms.ToString(CultureInfo.InvariantCulture),
                _bingMapsKey);
            XmlDocument response = GetXmlResponse(requestUrl);
            return ProcessEntityElements(response).ToArray();
        }

        public CoffeeShop GetCoffeeShopById(int id)
        {
            if (id > 0)
            {
                id *= -1;
            }
            string requestUrl = string.Format(GetIdTemplate, UrlBase, DataSourceId, DataSourceName, DataEntityName, id,
                                              _bingMapsKey);
            XmlDocument response = GetXmlResponse(requestUrl);
            var list = ProcessEntityElements(response);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        private IList<CoffeeShop> ProcessEntityElements(XmlDocument response)
        {
            var result = new List<CoffeeShop>();
            XmlNodeList entryElements = response.GetElementsByTagName("entry");
            for (int i = 0; i <= entryElements.Count - 1; i++)
            {
                var element = (XmlElement)entryElements[i];
                var contentElement = (XmlElement)element.GetElementsByTagName("content")[0];
                var propElement = (XmlElement)contentElement.GetElementsByTagName("m:properties")[0];
                result.Add(new CoffeeShop { 
                    EntityId = GetStringValue(propElement, "d:EntityID"), 
                    Latitude = GetDoubleValue(propElement, "d:Latitude"),
                    Longitude = GetDoubleValue(propElement, "d:Longitude"),
                    AddressLine = GetStringValue(propElement, "d:AddressLine"),
                    PrimaryCity = GetStringValue(propElement, "d:PrimaryCity"),
                    SecondaryCity = GetStringValue(propElement, "d:SecondaryCity"),
                    Subdivision = GetStringValue(propElement, "d:Subdivision"),
                    CountryRegion = GetStringValue(propElement, "d:CountryRegion"),
                    PostalCode = GetStringValue(propElement, "d:PostalCode"),
                    Phone = GetStringValue(propElement, "d:Phone"),
                    Manager = GetStringValue(propElement, "d:Manager"),
                    StoreOpen = GetStringValue(propElement, "d:StoreOpen"),
                    StoreType = GetStringValue(propElement, "d:StoreType"),
                    Name = GetStringValue(propElement, "d:Name"),
                    DisplayName = GetStringValue(propElement, "d:DisplayName"), 
                    IsWiFiHotSpot = GetBooleanValue(propElement, "d:IsWiFiHotSpot"),
                    SeatingCapacity = GetIntValue(propElement, "d:SeatingCapacity"),
                    IsWheelChairAccessible = GetBooleanValue(propElement, "d:IsWheelchairAccessible"),
                    AcceptsOnlineOrders = GetBooleanValue(propElement, "d:AcceptsOnlineOrders"),
                    AcceptsCoffeeCards = GetBooleanValue(propElement, "d:AcceptsCoffeeCards"),
                    Open = GetIntValue(propElement, "d:Open"),
                    Close = GetIntValue(propElement, "d:Close"),
                    CreatedDate = GetDateTimeValue(propElement, "d:CreatedDate"),
                    LastUpdatedDate = GetDateTimeValue(propElement, "d:LastUpdatedDate")
                });
            }
            return result;
        }

        private string GetStringValue(XmlElement element, string tag)
        {
            return element.GetElementsByTagName(tag)[0].InnerText;
        }

        private double GetDoubleValue(XmlElement element, string tag) 
        {
            var stringValue = GetStringValue(element, tag);
            double doubleValue;
            Double.TryParse(stringValue, out doubleValue);
            return doubleValue;
        }

        private int GetIntValue(XmlElement element, string tag)
        {
            var stringValue = GetStringValue(element, tag);
            int intValue;
            Int32.TryParse(stringValue, out intValue);
            return intValue;
        }

        private bool GetBooleanValue(XmlElement element, string tag)
        {
            var stringValue = GetStringValue(element, tag);
            return stringValue == "1";
        }

        private DateTime GetDateTimeValue(XmlElement element, string tag)
        {
            var stringValue = GetStringValue(element, tag);
            DateTime dateTimeValue;
            DateTime.TryParse(stringValue, out dateTimeValue);
            return dateTimeValue;
        }

        private static XmlDocument GetXmlResponse(string requestUrl)
        {
            try
            {
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                if (request == null)
                {
                    return null;
                }
                var response = request.GetResponse() as HttpWebResponse;
                if (response == null)
                {
                    return null;
                }

                var xmlDoc = new XmlDocument();
                var responseStream = response.GetResponseStream();
                if (responseStream == null)
                {
                    return null;
                }

                xmlDoc.Load(responseStream);
                return (xmlDoc);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ActionResult Index()
        {
            ViewData["Message"] = "Task 1";
            ViewData["Text"] = GetCoffeeShopName(13);
            return View();
        }

        public ActionResult IndexId(int id)
        {
            ViewData["Message"] = "Task 2";
            ViewData["Text"] = GetCoffeeShopName(id);
            return View("Index");
        }

        public ActionResult IndexWifi(int id)
        {
            ViewData["Message"] = "Task 3";
            ViewData["Text"] = GetCoffeeShopNameAndMoreDetails(id);
            return View("Index");
        }

        public ActionResult IndexMulti(double lat, double lon, double r)
        {
            ViewData["Message"] = "Task 4";
            CoffeeShop[] shops = FindByAreaRadius(lat, lon, r);

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
            ViewData["Text"] = printHelper.PrintTable(shops);
            return View("Index");
        }


        private string GetCoffeeShopName(int id)
        {
            CoffeeShop result = GetCoffeeShopById(id);
            string displayName = result.DisplayName;
            return displayName;
        }

        private string GetCoffeeShopNameAndMoreDetails(int id)
        {
            CoffeeShop shop = GetCoffeeShopById(id);
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
