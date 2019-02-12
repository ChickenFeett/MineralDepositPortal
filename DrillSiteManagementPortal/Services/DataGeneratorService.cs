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
                drillSites.Add(drillSite);
            }

            return drillSites;
        }

        public static List<DepthReadingModel> GenerateDepthReadings(
            double dipTrend, double azimuthTrend,
            int numberOfDrillSites = DefaultNumberOfDrillSites,
            int maxDepthReadingsPerSite = DefaultNumberOfDepthReadings)
        {
            var config = DrillConfigService.GetInstance().DrillConfigModel;
            var random = new Random();
            // random number of depth readings, between 1 and 100
            var nDepthReadings = (int)(random.NextDouble() * maxDepthReadingsPerSite) + 1;
            var orderedDepthReadings = new List<DepthReadingModel>();
            for (var j = 0; j < nDepthReadings; j++)
            {
                // create random depth reading, with dip and azimuth in between bounds
                var reading = CreateRandomDepthReading(j, random, dipTrend, config.DipMarginOfError, azimuthTrend, config.AzimuthMarginOfError);
                
                orderedDepthReadings.Add(reading); // add to ordered list
                // retrieve number of records (as configured) to create dip average
                var records = DrillSiteService.RetrieveXRecordsBefore(
                    orderedDepthReadings,
                    j + 1,
                    config.NumberOfRecordsToQueryDip);
                dipTrend = records.Sum(x => x.Dip) / records.Count();
                // retrieve number of records (as configured) to create azimuth average
                records = DrillSiteService.RetrieveXRecordsBefore(
                    orderedDepthReadings,
                    j + 1,
                    config.NumberOfRecordsToQueryAzimuth);
                azimuthTrend = records.Sum(x => x.Azimuth) / records.Count();
            }

            return orderedDepthReadings;
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

        public static DepthReadingModel CreateRandomDepthReading(int index, Random random, double dipTrend, double dipMarginOfError, double azimuthTrend, double azimuthMarginOfError)
        {
            // calculate the upper and lower bounds of the dip & azimuth values
            var highestPossibleDip = Math.Min(90f, dipTrend + dipMarginOfError);
            var highestPossibleAzimuth = Math.Min(360f, azimuthTrend + azimuthMarginOfError);
            var lowestPossibleDip = Math.Min(90, Math.Max(0, dipTrend - dipMarginOfError));
            var lowestPossibleAzimuth = Math.Min(360, Math.Max(0, azimuthTrend - azimuthMarginOfError));
            
            var dip = random.NextDouble() * (highestPossibleDip - lowestPossibleDip) + lowestPossibleDip;
            var azimuth = random.NextDouble() * (highestPossibleAzimuth - lowestPossibleAzimuth) + lowestPossibleAzimuth;
            return new DepthReadingModel(dip, azimuth, 100.0);
        }
    }
}