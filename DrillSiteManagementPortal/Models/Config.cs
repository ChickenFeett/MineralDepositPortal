using System.Web.Mvc;

namespace DrillSiteManagementPortal.Models
{
    public class Config
    {
        // Dip configuration values
        public int NumberOfRecordsToQueryDip;
        public int DipMarginOfError;
        // Azimuth configuration values
        public int NumberOfRecordsToQueryAzimuth;
        public int AzimuthMarginOfError;


        public Config(int numberOfRecordsToQueryDip, int dipMarginOfError, int numberOfRecordsToQueryAzimuth, int azimuthMarginOfError)
        {
            NumberOfRecordsToQueryDip = numberOfRecordsToQueryDip;
            DipMarginOfError = dipMarginOfError;
            NumberOfRecordsToQueryAzimuth = numberOfRecordsToQueryAzimuth;
            AzimuthMarginOfError = azimuthMarginOfError;
        }
    }
}