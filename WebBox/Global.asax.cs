using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using WebBox.Web.Http;

namespace WebBox.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // 
            Data.Drive.DriveObject.GetDrivesFunc = WebBox.Configuration.DrivesGetter.GetDrives;

            //
            Data.MachineObject.GetLocalFunc = WebBox.Configuration.MachineGetter.GetLocal;
            Data.Pool.PoolObject.GetPoolsFunc = WebBox.Configuration.PoolsGetter.GetPools;
        }
    }
}
