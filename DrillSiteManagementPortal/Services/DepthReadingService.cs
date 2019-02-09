using System;
using DrillSiteManagementPortal.Models;

namespace DrillSiteManagementPortal.Services
{
    public class DepthReadingService
    {
        public DepthReadingModel DepthReadingModel { get; set; }


        public DepthReadingService(DepthReadingModel depthReadingModel)
        {
            DepthReadingModel = depthReadingModel;
        }
        
        public static DepthReadingModel CreateRandomDepthReading(double dipTrend, double dipMarginOfError, double azimuthTrend, double azimuthMarginOfError)
        {
            // calculate the upper and lower bounds of the dip & azimuth values
            var upperDip = Math.Min(90f, dipTrend + dipMarginOfError);
            var lowerDip = Math.Max(0, dipTrend - dipMarginOfError);
            var upperAzimuth = Math.Min(360f, azimuthTrend + azimuthMarginOfError);
            var lowerAzimuth = Math.Max(0, azimuthTrend - azimuthMarginOfError);

            var random = new Random();
            var dip = Convert.ToSingle(random.NextDouble() * (upperDip + lowerDip) - lowerDip);
            var azimuth = Convert.ToSingle(random.NextDouble() * (upperAzimuth + lowerAzimuth) - lowerAzimuth);
            return new DepthReadingModel(dip, azimuth);
        }

        public bool IsTrustworthy => DepthReadingModel.TrustWorthiness - 100.0f < 0.000001f;
    }
}