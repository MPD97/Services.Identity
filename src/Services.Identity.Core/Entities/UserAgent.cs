using System;

namespace Services.Identity.Core.Entities
{
    public class UserAgent 
    {
        public AggregateId Id { get; private set; }
        public AggregateId UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string BrowserFamily { get; private set; }
        public string BrowserMajorVersion { get; private set; }
        public string BrowserMinorVersion { get; private set; }
        public string OSFamily { get; private set; }
        public string OSMajorVersion { get; private set; }
        public string OSMinorVersion { get; private set; }
        public string DeviceFamily { get; private set; }
        public string DeviceBrand { get; private set; }
        public string DeviceModel { get; private set; }

        public UserAgent(AggregateId id, AggregateId userId, DateTime createdAt, string browserFamily, 
            string browserMajorVersion, string browserMinorVersion, string osFamily, string osMajorVersion,
            string osMinorVersion, string deviceFamily,
            string deviceBrand, string deviceModel)
        {
            Id = id;
            UserId = userId;
            CreatedAt = createdAt;
            
            BrowserFamily = browserFamily;
            BrowserMajorVersion = browserMajorVersion;
            BrowserMinorVersion = browserMinorVersion;
            OSFamily = osFamily;
            OSMajorVersion = osMajorVersion;
            OSMinorVersion = osMinorVersion;
            DeviceFamily = deviceFamily;
            DeviceBrand = deviceBrand;
            DeviceModel = deviceModel;
        }
    }
}