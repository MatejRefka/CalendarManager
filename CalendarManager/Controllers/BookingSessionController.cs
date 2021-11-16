using CalendarManager.Data;
using CalendarManager.Models;
using CalendarManager.Services;
using CalendarManagerLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarManager.Controllers {

    public class BookingSessionController : Controller {

        #region Helper fields
        private ApplicationDbContext context;
        private EventFormatter formatter;
        private Rounder rounder;
        #endregion Helper fields

        #region Helper Properties
        public string ShortURL { get; set; }

        public string UserEmail { get; set; }

        public string BookedEvents { get; set; }

        public bool EnforcePeriod { get; set; }

        public string PeriodName { get; set; }

        public int Period { get; set; }

        public List<BookedEvent> RetrievedBookings { get; set; } = new List<BookedEvent>();
        #endregion Helper Properties

        #region Constructor
        public BookingSessionController(ApplicationDbContext context, EventFormatter formatter, Rounder rounder) {

            this.context = context;
            this.formatter = formatter;
            this.rounder = rounder;
        }
        #endregion Constructor


        public IActionResult Index(String id = "") {

            using (context) {

                //if shortURL exists in the DB, it is valid
                var result = context.URLs.SingleOrDefault(entry => entry.ShortURL == id);
                if (result != null) {

                    //retrieve short URL info
                    ShortURL = result.ShortURL;
                    UserEmail = result.Email;
                    EnforcePeriod = result.EnforcePeriods;
                    PeriodName = result.PeriodName;
                    Period = result.Period;

                    //retrieve all event mapping to this Short URL
                    var bookingResults = context.BookedEvents.Where(entry => entry.ShortURL == ShortURL);

                    foreach (var i in bookingResults) {

                        BookedEvent tempEvent = new BookedEvent();

                        tempEvent.StartHour = i.StartHour;
                        tempEvent.StartMinute = i.StartMinute;
                        tempEvent.EndHour = i.EndHour;
                        tempEvent.EndMinute = i.EndMinute;
                        tempEvent.Day = i.Day;
                        tempEvent.Date = i.Date;
                        tempEvent.Month = i.Month;
                        tempEvent.Year = i.Year;

                        RetrievedBookings.Add(tempEvent);
                    }
                }
                else {
                    return RedirectToAction("Error", "BookingSession");
                }
            }

            EventDeserializer eventConverter = new EventDeserializer(rounder);
            
            ViewBag.BookingsList = eventConverter.BookedEventToEvent(RetrievedBookings);
            ViewBag.EnforcePeriod = EnforcePeriod;
            ViewBag.PeriodName = PeriodName;
            ViewBag.Email = UserEmail;

            return View();
        }

        public IActionResult Error() {

            return View();
        }

    }
}
