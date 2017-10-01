using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedHardware.Data;
using SharedHardware.Models;
using Microsoft.EntityFrameworkCore;
using SharedHardware.NetworkTypes;

namespace SharedHardware.Controllers
{
    [Produces("application/json")]
    [Route("api/Monitor")]
    public class MonitorController : Controller
    {
        private Context db;
        public MonitorController(Context db) => this.db = db;        

        private async Task TouchPlatform(Platform platform)
        {
            var now = DateTime.UtcNow;
            if ((now - platform.LastUptime.EndDate).TotalMinutes > 5) //TODO: system setting - move somewhere to config
            {
                PlatformUptimeSpan newSpan = new PlatformUptimeSpan
                {
                    PlatformId = platform.Id,
                    SpanId = platform.LastUptime.SpanId + 1,
                    StartDate = now,
                    EndDate = now,
                };
                platform.LastUptime = newSpan;
            }
            else
            {
                platform.LastUptime.EndDate = now;
            }
            db.Platform.Update(platform);
            await db.SaveChangesAsync();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(NodeStatusUpdate statusUpdate)
        {
            var platform = await 
                (from p in db.Platform.Include(p => p.LastUptime)
                 where (p.Id == statusUpdate.PlatformId)
                 select p).FirstOrDefaultAsync();
            if (platform == null) return NotFound();
            await TouchPlatform(platform);

            var now = DateTime.UtcNow;
            var nextRuns = await
                (from r in db.Runs.Include(r => r.Computation)
                where r.PlatformId == platform.Id && r.StartTime <= now
                select r).ToArrayAsync();
            var freeResources = new HashSet<byte>(statusUpdate.FreeSharedResources);
            nextRuns =
                (from r in nextRuns
                 where freeResources.Contains(r.SharedResourceId)
                 select r).ToArray();
            if (nextRuns.Length > 0)
            {
                return Json(new NodeStatusUpdateResult {
                    ToStart = 
                        nextRuns.Select(r => 
                            new SharedResourceStatusUpdateResult
                            {
                                ComputationId = r.ComputationId,
                                ComputationUrl = r.Computation.BundleUrl,
                                SharedResourceId = r.SharedResourceId
                            }).ToArray()
                });
            }
            return NoContent();
        }
    }
}