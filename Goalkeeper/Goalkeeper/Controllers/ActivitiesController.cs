using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Goalkeeper.Models;
using Raven.Abstractions.Commands;
using Raven.Client;
using Raven.Client.Linq;

namespace Goalkeeper.Controllers
{
    public class ActivitiesController : RavenDbController
    {
        [HttpGet("api/goals/{goalId}/activities")]
        public async Task<IEnumerable<Activity>> GetByGoalId(string goalId)
        {
            return await Session.Query<Activity>()
                .Where(x => x.GoalId == goalId.Replace('-','/'))
                .ToListAsync();
        }

        [HttpGet("api/performers/{performerId}/open-activities")]
        public async Task<IEnumerable<Activity>> GetOpenByPerformer(string performerId)
        {
            return await Session.Query<Activity>()
                                .Where(x => x.PerformerId == performerId.Replace('-', '/') &&
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

        public async Task<HttpResponseMessage> Post([FromBody]Activity value)
        {
            await Session.StoreAsync(value);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public async Task<HttpResponseMessage> Put(string id, [FromBody]Activity value)
        {
            await Session.StoreAsync(value, id.Replace('-', '/'));

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public void Delete(string id)
        {
            Session.Advanced.Defer(new DeleteCommandData { Key = id.Replace('-', '/') });
        }
    }
}