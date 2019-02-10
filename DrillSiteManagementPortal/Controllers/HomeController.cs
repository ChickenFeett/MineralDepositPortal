using System.Web.Mvc;
using System.Collections.Generic;
using DrillSiteManagementPortal.Models;
using System;
using System.Linq;
using DrillSiteManagementPortal.Migrations;
using Microsoft.Data.Sqlite;

namespace DrillSiteManagementPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            var drillSites = new List<DrillSiteModel>();
            try
            {
                using (var db = new DsmContext())
                {
                    db.TryCreateDatabase();
                    drillSites = db.DrillSites.ToList();
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex);
            }

            return View(drillSites);
        }
    }
}
