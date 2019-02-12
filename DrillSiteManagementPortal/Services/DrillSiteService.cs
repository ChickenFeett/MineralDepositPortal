using System;
using System.Collections.Generic;
using System.Linq;
using DrillSiteManagementPortal.Models;

namespace DrillSiteManagementPortal.Services
{
    public class DrillSiteService
    {
        public static DrillConfigModel Config;
        public DrillSiteModel DrillSiteModel { get; set; }

        public DrillSiteService(DrillSiteModel drillSiteModel, DrillConfigModel config)
        {
            Config = config;
            DrillSiteModel = drillSiteModel;         
        }
        
        public void AddReading(DepthReadingModel reading)
        {
            var index = DrillSiteModel.DepthReadings.Count();
            DrillSiteModel.AddReading(reading);
            // todo - change to List for guaranteed ordering
            var depthReadings = DrillSiteModel.DepthReadings.ToList();
            UpdateReadingsTrustworthiness(depthReadings, index); 
        }

        /// <summary>
        ///     Recursive function that will cycle through the readings, starting at
        ///     indexToUpdate until it reaches the end of the readings list, updating
        ///     the trustworthiness as it goes along.
        /// </summary>
        /// <param name="readings"></param>
        /// <param name="indexToUpdate"></param>
        /// <param name="config">If left null, will use config from database - optional for unit testing purposes</param>
        public static void UpdateReadingsTrustworthiness(List<DepthReadingModel> readings, int indexToUpdate, DrillConfigModel config = null)
        {
            if (indexToUpdate >= readings.Count)
                return; // end of list

            if (config == null)
                config = Config ?? (Config = DrillConfigService.GetInstance().DrillConfigModel);

            var azimuthRecordsToQuery =
                RetrieveXRecordsBefore(readings, indexToUpdate, config.NumberOfRecordsToQueryAzimuth).ToList();
            var dipRecordsToQuery = config.NumberOfRecordsToQueryAzimuth == config.NumberOfRecordsToQueryDip
                ? azimuthRecordsToQuery // no need to calculate new list, if they're both going to be the same
                : RetrieveXRecordsBefore(readings, indexToUpdate, config.NumberOfRecordsToQueryDip).ToList();
           
            CalculateTrustWorthiness(config, readings[indexToUpdate], dipRecordsToQuery, azimuthRecordsToQuery);
            UpdateReadingsTrustworthiness(readings, indexToUpdate + 1, config);
        }

        private static void CalculateTrustWorthiness(DrillConfigModel config, DepthReadingModel reading, List<DepthReadingModel> dipRecordsToQuery, List<DepthReadingModel> azimuthRecordsToQuery)
        {
            // take averages
            var azimuthAverage = azimuthRecordsToQuery.Any()
                ? azimuthRecordsToQuery.Sum(x => x.Azimuth) / azimuthRecordsToQuery.Count()
                : (double?) null;
            var dipAverage = dipRecordsToQuery.Any()
                ? dipRecordsToQuery.Sum(x => x.Dip) / dipRecordsToQuery.Count()
                : (double?) null;

            if (!dipAverage.HasValue || !azimuthAverage.HasValue)
            {
                // assume trustworthy, likely no records exist before this node
                reading.TrustWorthiness = 100f;
                return;
            }

            // calculate trustworthiness
            reading.TrustWorthiness = Math.Abs(reading.Azimuth - azimuthAverage.Value) < config.AzimuthMarginOfError &&
                                      Math.Abs(reading.Dip - dipAverage.Value) < config.DipMarginOfError
                ? 100
                : 0;
        }

        public static List<DepthReadingModel> RetrieveXRecordsBefore(List<DepthReadingModel> readings, int index, int numberOfRecordsToRetrieve)
        {
            var startingIndex = Math.Max(0, index - numberOfRecordsToRetrieve); // move x records back
            var nRecordsAvailableAfterStartingIndex = index - startingIndex; // calculate how many records available forwards
            return readings.OrderBy(x => x.Id).Skip(startingIndex).Take(nRecordsAvailableAfterStartingIndex).ToList();
        }
    }
}