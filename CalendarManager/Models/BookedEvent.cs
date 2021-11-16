using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CalendarManager.Models {

    public class BookedEvent {

        [JsonPropertyName("startHour")]
        public int StartHour { get; set; }

        [JsonPropertyName("startMinute")]
        public int StartMinute { get; set; }

        [JsonPropertyName("endHour")]
        public int EndHour { get; set; }

        [JsonPropertyName("endMinute")]
        public int EndMinute { get; set; }

        [JsonPropertyName("day")]
        public string Day { get; set; }

        [JsonPropertyName("date")]
        public int Date { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        }
}
