using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Project.ServicesSecurityWCF
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        { }

        protected void Session_Start(object sender, EventArgs e)
        { }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.UrlReferrer != null)
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
            }
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, DELETE, PUT, OPTIONS");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization, X-Request-With, SUDO");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.End();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}