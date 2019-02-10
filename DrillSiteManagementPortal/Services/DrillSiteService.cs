using System;
using System.Collections.Generic;
using System.Linq;
using DrillSiteManagementPortal.Models;

namespace DrillSiteManagementPortal.Services
{
    public class DrillSiteService
    {
        public static DrillConfigModel Config { get; set; }
        public DrillSiteModel DrillSiteModel { get; set; }

        public DrillSiteService(DrillSiteModel drillSiteModel, DrillConfigModel config)
        {
            Config = config;
            DrillSiteModel = drillSiteModel;         
        }
        
        public void AddReading(DepthReadingModel reading)
        {
            CalculateTrustWorthiness(reading, DrillSiteModel.DepthReadings.ToList(), DrillSiteModel.DepthReadings.Count());
            DrillSiteModel.AddReading(reading);
        }

        private void CalculateTrustWorthiness(DepthReadingModel reading, List<DepthReadingModel> depthReadings, int index)
        {
            // retrieve last x records (if x do not exist, take as many as possible up to x)
            var azimuthRecordsToQuery = RetrieveLastXRecords(depthReadings, index, Config.NumberOfRecordsToQueryAzimuth).ToList();
            var dipRecordsToQuery = Config.NumberOfRecordsToQueryAzimuth == Config.NumberOfRecordsToQueryDip
                ? azimuthRecordsToQuery // no need to calculate new list, if they're both going to be the same
                : RetrieveLastXRecords(depthReadings, index, Config.NumberOfRecordsToQueryDip).ToList();

            // take averages
            var azimuthAverage = azimuthRecordsToQuery.Sum(x => x.Azimuth) / azimuthRecordsToQuery.Count();
            var dipAverage = dipRecordsToQuery.Sum(x => x.Dip) / dipRecordsToQuery.Count();

            // calculate trustworthiness
            reading.TrustWorthiness = Math.Abs(reading.Azimuth - azimuthAverage) < Config.AzimuthMarginOfError &&
                                      Math.Abs(reading.Dip - dipAverage) < Config.DipMarginOfError
                ? 100
                : 0;
        }

        public static List<DepthReadingModel> RetrieveLastXRecords(List<DepthReadingModel> readings, int startingIndex, int numberOfRecordsToRetrieve)
        {
            var nRecordsAvailableAfterIndex = Math.Min(readings.Count() - startingIndex, numberOfRecordsToRetrieve);
            return readings.Skip(startingIndex).Take(nRecordsAvailableAfterIndex).ToList();
        }
    }
}