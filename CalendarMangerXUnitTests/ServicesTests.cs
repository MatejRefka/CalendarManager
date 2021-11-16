using CalendarManager.Models;
using CalendarManager.Services;
using CalendarManagerLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CalendarMangerXUnitTests {

    public class ServicesTests {

        [Fact]
        public void BookedEventListToEventListTypeCheck() {

            var input = new List<BookedEvent>();
            var bookedEvent = new BookedEvent();
            bookedEvent.StartHour = 4;
            bookedEvent.StartMinute = 0;
            bookedEvent.EndHour = 5;
            bookedEvent.EndMinute = 0;
            bookedEvent.Day = "Tuesday";
            bookedEvent.Date = 22;
            bookedEvent.Month = 2;
            bookedEvent.Year = 2021;
            input.Add(bookedEvent);

            var rounder = new Rounder();
            var deserializer = new EventDeserializer(rounder);

            var actual = deserializer.BookedEventToEvent(input);

            Assert.IsType<List<Event>>(actual);

        }

        [Fact]
        public void ChopIntoThreeBookedEvents() {

            var input = new List<BookedEvent>();
            var bookedEvent = new BookedEvent();
            bookedEvent.StartHour = 4;
            bookedEvent.StartMinute = 0;
            bookedEvent.EndHour = 5;
            bookedEvent.EndMinute = 0;
            bookedEvent.Day = "Tuesday";
            bookedEvent.Date = 22;
            bookedEvent.Month = 2;
            bookedEvent.Year = 2021;
            input.Add(bookedEvent);

            var rounder = new Rounder();
            var formatter = new EventFormatter();

            var expected = 3;

            var actual = formatter.ChopEvents(input,20).Count();

            Assert.Equal(actual,expected);

        }

        [Fact]
        public void ChopIntoFiveBookedEvents() {

            var input = new List<BookedEvent>();
            var bookedEvent = new BookedEvent();
            bookedEvent.StartHour = 8;
            bookedEvent.StartMinute = 0;
            bookedEvent.EndHour = 10;
            bookedEvent.EndMinute = 30;
            bookedEvent.Day = "Tuesday";
            bookedEvent.Date = 22;
            bookedEvent.Month = 2;
            bookedEvent.Year = 2021;
            input.Add(bookedEvent);

            var formatter = new EventFormatter();

            var expected = 5;

            var actual = formatter.ChopEvents(input, 30).Count();

            Assert.Equal(actual, expected);

        }

    }
}
