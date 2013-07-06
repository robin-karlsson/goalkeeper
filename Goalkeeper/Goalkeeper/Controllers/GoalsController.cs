﻿using System.Collections.Generic;
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
    public class GoalsController : RavenDbController
    {
        [HttpGet("api/areas/{areaId}/goals")]
        public async Task<object> GetGoalsByArea(string areaId)
        {
            areaId = areaId.Replace('-', '/');
            var goals = await Session.Query<Goal>()
                .Where(x => x.AreaId == areaId)
                .ToListAsync();
            var area = await Session.LoadAsync<Area>(areaId);

            return new {area, goals};
        }

        public async Task<IEnumerable<Goal>> Get()
        {
            return await Session.Query<Goal>().ToListAsync();
        }

        public async Task<Goal> Get(string id)
        {
            return await Session.LoadAsync<Goal>(id.Replace('-', '/'));
        }

        public async Task<HttpResponseMessage> Post([FromBody]Goal value)
        {
            await Session.StoreAsync(value);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public async Task<HttpResponseMessage> Put(string id, [FromBody]Goal value)
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