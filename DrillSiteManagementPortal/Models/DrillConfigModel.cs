namespace DrillSiteManagementPortal.Models
{
    public class DrillConfigModel
    {
        // TODO - consider putting the default values in a config.xml file
        // Default dip configuration values
        private const int DefaultNumberOfRecordsToQueryDip = 5;
        private const int DefaultDipMarginOfError = 3;
        // Default azimuth configuration values
        private const int DefaultNumberOfRecordsToQueryAzimuth = 1;
        private const int DefaultAzimuthMarginOfError = 5;

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


        public DrillConfigModel()
        {
            NumberOfRecordsToQueryDip = DefaultNumberOfRecordsToQueryDip;
            DipMarginOfError = DefaultDipMarginOfError;
            NumberOfRecordsToQueryAzimuth = DefaultNumberOfRecordsToQueryAzimuth;
            AzimuthMarginOfError = DefaultAzimuthMarginOfError;
        }
   
    }
}