using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ContainerApp.Domain
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Event
    {
        public Guid Id { get; }

        public DateTime Timestamp { get; }

        public string Longitude { get; }

        public string Latitude { get; }

        public Event(Guid id, DateTime timestamp, string longitude, string latitude)
        {
            this.Id = id;
            this.Timestamp = timestamp;
            this.Longitude = longitude;
            this.Latitude = latitude;
        }
    }
}