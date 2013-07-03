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
    public class AreasController : RavenDbController
    {
        private const string IdFormat = "Areas/{0}";

        public async Task<IEnumerable<Area>> Get()
        {
            return await Session.Query<Area>()
                .ToListAsync();
        }

        public async Task<Area> Get(string id)
        {
            return await Session.LoadAsync<Area>(string.Format(IdFormat, id));
        }

        public async Task<HttpResponseMessage> Post([FromBody]Area value)
        {
            await Session.StoreAsync(value);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public async Task<HttpResponseMessage> Put(string id, [FromBody]Area value)
        {
            await Session.StoreAsync(value, id);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        public void Delete(string id)
        {
            Session.Advanced.Defer(new DeleteCommandData { Key = string.Format(IdFormat, id) });
        }
    }
}