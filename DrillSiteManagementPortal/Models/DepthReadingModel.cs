using System.Web.Helpers;
using System.Web.Mvc;

namespace DrillSiteManagementPortal.Models
{
    public class DepthReadingModel
    {
        public long Id { get; set; }
        public double Dip { get; set; }
        public double Azimuth { get; set; }
        public double TrustWorthiness { get; set; }


        public DepthReadingModel(double dip, double azimuth, double trustWorthiness)
        {
            Dip = dip;
            Azimuth = azimuth;
            TrustWorthiness = trustWorthiness;
        }

        public bool IsTrustworthy => TrustWorthiness - 100.0 < 0.000001;

        public string Serialize()
        {
            return Json.Encode(this);
        }
    }
}