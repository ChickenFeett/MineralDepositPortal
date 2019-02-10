using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrillSiteManagementPortal.Models;
using DrillSiteManagementPortal.Services;
using Microsoft.Data.Sqlite;

namespace DrillSiteManagementPortal.Controllers
{
    public class DataGeneratorController : Controller
    {

        public ActionResult GenerateData()
        {
            // create random data
            var drillSites = DataGeneratorService.GenerateDrillSitesWithDepthReadings();
            // try insert data into DB
            try
            { 
                using (var db = new DsmContext())
                {
                    // remove all data
                    var allDrillSites = from sites in db.DrillSites select sites;
                    db.DrillSites.RemoveRange(allDrillSites);
                    // add newly generated data
                    foreach (var drillSite in drillSites)
                        db.Add(drillSite);
                    db.SaveChanges();
                }
            }
            catch (SqliteException ex)
            {
                // TODO - consider redirecting to a "failed to create/insert data" page
                Console.WriteLine("Failed to insert generated drill site & depth readings data into database", ex); 
            }

            return RedirectToAction("Index", "Home");
        }
    }
}