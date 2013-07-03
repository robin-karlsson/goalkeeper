using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Goalkeeper.Models;
using Raven.Abstractions.Commands;
using Raven.Client;

namespace Goalkeeper.Controllers
{
    public class SampleDataController : RavenDbController
    {
        public async Task<HttpResponseMessage> Post([FromBody]string value)
        {
            var area = new Area {Name = value};
            await Session.StoreAsync(area);

            var goal = new Goal {AreaId = area.Id, Name = string.Format("Make {0} significantly better!", value)};
            await Session.StoreAsync(goal);

            var performer = new Performer {Name = string.Format("{0} Worker", value)};
            await Session.StoreAsync(performer);

            var newActivity = new Activity
                {
                    ActivityState = ActivityState.NotStarted,
                    GoalId = goal.Id,
                    PerformerId = performer.Id,
                    Title = string.Format("Evaluate {0} process",value)
                };
            await Session.StoreAsync(newActivity);

            var inProgressActivity = new Activity
            {
                ActivityState = ActivityState.InProgress,
                GoalId = goal.Id,
                PerformerId = performer.Id,
                Title = string.Format("Improve {0} process", value)
            };
            await Session.StoreAsync(inProgressActivity);

            var completedActivity = new Activity
            {
                ActivityState = ActivityState.NotStarted,
                GoalId = goal.Id,
                PerformerId = performer.Id,
                Title = string.Format("Document {0} process", value)
            };
            await Session.StoreAsync(completedActivity);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}