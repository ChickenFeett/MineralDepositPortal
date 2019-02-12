using System.Collections.Generic;
using DrillSiteManagementPortal.Models;
using DrillSiteManagementPortal.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class DrillSiteServiceTest
    {
        [TestMethod]
        public void TestUpdateReadingsTrustworthiness()
        {
            var config = new DrillConfigModel(5,3,1,5); 
            var readings = new List<DepthReadingModel>()
            {
                new DepthReadingModel(5, 150, 100), // avg dip = -     last azimuth = -      - trustworthy
                new DepthReadingModel(4, 145.01, 100),//avg dip = 5    last azimuth = 150   - trustworthy
                new DepthReadingModel(3, 142, 100), // avg dip = 4.5   last azimuth = ~145   - trustworthy
                new DepthReadingModel(5, 139, 100), // avg dip = 4     last azimuth = 142    - trustworthy
                new DepthReadingModel(5, 135, 100), // avg dip = 4.25  last azimuth = 139    - trustworthy
                new DepthReadingModel(1, 137, 100), // avg dip = 4.4   last azimuth = 135    - not trustworthy
                new DepthReadingModel(1, 134, 100), // avg dip = 3.6   last azimuth = 137    - trustworthy
                new DepthReadingModel(1, 120, 100), // avg dip = 3     last azimuth = 134    - not trustworthy
                new DepthReadingModel(2, 118, 100), // avg dip = 2.6   last azimuth = 120    - trustworthy
                new DepthReadingModel(3, 150, 100), // avg dip = 2     last azimuth = 120    - not trustworthy
            };
            var expectedReadings = new List<DepthReadingModel>()
            {
                new DepthReadingModel(5, 150, 100), // avg dip = -     last azimuth = -      - trustworthy
                new DepthReadingModel(4, 145, 100), // avg dip = 5     last azimuth = 150    - trustworthy
                new DepthReadingModel(3, 140, 100), // avg dip = 4.5   last azimuth = 145    - trustworthy
                new DepthReadingModel(5, 139, 100), // avg dip = 4     last azimuth = 140    - trustworthy
                new DepthReadingModel(5, 135, 100), // avg dip = 4.25  last azimuth = 139    - trustworthy
                new DepthReadingModel(1, 137, 0.0), // avg dip = 4.4   last azimuth = 135    - not trustworthy
                new DepthReadingModel(1, 134, 100), // avg dip = 3.6   last azimuth = 137    - trustworthy
                new DepthReadingModel(1, 120, 0.0), // avg dip = 3     last azimuth = 134    - not trustworthy
                new DepthReadingModel(2, 118, 100), // avg dip = 2.6   last azimuth = 120    - trustworthy
                new DepthReadingModel(3, 150, 0.0), // avg dip = 2     last azimuth = 120    - not trustworthy
            };
            // test not equal where trustworthness is expected to be false
            for (var i = 0; i < readings.Count; i++)
            {
                if (!expectedReadings[i].IsTrustworthy)
                    Assert.AreNotEqual(expectedReadings[i].IsTrustworthy, readings[i].IsTrustworthy);
            }

            DrillSiteService.UpdateReadingsTrustworthiness(readings, 0, config);

            for (var i = 0; i < readings.Count; i++)
            {
                Assert.AreEqual(expectedReadings[i].IsTrustworthy, readings[i].IsTrustworthy);
            }
        }
    }
}
