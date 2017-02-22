using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Xml.Linq;
using WebBox.Data.Drive;
using WebBox.Web.Http.Extensions;
using WebBox.Data.Drive.Extensions;
using System.Net.Http.Formatting;

namespace WebBox.Web.Http.Drive.Models
{
    internal class DrivesModel
    {
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            string accept = request.GetAccept();
            if (accept == "xml")
            {
                XElement element = DriveObject.Drives.ToElement();
                return new HttpResponseMessage()
                {
                    Content = new ObjectContent<XElement>(element, new XmlMediaTypeFormatter(), "application/xml")
                };
            }
            else
            {
                return new HttpResponseMessage()
                {
                    Content = new ObjectContent<IEnumerable<DriveObject>>(DriveObject.Drives, new JsonMediaTypeFormatter(), "application/json")
                };
            }
        }


    }
}