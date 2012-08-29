using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BsdsApi
{
    public class BsdsAccess
    {
        private string _bingMapsKey;
        private string _dataSourceId = "20181f26d9e94c81acdf9496133d4f23";
        private string _dataSourceName = "FourthCoffeeSample";
        private string _dataEntityName = "FourthCoffeeShops";
        private string _urlBase = "http://spatial.virtualearth.net/REST/v1/data";
        private string _radiusSearchTemplate = "{0}/{1}/{2}/{3}?spatialFilter=nearby({4},{5},{6})&key={7}";

        public BsdsAccess(string bingMapsKey)
        {
            _bingMapsKey = bingMapsKey;
        }

        public IList<CoffeeShop> FindByAreaRadius(double latitude, double longitude, double radiusInKms)
        {
            string requestUrl = string.Format(_radiusSearchTemplate, _urlBase, _dataSourceId,
                _dataSourceName, _dataEntityName, latitude.ToString(CultureInfo.InvariantCulture),
                longitude.ToString(CultureInfo.InvariantCulture), radiusInKms.ToString(CultureInfo.InvariantCulture),
                _bingMapsKey);
            XmlDocument response = GetXmlResponse(requestUrl);
            return ProcessEntityElements(response);
        }

        private IList<CoffeeShop> ProcessEntityElements(XmlDocument response)
        {
            var result = new List<CoffeeShop>();
            XmlNodeList entryElements = response.GetElementsByTagName("entry");
            for (int i = 0; i <= entryElements.Count - 1; i++)
            {
                XmlElement element = (XmlElement)entryElements[i];
                XmlElement contentElement = (XmlElement)element.GetElementsByTagName("content")[0];
                XmlElement propElement = (XmlElement)contentElement.GetElementsByTagName("m:properties")[0];
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
            return ((XmlElement)element.GetElementsByTagName(tag)[0]).InnerText;
        }

        private double GetDoubleValue(XmlElement element, string tag) 
        {
            var stringValue = GetStringValue(element, tag);
            double doubleValue = 0;
            Double.TryParse(stringValue, out doubleValue);
            return doubleValue;
        }

        private int GetIntValue(XmlElement element, string tag)
        {
            var stringValue = GetStringValue(element, tag);
            int intValue = 0;
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
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return (xmlDoc);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
