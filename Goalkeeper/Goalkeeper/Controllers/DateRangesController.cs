using System;
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
    public class DateRangesController : RavenDbController
    {
        [HttpGet("api/dateranges")]
        public async Task<IEnumerable<DateRange>> Get()
        {
            return await Session.Query<DateRange>()
                .ToListAsync();
        }

        [HttpGet("api/dateranges/current-latest")]
        public async Task<DateRange> GetCurrentOrLatest()
        {
            return await Session.Query<DateRange>()
                .Where(d => d.StartDate <= DateTime.Today)
                .OrderByDescending(d => d.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task<DateRange> Get(string id)
        {
            return await Session.LoadAsync<DateRange>(id.Replace('-', '/'));
        }

        public async Task<HttpResponseMessage> Post([FromBody]DateRange value)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState));
            }

            await Session.StoreAsync(value);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public async Task<HttpResponseMessage> Put(string id, [FromBody]DateRange value)
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