using System;
using DrillSiteManagementPortal.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DrillSiteManagementPortal.Services
{
    public class DrillConfigService
    {
        private static DrillConfigService _instance;
        public DrillConfigModel DrillConfigModel;

        public static DrillConfigService GetInstance()
        {
            if (_instance == null)
            {
                using (var db = new DsmContext())
                {
                    DrillConfigModel config = null;
                    try { config = db.Config.FirstOrDefaultAsync().Result; }
                    catch (AggregateException ex) { Console.WriteLine(ex); }
                    if (config == null)
                    {
                        _instance = new DrillConfigService();
                        db.Config.Add(_instance.DrillConfigModel);
                        db.SaveChanges();
                    }
                    else
                    {
                        _instance= new DrillConfigService(config);
                    }

                }
            }
            return _instance;
        }

        public DrillConfigService()
        {
            DrillConfigModel = new DrillConfigModel(5, 3, 1, 5);
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