using System;
using System.Collections.Generic;
using System.Linq;
using DrillSiteManagementPortal.Models;

namespace DrillSiteManagementPortal.Services
{
    public class DrillSiteService
    {
        public static DrillConfigModel Config = DrillConfigService.GetInstance().DrillConfigModel;
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
        public static void UpdateReadingsTrustworthiness(List<DepthReadingModel> readings, int indexToUpdate)
        {
            if (indexToUpdate >= readings.Count)
                return; // end of list

            var azimuthRecordsToQuery =
                RetrieveXRecordsBefore(readings, indexToUpdate, Config.NumberOfRecordsToQueryAzimuth).ToList();
            var dipRecordsToQuery = Config.NumberOfRecordsToQueryAzimuth == Config.NumberOfRecordsToQueryDip
                ? azimuthRecordsToQuery // no need to calculate new list, if they're both going to be the same
                : RetrieveXRecordsBefore(readings, indexToUpdate, Config.NumberOfRecordsToQueryDip).ToList();
           
            CalculateTrustWorthiness(readings[indexToUpdate], dipRecordsToQuery, azimuthRecordsToQuery);
            UpdateReadingsTrustworthiness(readings, indexToUpdate + 1);
        }

        public static void CalculateTrustWorthiness(DepthReadingModel reading, List<DepthReadingModel> dipRecordsToQuery, List<DepthReadingModel> azimuthRecordsToQuery)
        {
            // take averages
            var azimuthAverage = azimuthRecordsToQuery.Any()
                ? azimuthRecordsToQuery.Sum(x => x.Azimuth) / azimuthRecordsToQuery.Count()
                : 0;
            var dipAverage = dipRecordsToQuery.Any()
                ? dipRecordsToQuery.Sum(x => x.Dip) / dipRecordsToQuery.Count()
                : 0;

            // calculate trustworthiness
            reading.TrustWorthiness = Math.Abs(reading.Azimuth - azimuthAverage) < Config.AzimuthMarginOfError &&
                                      Math.Abs(reading.Dip - dipAverage) < Config.DipMarginOfError
                ? 100
                : 0;
        }

        public static List<DepthReadingModel> RetrieveXRecordsBefore(List<DepthReadingModel> readings, int index, int numberOfRecordsToRetrieve)
        {
            var startingIndex = Math.Max(0, index - numberOfRecordsToRetrieve); // move x records back
            var nRecordsAvailableAfterStartingIndex = index - startingIndex; // calculate how many records available forwards
            return readings.Skip(startingIndex).Take(nRecordsAvailableAfterStartingIndex).ToList();
        }
    }
}