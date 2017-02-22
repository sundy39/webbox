using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Xml.Linq;
using WebBox.Web.Http.Extensions;
using WebBox.Data.Pool;
using WebBox.Data.Pool.Extensions;

namespace WebBox.Web.Http.Pool.Models
{
    internal class PoolsModel
    {
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            string accept = request.GetAccept();
            if (accept == "xml")
            {
                XElement element = PoolObject.Pools.ToElement();
                return new HttpResponseMessage()
                {
                    Content = new ObjectContent<XElement>(element, new XmlMediaTypeFormatter(), "application/xml")
                };
            }
            else
            {
                return new HttpResponseMessage()
                {
                   Content = new ObjectContent<IEnumerable<PoolObject>>(PoolObject.Pools, new JsonMediaTypeFormatter(), "application/json")
                };
            }
        }
    }
}