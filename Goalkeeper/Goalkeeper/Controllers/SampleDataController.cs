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

            var areaId = new NameId {Id = area.Id, Name = area.Name};
            var periodId = new NameId {Id = period.Id, Name = period.Name};

            var goal = new Goal {Area = areaId, DateRange = periodId, Name = string.Format("Make {0} significantly better!", value)};
            await Session.StoreAsync(goal);

            var goalId = new NameId {Id = goal.Id, Name = goal.Name};

            var performer = new Performer {Name = string.Format("{0} Worker", value)};
            await Session.StoreAsync(performer);

            var performerId = new NameId {Id = performer.Id, Name = performer.Name};

            var newActivity = new Activity
                {
                    ActivityState = ActivityState.NotStarted,
                    Goal = goalId,
                    Performer = performerId,
                    Title = string.Format("Evaluate {0} process",value),
                    Abstract = "A short summary of this activity"
                };
            await Session.StoreAsync(newActivity);

            var inProgressActivity = new Activity
            {
                ActivityState = ActivityState.InProgress,
                Goal = goalId,
                Performer = performerId,
                Title = string.Format("Improve {0} process", value),
                Abstract = "A short summary of this activity"
            };
            await Session.StoreAsync(inProgressActivity);

            var completedActivity = new Activity
            {
                ActivityState = ActivityState.Completed,
                Goal = goalId,
                Performer = performerId,
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