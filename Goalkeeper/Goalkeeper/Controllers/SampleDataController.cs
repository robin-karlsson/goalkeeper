using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Goalkeeper.Models;
using Raven.Abstractions.Data;

namespace Goalkeeper.Controllers
{
    public class SampleDataController : RavenDbController
    {
        public async Task<HttpResponseMessage> Post([FromBody]string value)
        {
            var area = new Area {Name = value};
            await Session.StoreAsync(area);

            var period = new DateRange
                {
                    Name = "2013",
                    StartDate = new DateTime(2013, 1, 1),
                    EndDate = new DateTime(2013, 12, 31)
                };
            await Session.StoreAsync(period);

            var goal = new Goal {AreaId = area.Id, DateRangeId = period.Id, Name = string.Format("Make {0} significantly better!", value)};
            await Session.StoreAsync(goal);

            var performer = new Performer {Name = string.Format("{0} Worker", value)};
            await Session.StoreAsync(performer);

            var newActivity = new Activity
                {
                    ActivityState = ActivityState.NotStarted,
                    GoalId = goal.Id,
                    PerformerId = performer.Id,
                    Title = string.Format("Evaluate {0} process",value),
                    Abstract = "A short summary of this activity"
                };
            await Session.StoreAsync(newActivity);

            var inProgressActivity = new Activity
            {
                ActivityState = ActivityState.InProgress,
                GoalId = goal.Id,
                PerformerId = performer.Id,
                Title = string.Format("Improve {0} process", value),
                Abstract = "A short summary of this activity"
            };
            await Session.StoreAsync(inProgressActivity);

            var completedActivity = new Activity
            {
                ActivityState = ActivityState.Completed,
                GoalId = goal.Id,
                PerformerId = performer.Id,
                Title = string.Format("Document {0} process", value),
                Abstract = "A short summary of this activity"
            };
            await Session.StoreAsync(completedActivity);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public async void Delete()
        {
            await
                Session.Advanced.DocumentStore.AsyncDatabaseCommands.DeleteByIndexAsync("AllDocumentsById",
                                                                                        new IndexQuery(),
                                                                                        allowStale: false);
        }
    }
}