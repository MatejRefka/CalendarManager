using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarManager.Models {

    public class BookedEventsResponse {

        public string ShortUrl { get; set; }

        public string UserEmail { get; set; }

        public string BookedEvents { get; set; }

        public bool EnforcePeriod { get; set; }

        public string PeriodName { get; set; }

        public string Period { get; set; }
    }
}
