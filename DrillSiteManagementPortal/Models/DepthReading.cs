using System.Web.Mvc;

namespace DrillSiteManagementPortal.Models
{
    public class DepthReading
    {
        public float HoleAzimuth;
        public float HoleDip;
        public float TrustWorthiness;

        public bool IsTrustworthy => TrustWorthiness - 100.0f < 0.000001f;
    }
}