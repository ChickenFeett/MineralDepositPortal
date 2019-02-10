using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DrillSiteManagementPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace DrillSiteManagementPortal.Controllers
{
    public class DepthReadingsController : ApiController
    {
        /// <summary>
        ///     Used to retrieve Depth Readings that
        ///     correspond with a particular Drill Site
        ///     Usage: GET api/<controller>/<site_id>
        /// </summary>
        /// <param name="id">Drill site ID</param>
        /// <returns>Depth Readings associated with the Drill Site</returns>
        public IEnumerable<string> GetDepthReadings(int id)
        {
            using (var db = new DsmContext())
            {
                var drillSite = db.DrillSites.Include(x => x.DepthReadings).FirstOrDefault(x => x.Id == id);

                return drillSite == null
                    ? new[] {"no result"}
                    : drillSite.DepthReadings.Select(x => x.Serialize());
            }
        }

        /// <summary>
        ///     Used to update a single value of a Depth Reading
        ///     Usage: // PUT api/<controller>/<reading_id>
        /// </summary>
        /// <param name="id">Reading ID</param>
        /// <param name="value">Jsonified string, in the format of {key: value}</param>
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}