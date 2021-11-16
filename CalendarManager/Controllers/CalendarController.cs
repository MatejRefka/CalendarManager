using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net;
using Newtonsoft.Json.Linq;
using CalendarManager.Models;
using CalendarManager.Services;
using CalendarManagerLibrary;
using System.Text.Json;
using CalendarManager.Data;

namespace CalendarManager.Controllers {

    public class CalendarController : Controller {

        #region Helper fields
        private readonly ILogger<CalendarController> logger;
        private IHostEnvironment environment;
        private IConfiguration config;
        private Rounder rounder;
        private ApplicationDbContext context;
        private EventFormatter formatter;
        #endregion Helper fields

        #region Helper Properties
        public String UserEmail {get;set;}
        #endregion Helper Properties

        #region Default constructor
        public CalendarController(ILogger<CalendarController> logger, IHostEnvironment environment, IConfiguration config, Rounder rounder, ApplicationDbContext context, EventFormatter formatter) {
            
            this.logger = logger;
            this.environment = environment;
            this.config = config;
            this.rounder = rounder;
            this.context = context;
            this.formatter = formatter;
        }
        #endregion Default constructor

        [HttpGet]
        #region Index page calling API and returning View [GET]
        public IActionResult Index() {

            Console.WriteLine(TempData["test"]);

            //String refreshToken = System.IO.File.ReadAllText(Path.Combine(environment.ContentRootPath, "Files", "RefreshToken.txt"));
            //RequestAccessToken(refreshToken);

            //Retrieve access token
            string accessToken = System.IO.File.ReadAllText(Path.Combine(environment.ContentRootPath, "Files", "AccessToken.txt"));
            //string accessToken = (string)TempData["tempAccessToken"];
            string refreshTokenP = (string)TempData["tempRefreshToken"];

            //The app generates the default "primary" calendar. Merging other user calendars is a further functionality
            String calendarAPIRequestURL = "https://www.googleapis.com/calendar/v3/calendars/primary/events?key=" + config["APIAccess:Google:CalendarAPIKey"];

            //WebRequest class is needed to capture the JSON response
            WebRequest request = WebRequest.Create(calendarAPIRequestURL);

            request.Method = "GET";

            //API uses headers instead of body
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            request.Headers.Add("Accept", "application/json");

            WebResponse response = request.GetResponse();

            //read the response data into a string
            Stream responseDataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(responseDataStream);
            string jsonResponse = reader.ReadToEnd();

            response.Close();

            
            JObject json = JObject.Parse(jsonResponse);

            //Console.WriteLine(json["items"].ToString());

            EventDeserializer eventModel = new EventDeserializer(rounder);

            //If a new refresh token is provided, add to DB
            if (refreshTokenP != null) {

                using (context) {

                    //overwrite refresh token if an entry for the email already exists
                    var result = context.RefreshTokens.SingleOrDefault(entry => entry.Email == eventModel.ConvertToUserEmail(json));
                    if (result != null) {
                        result.Token = refreshTokenP;
                        context.SaveChanges();
                    }
                    //otehrwise add a full new entry
                    else {
                        context.RefreshTokens.Add(new RefreshTokens {
                            Email = eventModel.ConvertToUserEmail(json),
                            Token = refreshTokenP
                        });
                        context.SaveChanges();
                    }
                }
            }

            UserEmail = eventModel.ConvertToUserEmail(json);

            ViewBag.EventsList = eventModel.ConvertToEventsList(json);
            ViewBag.UserEmail = UserEmail;

            if(TempData["url"] != null) {
                TempData["test"] = TempData["url"];
                ViewBag.ShortUrl = ViewData["shortURL"];
            }

            //ViewData.Remove("shortURL");
            
            return View();
        }
        #endregion Index page calling API and returning View

        #region Generate a short URL for a booking session
        
        [HttpPost]
        public IActionResult BookingSession([FromBody]BookedEventsResponse bookedEvents) {

            
            //parse response and deserialize events string into list of BookedEvent
            UserEmail = bookedEvents.UserEmail;
            bool enforcePeriods = bookedEvents.EnforcePeriod;
            string periodName = bookedEvents.PeriodName;
            
            //int period = Int32.Parse(bookedEvents.Period);
            int period;
            bool success = int.TryParse(bookedEvents.Period, out period);
            if (success) {
                period = Int32.Parse(bookedEvents.Period);
                if(period == 1 || period == 2) {
                    period = 0;
                    enforcePeriods = false;
                }
                else {
                    //round to nearest 5
                    double temp = period / 5;
                    period = (int)((Math.Round(temp, 0, MidpointRounding.AwayFromZero)) * 5);
                }
            }
            else {
                period = 0;
                enforcePeriods = false;
            }

            string jsonString = bookedEvents.BookedEvents;
            List<BookedEvent> bookings = JsonSerializer.Deserialize<List<BookedEvent>>(jsonString);

            //chop events into booking slots based on period
            if (enforcePeriods) {

                bookings = formatter.ChopEvents(bookings, period);
            }

            //grab the generated short URL
            String shortURL = bookedEvents.ShortUrl;

            //add URL table entry and Bookings table entries
            using (context) {
                context.URLs.Add(new URLs {
                    ShortURL = shortURL,
                    Email = UserEmail,
                    EnforcePeriods = enforcePeriods,
                    PeriodName = periodName,
                    Period = period
                });
                context.SaveChanges();

                foreach (BookedEvent e in bookings) {
                    context.BookedEvents.Add(new BookedEvents {
                        ShortURL = shortURL,
                        StartHour = e.StartHour,
                        StartMinute = e.StartMinute,
                        EndHour = e.EndHour,
                        EndMinute = e.EndMinute,
                        Day = e.Day,
                        Date = e.Date,
                        Month = e.Month,
                        Year = e.Year
                    });
                }
                context.SaveChanges();

            }

            return RedirectToAction("Index");
        }
        #endregion Generate a short URL for a booking session [POST]

    }
}
