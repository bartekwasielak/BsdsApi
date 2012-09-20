using System;

namespace BsdsApi
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
}
