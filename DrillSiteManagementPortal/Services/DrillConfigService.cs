using System;
using DrillSiteManagementPortal.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DrillSiteManagementPortal.Services
{
    /// <summary>
    /// Service to allow the retrieval and creation of the config model
    /// </summary>
    public class DrillConfigService
    {
        // TODO - consider putting the default values in a config.xml file
        // Default dip configuration values
        private const int DefaultNumberOfRecordsToQueryDip = 5;
        private const int DefaultDipMarginOfError = 3;
        // Default azimuth configuration values
        private const int DefaultNumberOfRecordsToQueryAzimuth = 1;
        private const int DefaultAzimuthMarginOfError = 5;

        private static DrillConfigService _instance;
        public readonly DrillConfigModel DrillConfigModel;

        /// <summary>
        /// Retrieve singleton instance of config service - on first call
        /// GetInstance will attempt to retrieve config from database, if
        /// config does not exist in database, an instance is created and
        /// inserted into the database.
        /// </summary>
        /// <returns>Database config service instance</returns>
        public static DrillConfigService GetInstance()
        {
            if (_instance != null)
                return _instance; // already instantiated

            // create default instance
            _instance = new DrillConfigService();

            // attempt to read config from database
            try
            {
                using (var db = new DsmContext())
                {
                    var config = db.Config.FirstOrDefaultAsync().Result;
                    if (config == null) // no config exists in database
                    {
                        // insert default instance
                        db.Config.Add(_instance.DrillConfigModel);
                        db.SaveChanges();
                    }
                    else
                    {
                        // config exists, create service instance using config data
                        _instance = new DrillConfigService(config);
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Failed to retrieve or write config to database", ex);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Failed to retrieve or write config to database", ex);
            }

            return _instance;
        }

        /// <summary>
        /// Default constructor, that will create the model with the hard-coded default values 
        /// </summary>
        public DrillConfigService()
        {
            DrillConfigModel = new DrillConfigModel(
                DefaultNumberOfRecordsToQueryDip,
                DefaultDipMarginOfError,
                DefaultNumberOfRecordsToQueryAzimuth, 
                DefaultAzimuthMarginOfError);
        }

        public DrillConfigService(int numberOfRecordsToQueryDip, int dipMarginOfError, int numberOfRecordsToQueryAzimuth, int azimuthMarginOfError)
        {
            DrillConfigModel = new DrillConfigModel(
                numberOfRecordsToQueryDip,
                dipMarginOfError, 
                numberOfRecordsToQueryAzimuth, 
                azimuthMarginOfError);
        }

        public DrillConfigService(DrillConfigModel config)
        {
            DrillConfigModel = config;
        }
    }
}