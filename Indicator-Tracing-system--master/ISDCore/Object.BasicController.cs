using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ISDCore
{
  public abstract class BasicController : Controller
  {
    [ChildActionOnly]
    public virtual ActionResult AdminView()
    {
      var items = new List<MenuItem>
      {
          new MenuItem{ Text = Resources.MapLabel["Home"] ,Action = "Control", Controller = "Home",Icon="fa fa-home", Selected="" },
          new MenuItem{ Text = Resources.MapLabel["Users"], Action = "Browse", Controller = "User",Icon="fa fa-user", Selected = ""},
          new MenuItem{ Text = Resources.MapLabel["Group"], Action = "Browse", Controller = "Group",Icon="fa fa-users", Selected = "" },
          new MenuItem{ Text = Resources.MapLabel["Indicator"] ,Action = "Browse", Controller = "Indicator",Icon="fa fa-industry", Selected="" },
          new MenuItem{ Text = Resources.MapLabel["Publisher"], Action = "Browse", Controller = "Publisher",Icon="fa fa-newspaper-o", Selected = ""},
          new MenuItem{ Text = Resources.MapLabel["Attachment"], Action = "Browse", Controller = "IndicatorAttachment",Icon="fa fa-file", Selected = "" }
      };
      string action = ControllerContext.ParentActionViewContext.RouteData.Values["action"].ToString();
      string controller = ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString();

      foreach (var item in items)
      {
        if (item.Controller == controller && item.Action == action)
        {
          item.Selected = "selected";
        }
      }
      return PartialView(items);
    }
  }
}
