﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Goalkeeper.Models;
using Raven.Abstractions.Commands;
using Raven.Client;

namespace Goalkeeper.Controllers
{
    public class PerformersController : RavenDbController
    {
        public async Task<IEnumerable<Performer>> Get()
        {
            return await Session.Query<Performer>()
                .ToListAsync();
        }

        public async Task<Performer> Get(string id)
        {
            return await Session.LoadAsync<Performer>(id.Replace('-', '/'));
        }

        public async Task<HttpResponseMessage> Post([FromBody]Performer value)
        {
            await Session.StoreAsync(value);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public async Task<HttpResponseMessage> Put(string id, [FromBody]Performer value)
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