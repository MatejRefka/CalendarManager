using CalendarManager.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using CalendarManagerLibrary;

namespace CalendarManager.Services {
    public class EventDeserializer {

        #region Helper fields
        private Rounder rounder;
        #endregion Helper fields

        #region Constructor
        public EventDeserializer(Rounder rounder) {
            this.rounder = rounder;
        }
        #endregion Constructor

        #region Parsing JObject into a list of Events
        public List<Event> ConvertToEventsList(JObject jsonEventResponse) {

            string jsonString = jsonEventResponse.ToString();

            AllEventsJson allEventsJson = JsonSerializer.Deserialize<AllEventsJson>(jsonString);

            //Console.WriteLine($"kind: {eventJson.Kind}");
            jsonEventResponse.ToString();

            List<Event> eventsList = new List<Event>();

            foreach(EventJson eventJson in allEventsJson.EventsList) {

                Event passedEvent = new Event {
                    Name = eventJson.Summary,
                    ColorID = eventJson.ColorId,
                    StartDayOfWeek = eventJson.Start.StartDateTime.DayOfWeek.ToString(),
                    StartDate = eventJson.Start.StartDateTime.Day,
                    StartMonth = eventJson.Start.StartDateTime.Month - 1,
                    StartYear = eventJson.Start.StartDateTime.Year,
                    StartHour = eventJson.Start.StartDateTime.TimeOfDay.Hours,
                    StartMinute = rounder.RoundToLowestFive(eventJson.Start.StartDateTime.TimeOfDay.Minutes),
                    EndDayOfWeek = eventJson.End.EndDateTime.DayOfWeek.ToString(),
                    EndDate = eventJson.End.EndDateTime.Day,
                    EndMonth = eventJson.End.EndDateTime.Month - 1,
                    EndYear = eventJson.End.EndDateTime.Year,
                    EndHour = eventJson.End.EndDateTime.TimeOfDay.Hours,
                    EndMinute = rounder.RoundToLowestFive(eventJson.End.EndDateTime.TimeOfDay.Minutes)
                };

                eventsList.Add(passedEvent);
            }

            return eventsList;

        }
        #endregion Parsing JObject into a list of Events

        #region Parsing JObject to retrieve user email
        public string ConvertToUserEmail(JObject jsonEventResponse) {
            
            string jsonString = jsonEventResponse.ToString();

            AllEventsJson allEventsJson = JsonSerializer.Deserialize<AllEventsJson>(jsonString);

            return allEventsJson.email;
        }
        #endregion Parsing JObject to retrieve user email

        #region Parsing list of BookedEvent into list of Event 
        public List<Event> BookedEventToEvent(List<BookedEvent> bookings) {

            List<Event> eventsList = new List<Event>();

            foreach (var i in bookings) {

                Event tempEvent = new Event();

                tempEvent.Name = "Booking Slot";
                tempEvent.ColorID = "5";
                tempEvent.StartDayOfWeek = i.Day;
                tempEvent.StartDate = i.Date;
                tempEvent.StartMonth = i.Month;
                tempEvent.StartYear = i.Year;
                tempEvent.StartHour = i.StartHour;
                tempEvent.StartMinute = i.StartMinute;
                tempEvent.EndDayOfWeek = i.Day;
                tempEvent.EndDate = i.Date;
                tempEvent.EndMonth = i.Month;
                tempEvent.EndYear = i.Year;
                tempEvent.EndHour = i.EndHour;
                tempEvent.EndMinute = i.EndMinute;

                eventsList.Add(tempEvent);
            }

            return eventsList;
        }
        #endregion Parsing list of BookedEvent into list of Event 

        #region Parsing ChosenEventResponse model into ChosenEventPost model
        public ChosenEventPost ResponseEventToPostEvent(ChosenEventResponse responseEvent) {

            ChosenEventPost postEvent = new ChosenEventPost();

            postEvent.Summary = responseEvent.PeriodName;
            
            postEvent.Start.TimeZone = "Europe/London";

            postEvent.End.TimeZone = "Europe/London";

            string startMin;
            if (responseEvent.StartMinute < 10) {
                startMin = "0" + responseEvent.StartMinute.ToString();
            }
            else {
                startMin = responseEvent.StartMinute.ToString();
            }
            string startHour;
            if (responseEvent.StartHour < 10) {
                startHour = "0" + responseEvent.StartHour.ToString();
            }
            else {
                startHour = responseEvent.StartHour.ToString();
            }
            
            string month;
            if (responseEvent.Month < 10) {
                month = "0" + (responseEvent.Month +1).ToString();
            }
            else {
                month = (responseEvent.Month + 1).ToString();
            }
            string date;
            if(responseEvent.Date < 10) {
                date = "0" + responseEvent.Date.ToString();
            }
            else {
                date = responseEvent.Date.ToString();
            }

            string endMin;
            if (responseEvent.EndMinute < 10) {
                endMin = "0" + responseEvent.EndMinute.ToString();
            }
            else {
                endMin = responseEvent.EndMinute.ToString();
            }
            string endHour;
            if (responseEvent.EndHour < 10) {
                endHour = "0" + responseEvent.EndHour.ToString();
            }
            else {
                endHour = responseEvent.EndHour.ToString();
            }

            //format: "2021-08-24T15:41:36-01:00"
            postEvent.Start.DateTime = responseEvent.Year.ToString() + "-" + month + "-" + date + "T" + startHour + ":" + startMin + ":00+00:00";
            
            postEvent.End.DateTime = responseEvent.Year.ToString() + "-" + month + "-" + date + "T" + endHour + ":" + endMin + ":00+00:00";

            return postEvent;
        }




        #endregion Parsing ChosenEventResponse model into ChosenEventPost model

    }
}
