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
     
        public bool IsTrustworthy => DepthReadingModel.TrustWorthiness - 100.0f < 0.000001f;
    }
}