using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Goalkeeper.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace Goalkeeper.Controllers
{
    public class ActivitiesController : RavenDbController
    {
        [HttpGet("api/goals/{goalId}/activities")]
        public async Task<object> GetByGoalId(string goalId)
        {
            goalId = goalId.Replace('-', '/');
            var goal = await Session.LoadAsync<Goal>(goalId);

            var activities = await Session.Query<Activity>()
                .Where(x => x.Goal.Id == goalId)
                .ToListAsync();

            return new {goal, activities};
        }

        [HttpGet("api/performers/{performerId}/open-activities")]
        public async Task<IEnumerable<Activity>> GetOpenByPerformer(string performerId)
        {
            return await Session.Query<Activity>()
                                .Where(x => x.Performer.Id == performerId.Replace('-', '/') &&
                                            x.ActivityState == ActivityState.InProgress)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> Get()
        {
            return await Session.Query<Activity>()
                .ToListAsync();
        }

        public async Task<Activity> Get(string id)
        {
            return await Session.LoadAsync<Activity>(id.Replace('-', '/'));
        }

        public async Task<IEnumerable<ActivityState>> ActivityStates()
        {
            return new[]
                {
                    ActivityState.Completed,
                    ActivityState.Deleted,
                    ActivityState.InProgress,
                    ActivityState.NotStarted
                };
        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity value)
        {
            await Session.StoreAsync(value);

            var response = Request.CreateResponse(HttpStatusCode.Created, value);

            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = value.Id }));
            return response;
        }

        [HttpPut("api/activities/{activityId}/start")]
        public async Task<HttpResponseMessage> Start(string id)
        {
            var activityId = id.Replace('-', '/');
            var activity = await Session.LoadAsync<Activity>(activityId);

            if (activity.ActivityState != ActivityState.NotStarted)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new NotSupportedException("Not allowed to start activity from other state than 'Not started'."));
            }

            activity.ActivityState = ActivityState.InProgress;

            var response = Request.CreateResponse(HttpStatusCode.OK, activity);

            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = activity.Id }));
            return response;
        }

        [HttpPut("api/activities/{activityId}/complete")]
        public async Task<HttpResponseMessage> Complete(string id)
        {
            var activityId = id.Replace('-', '/');
            var activity = await Session.LoadAsync<Activity>(activityId);

            if (activity.ActivityState != ActivityState.InProgress)
            {
                throw new NotSupportedException("Not allowed to start activity from other state than 'Not started'.");
            }

            activity.ActivityState = ActivityState.Completed;

            var response = Request.CreateResponse(HttpStatusCode.OK, activity);

            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = activity.Id }));
            return response;
        }

        [HttpPut("api/activities/{activityId}/not-started")]
        public async Task<HttpResponseMessage> NotStarted(string id)
        {
            var activityId = id.Replace('-', '/');
            var activity = await Session.LoadAsync<Activity>(activityId);

            activity.ActivityState = ActivityState.NotStarted;

            var response = Request.CreateResponse(HttpStatusCode.Created, activity);

            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = activity.Id }));
            return response;
        }

        [HttpPut("api/activities/{activityId}/change")]
        public async Task<HttpResponseMessage> Put(string id, [FromBody]Activity value)
        {
            await Session.StoreAsync(value, id.Replace('-', '/'));

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async void Delete(string id)
        {
            var activityId = id.Replace('-', '/');
            var activity = await Session.LoadAsync<Activity>(activityId);

            activity.ActivityState = ActivityState.Deleted;
            
            await Session.StoreAsync(activity, activityId);
        }
    }
}
