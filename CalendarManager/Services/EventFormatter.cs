using CalendarManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarManager.Services {


    public class EventFormatter {

        #region Constructor
        public EventFormatter() {

        }
        #endregion Constructor

        public List<BookedEvent> ChopEvents(List<BookedEvent> allEvents, int period) {

            List<BookedEvent> formattedEvents = new List<BookedEvent>();

            foreach(var i in allEvents) {

                int startMinutes = (i.StartHour * 60) + i.StartMinute;
                int endMinutes = (i.EndHour * 60) + i.EndMinute;

                while (endMinutes - startMinutes >= period) {

                    BookedEvent tempEvent = new BookedEvent();

                    tempEvent.StartHour = startMinutes/60;
                    tempEvent.StartMinute = startMinutes%60;
                    tempEvent.EndHour = (startMinutes+period)/60;
                    tempEvent.EndMinute = (startMinutes+period)%60;
                    tempEvent.Day = i.Day;
                    tempEvent.Date = i.Date;
                    tempEvent.Month = i.Month;
                    tempEvent.Year = i.Year;

                    startMinutes = startMinutes + period;
                    formattedEvents.Add(tempEvent);
                }

            }

            return formattedEvents;
        }


    }
}
