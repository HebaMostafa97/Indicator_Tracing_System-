using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace ISDCore
{
    public class SessionHandler
    {
        //public static HttpSessionState Session
        //{ get { return HttpContext.Current.Session; } }

        public static VarType Load<VarType>(string Key, VarType def)
        { return ConvertHelper.Convert<VarType>(HttpContext.Current.Session[Key], def); }

        public static void Save<VarType>(string Key, VarType val)
        {
            HttpContext.Current.Session[Key] = val;
        }

        public static HttpServerUtility Server
        { get { return HttpContext.Current.Server; } }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.RemoveAll();
            FormsAuthentication.SignOut();

        }
    }
    public static class ContextHandler
    {
        public static IDictionary Items
        { get { return HttpContext.Current.Items; } }

        public static VarType Load<VarType>(string Key, VarType def)
        { return ConvertHelper.Convert<VarType>(Items[Key], def); }

        public static void Save<VarType>(string Key, VarType val)
        { Items[Key] = val; }
    }

    public class ViewStateHandler
    {
        private StateBag _ViewState = null;

        public ViewStateHandler(StateBag ViewState)
        {
            _ViewState = ViewState;
        }

        public VarType Load<VarType>(string Key, VarType def)
        { return ConvertHelper.Convert<VarType>(_ViewState[Key], def); }

        public void Save<VarType>(string Key, VarType val)
        { _ViewState[Key] = val; }
    }
}
