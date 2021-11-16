using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CalendarManager.Models {
    public class AllEventsJson {

        [JsonPropertyName("summary")]
        public string email { get; set; }

        [JsonPropertyName("items")]
        public List<EventJson> EventsList { get; set; }

    }

    public class EventJson {

        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("etag")]
        public string Etag { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("htmlLink")]
        public string HtmlLink { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }


        [JsonPropertyName("colorId")]
        public string ColorId { get; set; }

        [JsonPropertyName("creator")]
        public Creator Creator { get; set; }

        [JsonPropertyName("organizer")]
        public Organizer Organizer { get; set; }

        [JsonPropertyName("start")]
        public Start Start { get; set; }

        [JsonPropertyName("end")]
        public End End { get; set; }

        [JsonPropertyName("iCalUID")]
        public string ICalUID { get; set; }

        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }

        [JsonPropertyName("reminders")]
        public Reminders Reminders { get; set; }

        [JsonPropertyName("eventType")]
        public string EventType { get; set; }

    }

    public class Creator {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("self")]
        public bool Self { get; set; }
    }

    public class Organizer {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("self")]
        public bool Self { get; set; }
    }

    public class Start {
        [JsonPropertyName("dateTime")]
        public DateTime StartDateTime { get; set; }
    }

    public class End {
        [JsonPropertyName("dateTime")]
        public DateTime EndDateTime { get; set; }
    }

    public class Reminders {
        [JsonPropertyName("reminders")]
        public string UseDefaults { get; set; }
    }

}
