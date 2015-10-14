using Orkidea.Bretano.WebMiddle.FrontEnd.WebMiddleBackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Orkidea.Bretano.WebMiddle.FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            #region User identification
            IIdentity context = HttpContext.User.Identity;
            bool admin = false;
            bool customerCreator = false;
            bool saleOrderCreator = false;
            int companyId = 0;
            string userName = "";            

            if (context.IsAuthenticated)
            {

                System.Web.Security.FormsIdentity ci = (System.Web.Security.FormsIdentity)HttpContext.User.Identity;
                string[] userRole = ci.Ticket.UserData.Split('|');

                admin = int.Parse(userRole[1]) == 1 ? true : false;
                customerCreator = int.Parse(userRole[2]) == 1 ? true : false;
                saleOrderCreator = int.Parse(userRole[3]) == 1 ? true : false;
                companyId = int.Parse(userRole[4]);
                userName = ci.Name;                
            }
            #endregion

            if (admin)
                return RedirectToAction("index", "company");

            if (saleOrderCreator)
                return RedirectToAction("create", "salesorder");

            if (customerCreator)
                return RedirectToAction("create", "customer");            

            return RedirectToAction("create", "salesorder");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}