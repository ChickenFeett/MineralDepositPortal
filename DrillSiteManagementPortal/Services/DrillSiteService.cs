using System.Collections.Generic;
using System;
using System.Linq;

namespace DrillSiteManagementPortal.Models
{
    public class DrillSiteService
    {
        public DrillConfigModel Config { get; set; }
        public DrillSiteModel DrillSiteModel { get; set; }

        public DrillSiteService(DrillSiteModel drillSiteModel, DrillConfigModel config)
        {
            Config = config;
            DrillSiteModel = drillSiteModel;         
        }

        public void AddReading(DepthReadingModel reading)
        {
            CalculateTrustWorthiness(reading, DrillSiteModel.DepthReadings, DrillSiteModel.DepthReadings.Count());
            DrillSiteModel.AddReading(reading);
        }

        private void CalculateTrustWorthiness(DepthReadingModel reading, IEnumerable<DepthReadingModel> depthReadings, int index)
        {
            // retrieve last x records (if x do not exist, take as many as possible up to x)
            var azimuthRecordsToQuery = RetrieveLastXRecords(depthReadings, index, Config.NumberOfRecordsToQueryAzimuth);
            var dipRecordsToQuery = Config.NumberOfRecordsToQueryAzimuth == Config.NumberOfRecordsToQueryDip
                ? azimuthRecordsToQuery // no need to calculate new list, if they're both going to be the same
                : RetrieveLastXRecords(depthReadings, index, Config.NumberOfRecordsToQueryDip);

            // take averages
            var azimuthAverage = azimuthRecordsToQuery.Sum(x => x.Azimuth) / azimuthRecordsToQuery.Count();
            var dipAverage = dipRecordsToQuery.Sum(x => x.Dip) / dipRecordsToQuery.Count();

            // calculate trustworthiness
            reading.TrustWorthiness = Math.Abs(reading.Azimuth - azimuthAverage) < Config.AzimuthMarginOfError &&
                                      Math.Abs(reading.Dip - dipAverage) < Config.DipMarginOfError
                ? 100f
                : 0f;
        }

        private static void RetrieveStatisticsOfPreviousXHoles(IEnumerable<DepthReadingModel> readings, out float azimuthAverage, out float dipOfPrevious)
        {
            azimuthAverage = readings.Sum(x => x.Azimuth) / readings.Count();
            dipOfPrevious = readings.Sum(x => x.Azimuth) / readings.Count();
        }

        private static IEnumerable<DepthReadingModel> RetrieveLastXRecords(IEnumerable<DepthReadingModel> readings, int startingIndex, int numberOfRecordsToRetrieve)
        {
            var nRecordsAvailableAfterIndex = Math.Min(startingIndex - readings.Count(), numberOfRecordsToRetrieve);
            return readings.Skip(startingIndex).Take(nRecordsAvailableAfterIndex);
        }
    }
}