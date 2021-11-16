using CalendarManager.Data;
using CalendarManager.Models;
using CalendarManager.Services;
using CalendarManagerLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CalendarManager.Controllers {

    public class BookingSessionAPIController : Controller {

        #region Helper fields
        private IConfiguration config;
        private ApplicationDbContext context;
        private IHostEnvironment environment;
        private Rounder rounder;
        #endregion Helper fields

        #region Constructor
        public BookingSessionAPIController(IConfiguration config, ApplicationDbContext context, IHostEnvironment environment, Rounder rounder) {

            this.config = config;
            this.context = context;
            this.environment = environment;
            this.rounder = rounder;
        }


        #endregion Constructor


        public IActionResult Index() {

            return View();
        }


        public IActionResult PostToCalendar([FromBody]ChosenEventResponse chosenEvent) {


            //get event Id from returned values
            Guid Id = new Guid();
            string shortUrl = "";
            ViewBag.Racecondition = "false";
            bool collision = true;
            using (context) {

                var result = context.BookedEvents.FirstOrDefault(entry => (
                    entry.StartMinute == chosenEvent.StartMinute &&
                    entry.StartHour == chosenEvent.StartHour &&
                    entry.EndMinute == chosenEvent.EndMinute &&
                    entry.EndHour == chosenEvent.EndHour &&
                    entry.Date == chosenEvent.Date &&
                    entry.Month == chosenEvent.Month &&
                    entry.Year == chosenEvent.Year));

                if (result != null) {
                    Id = result.Id;
                    collision = false;
                }
            }

            //check if this id exists in the booked events (race condition)
            if(collision == true) {

                throw new Exception("Race Condition");
                return RedirectToAction("Index", "ErrorHandler");
            }

            //get refresh token from user email
            string email = chosenEvent.Email.Substring(1);
            string refreshToken = "";

            using (context) {

                var result = context.RefreshTokens.SingleOrDefault(entry => entry.Email == email);
                refreshToken = result.Token;
            }

            //get access token from refresh token
            string accessToken = RequestAccessToken(refreshToken);

            //Post request to Google API servers
            String calendarAPIRequestURL = "https://www.googleapis.com/calendar/v3/calendars/primary/events?key=" + config["APIAccess:Google:CalendarAPIKey"];

            WebRequest request = WebRequest.Create(calendarAPIRequestURL);

            request.Method = "POST";

            request.Headers.Add("Authorization", "Bearer " + accessToken);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Content-Type", "application/json");

            EventDeserializer deserializer = new EventDeserializer(rounder);

            ChosenEventPost postEvent = deserializer.ResponseEventToPostEvent(chosenEvent);

            var postData = JsonConvert.SerializeObject(postEvent, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/json";

            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try {

                WebResponse response = request.GetResponse();

                //read the response data into a string
                Stream responseDataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(responseDataStream);
                string jsonResponse = reader.ReadToEnd();

                response.Close();

            }
            catch (Exception) {

                throw;

            }

            //delete slot from bookable slots
            using (context) {

                var result = context.BookedEvents.SingleOrDefault(entry => entry.Id == Id);
                context.BookedEvents.Remove(result);
                context.SaveChanges();
            }

            return RedirectToAction("Index", "BookingSession" );
            }



        #region Upon access token expiration, request a new one from the stored refresh token
        public string RequestAccessToken(String refreshToken) {

            String exchangeRequestURL = "https://oauth2.googleapis.com/token";

            //WebRequest class is needed to capture the JSON response
            WebRequest request = WebRequest.Create(exchangeRequestURL);

            request.Method = "POST";

            String postData =
                "refresh_token=" + refreshToken + "&" +
                "client_id=" + config["Authentication:Google:ClientId"].ToString() + "&" +
                "client_secret=" + config["Authentication:Google:ClientSecret"].ToString() + "&" +
                "grant_type=refresh_token";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try {

                WebResponse response = request.GetResponse();

                //read the response data into a string
                Stream responseDataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(responseDataStream);
                string jsonResponse = reader.ReadToEnd();

                response.Close();

                //collect the access token and refresh token
                JObject json = JObject.Parse(jsonResponse);
                string accessToken = (string)json.SelectToken("access_token");

                return accessToken;

            }
            catch (Exception) {

                throw;
            }

        }
        #endregion Upon access token expiration, request a new one from the stored refresh token















    }
}
