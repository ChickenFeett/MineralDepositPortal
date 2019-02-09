using System.Web.Mvc;
using System.Collections.Generic;
using DrillSiteManagementPortal.Models;
using System;
using System.Linq;
using DrillSiteManagementPortal.Services;
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
                    drillSites = db.DrillSites.ToList();
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex);
            }

            return View(drillSites);
        }

        public ActionResult GenerateData()
        {
            // create random data
            var drillSites = new List<DrillSiteService>();
            var config = DrillConfigService.GetInstance().DrillConfigModel;
            for (var i = 0; i < 3; i++)
            {
                var drillSite = DrillSiteService.CreateRandomDrillSite();
                var rand = new Random();
                // random number of depth readings, between 1 and 100
                var nDepthReadings = Convert.ToInt16(rand.NextDouble() * 100) + 1;
                // retrieve dip and azimuth from root
                var dipTrend = drillSite.DrillSiteModel.CollarDip;
                var azimuthTrend = drillSite.DrillSiteModel.CollarAzimuth;
                for (var j = 0; j < nDepthReadings; j++)
                {
                    // create random depth reading, with dip and azimuth in between bounds
                    var reading = DepthReadingService.CreateRandomDepthReading(dipTrend, config.DipMarginOfError, azimuthTrend, config.AzimuthMarginOfError);
                    drillSite.AddReading(reading);
                    // retrieve number of records (as configured) to create dip average
                    var records = DrillSiteService.RetrieveLastXRecords(drillSite.DrillSiteModel.DepthReadings.ToList(), j, config.NumberOfRecordsToQueryDip);
                    dipTrend = records.Sum(x => x.Dip) / records.Count();
                    // retrieve number of records (as configured) to create azimuth average
                    records = DrillSiteService.RetrieveLastXRecords(drillSite.DrillSiteModel.DepthReadings.ToList(), j, config.NumberOfRecordsToQueryAzimuth);
                    azimuthTrend = records.Sum(x => x.Azimuth) / records.Count();
                }
                drillSites.Add(drillSite);
            }
            // insert data into DB
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

            return Index();
        }

    }
}
