using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ULoveWebService.Models;

namespace ULoveWebService.Attributes
{
    public class TokenAuthorizeAttribute : ActionFilterAttribute
    {
        private ULoveWebServiceContext db = new ULoveWebServiceContext();
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var uids = actionContext.Request.Headers.Where(m => m.Key == "Uid").Select(m => m.Value).FirstOrDefault();
            var tokens = actionContext.Request.Headers.Where(m => m.Key == "Token").Select(m => m.Value).FirstOrDefault();
            if (uids == null || tokens == null)
            {
                
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }
            var user = db.Users.AsNoTracking().Where(m => m.Uid == uids.FirstOrDefault()).FirstOrDefault();
            if (user == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;
            }
            if (user.Token != (string)tokens.FirstOrDefault())
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                var identity = new GenericIdentity(uids.FirstOrDefault(), "basic");
                actionContext.RequestContext.Principal = new GenericPrincipal(identity, null);
            }
        }
    }
}