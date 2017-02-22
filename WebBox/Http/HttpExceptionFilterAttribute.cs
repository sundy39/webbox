using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;
using WebBox.Web.Http.Extensions;

namespace WebBox.Web.Http.Filters
{
    public class HttpExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string accept = actionExecutedContext.Request.GetAccept();

            if (accept == "json")
            {
                Exception exception = actionExecutedContext.Exception;
                var error = new
                {
                    Error = new
                    {
                        ExceptionMessage = exception.Message,
                        ExceptionType = exception.GetType().FullName,
                        StackTrace = exception.StackTrace
                    }
                };
                ObjectContent content = new ObjectContent(error.GetType(), error, new JsonMediaTypeFormatter(), "application/json");
                actionExecutedContext.Response = new HttpResponseMessage() { Content = content };
            }

            base.OnException(actionExecutedContext);
        }


    }
}