using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarManager.Controllers {

    public class ErrorHandlerController : Controller {

        public IActionResult Index() {

            return View();
        }

        public IActionResult RaceCondition() {

            return View();
        }
    }
}
