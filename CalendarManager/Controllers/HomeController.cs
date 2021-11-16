using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CalendarManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CalendarManager.Controllers {

    public class HomeController : Controller {

        private readonly ILogger<HomeController> logger;
        private ApplicationDbContext context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) {

            this.logger = logger;
            this.context = context;
        }

        public IActionResult Index() {

            /*using (context) {
                context.URLs.Add(new URLs {
                    ShortURL = "Jimmy",
                    Email = "jjjj",
                    EnforcePeriods = true,
                    PeriodName = "ssd",
                    Period = 6
                });
                context.BookedEvents.Add(new BookedEvents {
                    ShortURL = "Jim",
                    StartHour = 2,
                    StartMinute = 1,
                    EndHour = 2,
                    EndMinute = 5,
                    Day = "Tuesday",
                    Date = 2,
                    Month = 2,
                    Year = 1
                });
                context.RefreshTokens.Add(new RefreshTokens {
                    Email = "matejrefka@gmail",
                    Token = "tokenString"
                });
                context.SaveChanges();
            }*/

            return View();
        }


    }
}
