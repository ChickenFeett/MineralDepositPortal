using System.Web.Mvc;

namespace DrillSiteManagementPortal.Models
{
    public class DepthReadingModel
    {
        public long Id { get; set; }
        public float Dip { get; set; }
        public float Azimuth { get; set; }
        public float TrustWorthiness { get; set; }


        public DepthReadingModel(long id, float dip, float azimuth)
        {
            Id = id;
            Dip = dip;
            Azimuth = azimuth;
        }

        public bool IsTrustworthy => TrustWorthiness - 100.0f < 0.000001f;
    }
}