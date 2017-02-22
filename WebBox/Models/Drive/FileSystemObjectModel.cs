using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using WebBox.Data.Drive;
using WebBox.Web.Http.Extensions;
using WebBox.Data.Drive.Extensions;
using System.Net.Http.Formatting;
using System.Xml.Linq;
using System.IO;
using System.Net.Http.Headers;
using System.Net;

namespace WebBox.Web.Http.Drive.Models
{
    internal class FileSystemObjectModel
    {
        public HttpResponseMessage GetOrDownload(HttpRequestMessage request, string route)
        {
            string origin = request.RequestUri.Scheme + "://" + request.RequestUri.Host;
            if (!request.RequestUri.IsDefaultPort) origin += ":" + request.RequestUri.Port;
            IEnumerable<KeyValuePair<string, string>> queryNameValuePairs = request.GetQueryNameValuePairs();
            FileSystemObject fileSystemObject = new DriveRepository().Query(origin, route, queryNameValuePairs);
            if (fileSystemObject is FileObject)
            {
                string download = queryNameValuePairs.GetValue("download");
                if (!string.IsNullOrWhiteSpace(download))
                {
                    if (download == "download" || download == "Download")
                    {
                        if (!fileSystemObject.Exists) throw new FileNotFoundException(fileSystemObject.Path);
                        return Download(fileSystemObject as FileObject);
                    }
                }
            }

            string accept = request.GetAccept();
            if (accept == "xml")
            {
                XElement element = fileSystemObject.ToElement();
                return new HttpResponseMessage()
                {
                    Content = new ObjectContent<XElement>(element, new XmlMediaTypeFormatter(), "application/xml")
                };
            }
            else
            {
                return new HttpResponseMessage()
                {
                    Content = new ObjectContent<FileSystemObject>(fileSystemObject, new JsonMediaTypeFormatter(), "application/json")
                };
            }
        }

        private HttpResponseMessage Download(FileObject fileObject)
        {
            string physicalFullPath = fileObject.GetPhysicalPath();
            FileStream fileStream = new FileStream(physicalFullPath, FileMode.Open, FileAccess.Read);

            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StreamContent(fileStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                //FileName = fileObject.Name,
                FileNameStar = fileObject.Name
            };
            return response;
        }


    }
}