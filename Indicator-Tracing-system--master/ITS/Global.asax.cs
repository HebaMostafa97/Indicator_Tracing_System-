using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ITS
{
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
        AreaRegistration.RegisterAllAreas();
        GlobalConfiguration.Configure(WebApiConfig.Register);
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
    protected void Session_Start()
    {
      Session.Timeout = 60;
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
      HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
      if (cookie != null && cookie.Value != null)
      {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie.Value);
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie.Value);
      }
      else
      {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
        HttpCookie cookie1 = new HttpCookie("Language");
        cookie1.Value = "en";
        Response.Cookies.Add(cookie1);
      }
    }


  }
}
