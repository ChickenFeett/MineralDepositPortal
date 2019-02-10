using System.Collections.Generic;
using System;
using System.Linq;

namespace DrillSiteManagementPortal.Models
{
    public class DrillSiteModel
    {
        public long Id { get; set; }
        // Date of analysis
        public DateTime StartDate { get; set; }
        // location of the collar
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        // Dip and Azimuth of the collar
        public double CollarDip { get; set; }
        public double CollarAzimuth { get; set; }
        // collection of readings between 0-100
        public ICollection<DepthReadingModel> DepthReadings { get; set; }
        

        public DrillSiteModel(double latitude, double longitude, double collarDip, double collarAzimuth, DateTime startDate)
        {
            Latitude = latitude;
            Longitude = longitude;
            CollarDip = collarDip;
            CollarAzimuth = collarAzimuth;
            StartDate = startDate;
            DepthReadings = new List<DepthReadingModel>();
        }

        public void AddReading(DepthReadingModel reading)
        {
            DepthReadings.Add(reading);
        }
    }
}