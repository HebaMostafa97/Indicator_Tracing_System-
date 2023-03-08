using System.Web.Mvc;

namespace ISDCore
{
    public class ISDPageSecured : ActionFilterAttribute
    {
        bool BypassSecurity = Settings.BypassSecurity;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string ObjectName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            FunctionSecurityContext fsc = new FunctionSecurityContext(Functions.FunctionName_Browse, ObjectName, 0);
            if (!BypassSecurity)
            {
                PermissionHelper.VerifyPermission(fsc);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
