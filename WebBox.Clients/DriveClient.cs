using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebBox.Clients
{
    public class DriveClient
    {
        public XElement GetDrives(string requestUri)
        {
            HttpClient client = new HttpClient();
            
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            HttpResponseMessage response = client.GetAsync(requestUri).Result;
            return new XElement("");
        }

        public async Task UploadFile(string requestUri, string fileName)
        {            
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read); 
            var fileContent = new StreamContent(fs);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Path.GetFileName(fileName)
            };

            var content = new MultipartFormDataContent();
            content.Add(fileContent, "file1", fileName);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));          
            var result = await client.PostAsync(requestUri, content);

            throw new NotImplementedException();
            
        }

        public async Task UploadFile(string requestUri, string fileName, NameValueCollection collection)
        {
            throw new NotImplementedException();
        }

        public async Task UploadFiles(string requestUri, IEnumerable<string> fileNames)
        {
            throw new NotImplementedException();
        }
    
        public async Task UploadFiles(string requestUri, IEnumerable<string> fileNames, NameValueCollection collection)
        {
            throw new NotImplementedException();
        }

    }
}
