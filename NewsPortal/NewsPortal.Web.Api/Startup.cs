﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NewsPortal.Web.Api.Startup))]

namespace NewsPortal.Web.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.UseWebApi(WebApiConfig.Register());
            AutoMapperConfig.Configure();
        }
    }
}
