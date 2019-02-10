using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrillSiteManagementPortal.Models;

namespace DrillSiteManagementPortal.Services
{
    public static class DataGeneratorService
    {
        // TODO - consider putting the default values in a config.xml file
        // Default dip configuration values
        private const int DefaultNumberOfDrillSites = 3;
        private const int DefaultNumberOfDepthReadings = 100;

        public static List<DrillSiteService> GenerateDrillSitesWithDepthReadings(
            int numberOfDrillSites = DefaultNumberOfDrillSites,
            int maxDepthReadingsPerSite = DefaultNumberOfDepthReadings)
        {
            var random = new Random();
            var drillSites = new List<DrillSiteService>();
            var config = DrillConfigService.GetInstance().DrillConfigModel;
            for (var i = 0; i < numberOfDrillSites; i++)
            {
                var drillSite = CreateRandomDrillSite(config, random);
                // random number of depth readings, between 1 and 100
                var nDepthReadings = Convert.ToInt16(random.NextDouble() * maxDepthReadingsPerSite) + 1;
                // retrieve dip and azimuth from root
                var dipTrend = drillSite.DrillSiteModel.CollarDip;
                var azimuthTrend = drillSite.DrillSiteModel.CollarAzimuth;
                for (var j = 0; j < nDepthReadings; j++)
                {
                    // create random depth reading, with dip and azimuth in between bounds
                    var reading = CreateRandomDepthReading(random, dipTrend, config.DipMarginOfError, azimuthTrend, config.AzimuthMarginOfError);
                    drillSite.AddReading(reading);
                    // retrieve number of records (as configured) to create dip average
                    var records = DrillSiteService.RetrieveLastXRecords(
                        drillSite.DrillSiteModel.DepthReadings.ToList(),
                        j,
                        config.NumberOfRecordsToQueryDip);
                    dipTrend = records.Sum(x => x.Dip) / records.Count();
                    // retrieve number of records (as configured) to create azimuth average
                    records = DrillSiteService.RetrieveLastXRecords(
                        drillSite.DrillSiteModel.DepthReadings.ToList(), 
                        j, 
                        config.NumberOfRecordsToQueryAzimuth);
                    azimuthTrend = records.Sum(x => x.Azimuth) / records.Count();
                }
                drillSites.Add(drillSite);
            }

            return drillSites;
        }

        public static DrillSiteService CreateRandomDrillSite(DrillConfigModel config, Random random)
        {
            var lng = random.NextDouble() * 360 - 180;
            var lat = random.NextDouble() * 180 - 90;
            var azimuth = random.NextDouble() * 360;
            var dip = random.NextDouble() * 90;
            var date = DateTime.Now - TimeSpan.FromDays(random.NextDouble() * 7); // up to a week in the past
            var model = new DrillSiteModel(lat, lng, dip, azimuth, date);
            return new DrillSiteService(model, config);
        }

        public static DepthReadingModel CreateRandomDepthReading(Random random, double dipTrend, double dipMarginOfError, double azimuthTrend, double azimuthMarginOfError)
        {
            // calculate the upper and lower bounds of the dip & azimuth values
            var lowestPossibleDip = Math.Min(90, Math.Max(0, dipTrend - dipMarginOfError));
            var lowestPossibleAzimuth = Math.Min(360, Math.Max(0, azimuthTrend - azimuthMarginOfError));
            
            var dip = random.NextDouble() * dipMarginOfError * 2 + lowestPossibleDip;
            var azimuth = random.NextDouble() * azimuthMarginOfError * 2 + lowestPossibleAzimuth;
            return new DepthReadingModel(dip, azimuth, 100.0);
        }
    }
}