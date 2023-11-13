using DevExpress.DashboardWeb.Mvc;
using System.Web.Mvc;

namespace MVCDashboardAuth.Controllers {
    [Authorize]
    public class DefaultDashboardController : DashboardController {
        // ...
    }
}