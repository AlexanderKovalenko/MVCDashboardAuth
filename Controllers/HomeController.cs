using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDashboardAuth.Controllers {
    public class HomeController : Controller {
        public ActionResult Dashboard() {
            return View();
        }

        public ActionResult Login() {
            return View();
        }
    }
}