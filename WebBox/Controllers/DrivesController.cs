using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBox.Data.Drive;
using WebBox.Web.Http.Drive.Models;

namespace WebBox.Web.Http.Controllers
{
    [RoutePrefix("Drives")]
    public class DrivesController : ApiController
    {
        // list
        [Route("")]    
        public HttpResponseMessage Get()
        {
            return new DrivesModel().Get(Request);
        }

        // query & download
        [Route("{*drive}")]
        public HttpResponseMessage Get(string drive)
        {
            return new FileSystemObjectModel().GetOrDownload(Request, GetRoute(drive));
        }

        // upload
        [Route("{*drive}")]
        public void Post(string drive)
        {
            new UploadModel().Upload(GetRoute(drive));
        }     
  
        [Route("{*drive}")]
        public void Put(string drive)
        {
            string origin = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host;
            if (!Request.RequestUri.IsDefaultPort) origin += ":" + Request.RequestUri.Port;
            new DriveRepository().Put(origin, GetRoute(drive), Request.GetQueryNameValuePairs());
        }

        [Route("{*drive}")]
        public void Delete(string drive)
        {
            string origin = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host;
            if (!Request.RequestUri.IsDefaultPort) origin += ":" + Request.RequestUri.Port;            
            new DriveRepository().Delete(origin, GetRoute(drive), Request.GetQueryNameValuePairs());
        }

        private const string RoutePrefix = "Drives";

        private string GetRoute(string drive)
        {            
            string route = "/" + RoutePrefix + "/" + drive;
            return route;
        }


    }
}
