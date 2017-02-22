using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBox.Data.Pool;
using WebBox.Web.Http.Pool.Models;

namespace WebBox.Web.Http.Controllers
{
    [RoutePrefix("Pools")]
    public class PoolsController : ApiController
    {
        // list
        [Route("")]
        public HttpResponseMessage Get()
        {
            return new PoolsModel().Get(Request);
        }

        [Route("{*id}")]
        public string Get(string id)
        {
            return "value";
        }

        // upload
        [Route("{*pool}")]
        public PoolFile Post(string pool)
        {
            string origin = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host;
            if (!Request.RequestUri.IsDefaultPort) origin += ":" + Request.RequestUri.Port;
            string route = "Pools";
            return new UploadModel().Upload(origin, route, pool);
        }

        // PUT: api/Pools/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [Route("{*poolwithid}")]
        public void Delete(string poolwithid)
        {
        }
    }
}
