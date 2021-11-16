using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarManager.Models {

    public class ChosenEventResponse {

        public string Email { get; set; }

        public string PeriodName { get; set; }

        public int StartHour { get; set; }

        public int StartMinute { get; set; }

        public int EndHour { get; set; }

        public int EndMinute { get; set; } 

        public string Day { get; set; }

        public int Date { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

    }
}
