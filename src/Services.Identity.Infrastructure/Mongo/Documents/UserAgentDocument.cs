using System;
using Convey.Types;

namespace Services.Identity.Infrastructure.Mongo.Documents
{
    internal sealed class UserAgentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string BrowserFamily { get; set; }
        public string BrowserMajorVersion { get; set; }
        public string BrowserMinorVersion { get; set; }
        public string OSFamily { get; set; }
        public string OSMajorVersion { get; set; }
        public string OSMinorVersion { get; set; }
        public string DeviceFamily { get; set; }
        public string DeviceBrand { get; set; }
        public string DeviceModel { get; set; }
        public string Raw { get; set; }
    }
}