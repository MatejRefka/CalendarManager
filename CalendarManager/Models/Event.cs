using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarManager.Models {


    public class Event {

        public string Name { get; set; }

        public string ColorID { get; set; }

        public string StartDayOfWeek { get; set; }

        public int StartDate { get; set; }

        public int StartMonth { get; set; }

        public int StartYear { get; set; }

        public int StartHour { get; set; }

        public int StartMinute { get; set; }

        public string EndDayOfWeek { get; set; }

        public int EndDate { get; set; }

        public int EndMonth { get; set; }

        public int EndYear { get; set; }

        public int EndHour { get; set; }

        public int EndMinute { get; set; }

    }

}
