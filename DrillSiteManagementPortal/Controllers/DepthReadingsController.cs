using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DrillSiteManagementPortal.Models;
using DrillSiteManagementPortal.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace DrillSiteManagementPortal.Controllers
{
    public class DepthReadingsController : ApiController
    {
        /// <summary>
        ///     Used to retrieve Depth Readings that
        ///     correspond with a particular Drill Site
        ///     Usage: GET api/<controller>/<site_id>
        /// </summary>
        /// <param name="id">Drill site ID</param>
        /// <returns>Depth Readings associated with the Drill Site</returns>
        public IEnumerable<string> GetDepthReadings(int id)
        {
            using (var db = new DsmContext())
            {
                var drillSite = db.DrillSites.Include(x => x.DepthReadings).FirstOrDefault(x => x.Id == id);

                return drillSite == null
                    ? new[] {"no result"}
                    : drillSite.DepthReadings.OrderBy(x => x.Id).Select(x => x.Serialize());
            }
        }

        // POST api/<controller>
        public void PostAzimuth(int drillSiteId, int readingId, string azimuth)
        {
            try
            {
                using (var db = new DsmContext())
                {
                    // retrieve requested "current" reading
                    var currentReading = db.DepthReadings.FirstOrDefault(x => x.Id == readingId);
                    var drillSite = db.DrillSites.Include(x => x.DepthReadings).FirstOrDefault(x => x.Id == drillSiteId);
                    if (currentReading == null || drillSite == null)
                    {
                        Console.WriteLine("Could not find the specified reading or drill site");
                        return;
                    }

                    currentReading.Azimuth = Convert.ToDouble(azimuth); // TODO - handle invalid input
                    var config = db.Config.FirstOrDefault() ?? new DrillConfigModel();
                    TrickleDownTrustWorthiness(drillSite, currentReading, config.NumberOfRecordsToQueryAzimuth);
                    db.SaveChanges();
                }
            }
            catch (SqliteException)
            {
                // do not want to output exception details to user (for security reasons)
                Console.WriteLine("Failed - a database related exception occurred");
            }
        }

        // POST api/<controller>
        public void PostDip(int drillSiteId, int readingId, string dip)
        {
            try
            {
                using (var db = new DsmContext())
                {
                    // retrieve requested "current" reading
                    var currentReading = db.DepthReadings.FirstOrDefault(x => x.Id == readingId);
                    var drillSite = db.DrillSites.Include(x => x.DepthReadings).FirstOrDefault(x => x.Id == drillSiteId);
                    if (currentReading == null || drillSite == null)
                    {
                        Console.WriteLine("Could not find the specified reading or drill site");
                        return;
                    }

                    currentReading.Dip = Convert.ToDouble(dip); // TODO - handle invalid input
                    var config = db.Config.FirstOrDefault() ?? new DrillConfigModel();
                    TrickleDownTrustWorthiness(drillSite, currentReading, config.NumberOfRecordsToQueryDip);
                    db.SaveChanges();
                }
            }
            catch (SqliteException)
            {
                // do not want to output exception details to user (for security reasons)
                Console.WriteLine("Failed - a database related exception occurred");
            }
        }

        private void TrickleDownTrustWorthiness(DrillSiteModel drillSite, DepthReadingModel currentReading, int nRecordsRequired)
        {
            var index = drillSite.DepthReadings.OrderBy(x => x.Id).IndexOf(currentReading);
            // starting index is the current index - the number of records necessary for the query 
            // e.g. if we need to query 3 records and current index is 35, then start index would be 32
            var indexOfFirstRequiredReading = Math.Max(0, index - nRecordsRequired);
            var indexToUpdate = index - indexOfFirstRequiredReading;
            var affectedReadings = drillSite.DepthReadings.OrderBy(x => x.Id).Skip(indexOfFirstRequiredReading).Take(nRecordsRequired * 2 + 1).ToList();
            DrillSiteService.UpdateReadingsTrustworthiness(affectedReadings, indexToUpdate);
        }


        // POST api/<controller>
        public void PostDepth(int drillSiteId, int readingId, string depth)
        {
            try
            {
                using (var db = new DsmContext())
                {
                    // retrieve requested "current" reading
                    var currentReading = db.DepthReadings.FirstOrDefault(x => x.Id == readingId);
                    if (currentReading == null)
                    {
                        Console.WriteLine("Could not find the specified reading");
                        return;
                    }

                    currentReading.Depth = Convert.ToDouble(depth); // TODO - handle invalid input
                    db.SaveChanges();
                }
            }
            catch (SqliteException)
            {
                // do not want to output exception details to user (for security reasons)
                Console.WriteLine("Failed - a database related exception occurred");
            }
        }
    }
}