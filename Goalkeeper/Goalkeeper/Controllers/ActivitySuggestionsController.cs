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
    public class ActivitySuggestionsController : RavenDbController
    {
        [HttpGet("api/goals/{goalId}/open-suggestions")]
        public async Task<object> GetByGoalId(string goalId)
        {
            goalId = goalId.Replace('-', '/');
            var goal = await Session.LoadAsync<Goal>(goalId);

            var activitySuggestions = await Session.Query<ActivitySuggestion>()
                                                   .Where(x => x.Goal.Id == goalId.Replace('/','-') && x.SuggestionState == ActivitySuggestionState.Open)
                                                   .ToListAsync();

            return new { goal, activitySuggestions };
        }

        public async Task<IEnumerable<ActivitySuggestion>> Get()
        {
            return await Session.Query<ActivitySuggestion>()
                                .ToListAsync();
        }

        public async Task<ActivitySuggestion> Get(string id)
        {
            return await Session.LoadAsync<ActivitySuggestion>(id.Replace('-', '/'));
        }

        public async Task<HttpResponseMessage> Post([FromBody]ActivitySuggestion value)
        {
            value.SuggestionState = ActivitySuggestionState.Open;
            await Session.StoreAsync(value);

            var response = Request.CreateResponse(HttpStatusCode.Created, value);

            response.Headers.Location = new Uri(Url.Link("DefaultApi", new {id = value.Id}));
            return response;
        }

        [HttpPut("api/activitysuggestions/approve/{suggestionId}")]
        public async Task<HttpResponseMessage> Approve(string suggestionId)
        {
            var suggestion = await Session.LoadAsync<ActivitySuggestion>(suggestionId.Replace('-', '/'));
            suggestion.SuggestionState = ActivitySuggestionState.Approved;

            var activity = new Activity
                {
                    ActivityState = ActivityState.NotStarted,
                    Description = suggestion.Description,
                    Goal = suggestion.Goal,
                    Title = suggestion.Description.Substring(0, Math.Min(suggestion.Description.Length,50))
                };
            await Session.StoreAsync(activity);

            var response = Request.CreateResponse(HttpStatusCode.OK, activity);

            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = activity.Id }));
            return response;
        }

        public async Task<HttpResponseMessage> Put(string id, [FromBody]ActivitySuggestion value)
        {
            await Session.StoreAsync(value, id.Replace('-', '/'));

            var response = Request.CreateResponse(HttpStatusCode.OK, value);

            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = value.Id }));
            return response;
        }

        public async Task<HttpResponseMessage> Delete(string id)
        {
            var activitySuggestionId = id.Replace('-', '/');
            var activitySuggestion = await Session.LoadAsync<ActivitySuggestion>(activitySuggestionId);

            activitySuggestion.SuggestionState = ActivitySuggestionState.Rejected;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}