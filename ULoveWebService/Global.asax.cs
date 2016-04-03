using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ULoveWebService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public WebApiApplication()
        {
            AuthorizeRequest += WebApiApplication_AuthorizeRequest;
        }

        private void WebApiApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            var identity = Context.User.Identity;
            if (identity.IsAuthenticated)
            {
                var roles = new string[] { "guest" };
                Context.User = new GenericPrincipal(identity, roles);
            }
        }

        protected void Application_Start()
        {
            
            GlobalConfiguration.Configure(WebApiConfig.Register);
            System.Data.Entity.Database.SetInitializer(new ULoveWebService.Models.ULoveWebServiceContextInitializer());
            AreaRegistration.RegisterAllAreas();
        }
    }
}
