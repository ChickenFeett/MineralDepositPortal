using System.Web.Mvc;
using System.Collections.Generic;
using DrillSiteManagementPortal.Models;
using System;

namespace DrillSiteManagementPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // load data from DB
            var myList = new List<DrillSite>();
            myList.Add(new DrillSite(1, new Config(5, 3, 1, 5), 123.541, 21.3123, 45, 21, DateTime.Now));
            // display data
            return View(myList);
        }        
    }
}
