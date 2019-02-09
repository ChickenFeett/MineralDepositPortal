using System.Web.Mvc;

namespace DrillSiteManagementPortal.Models
{
    public class DrillConfigService
    {
        // Dip configuration values
        public int NumberOfRecordsToQueryDip { get; set; }
        public int DipMarginOfError { get; set; }
        // Azimuth configuration values
        public int NumberOfRecordsToQueryAzimuth { get; set; }
        public int AzimuthMarginOfError { get; set; }


        public DrillConfigService(int numberOfRecordsToQueryDip, int dipMarginOfError, int numberOfRecordsToQueryAzimuth, int azimuthMarginOfError)
        {
            NumberOfRecordsToQueryDip = numberOfRecordsToQueryDip;
            DipMarginOfError = dipMarginOfError;
            NumberOfRecordsToQueryAzimuth = numberOfRecordsToQueryAzimuth;
            AzimuthMarginOfError = azimuthMarginOfError;
        }
    }
}