using System.Web.Mvc;

namespace DrillSiteManagementPortal.Models
{
    public class DrillConfigModel
    {        
        public int Id { get; set; }
        // Dip configuration values
        public int NumberOfRecordsToQueryDip { get; set; }
        public int DipMarginOfError { get; set; }
        // Azimuth configuration values
        public int NumberOfRecordsToQueryAzimuth { get; set; }
        public int AzimuthMarginOfError { get; set; }


        public DrillConfigModel(int numberOfRecordsToQueryDip, int dipMarginOfError, int numberOfRecordsToQueryAzimuth, int azimuthMarginOfError)
        {
            NumberOfRecordsToQueryDip = numberOfRecordsToQueryDip;
            DipMarginOfError = dipMarginOfError;
            NumberOfRecordsToQueryAzimuth = numberOfRecordsToQueryAzimuth;
            AzimuthMarginOfError = azimuthMarginOfError;
        }
    }
}