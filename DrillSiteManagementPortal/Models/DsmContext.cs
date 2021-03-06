﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DrillSiteManagementPortal.Models
{
    public class DsmContext: DbContext
    {
        public DbSet<DrillSiteModel> DrillSites { get; set; }
        public DbSet<DepthReadingModel> DepthReadings { get; set; }
        public DbSet<DrillConfigModel> Config { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlite("Data Source=dsm.db");
        }

        public void TryCreateDatabase()
        {
            Database.EnsureCreated();
        }
    }
}