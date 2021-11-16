using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarManager.Models {

    public class ChosenEventPost {

        public ChosenEventPost() {
            this.Start = new EventDateTime() {
                TimeZone = "Europe/Paris"
            };
            this.End = new EventDateTime() {
                TimeZone = "Europe/Paris"
            };
        }


        public string Summary { get; set; }

        public EventDateTime Start { get; set; }

        public EventDateTime End { get; set; }

    }

    public class EventDateTime {

        public string DateTime { get; set; }

        public string TimeZone { get; set; }

    }
}
