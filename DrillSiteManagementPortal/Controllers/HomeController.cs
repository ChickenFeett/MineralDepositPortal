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
            var myList = new List<DrillSiteModel>();
            var drillSite = new DrillSiteModel(1, 123.541, 21.3123, 45, 21, DateTime.Now);
            drillSite.AddReading(new DepthReadingModel(1, 23, 51));
            drillSite.AddReading(new DepthReadingModel(2, 24, 51.123f));
            drillSite.AddReading(new DepthReadingModel(3, 25, 51.45f));
            myList.Add(drillSite);
            using (var db = new DsmContext())
            {
                db.Add(drillSite);
            }
                // load data from DB
            // display data
            return View(myList);
        }        
    }
}
