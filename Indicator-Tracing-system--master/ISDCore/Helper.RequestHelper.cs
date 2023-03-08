using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ISDCore
{
    public static class RequestHelper
    {
        public static object HttpContextOnly(string Key)
        { return HttpContext.Current.Items[Key]; }
        public static void HttpContextPost(string Key, object Value)
        { HttpContext.Current.Items[Key] = Value; }
        public static object RequestForm(string Key) 
        { return HttpContext.Current.Request.Form[Key]; }
        public static object RequestOnly(string Key)
        { return HttpContext.Current.Request[Key]; }

        public static object Request(string Key)
        {
            object Ret = HttpContextOnly(Key);
            if (null == Ret)
                Ret = RequestOnly(Key);
            return Ret;
        }
        public static VarType Request<VarType>(string Key, VarType def)
        { return ConvertHelper.Convert<VarType>(Request(Key), def); }
        public static string GetMatchedKey(string Key)
        {
            string Ret = "";
            string[] Keys = HttpContext.Current.Request.Form.AllKeys;
            for (int i = 0; i < Keys.Length; i++)
            {
                if (-1 != Keys[i].IndexOf(Key))
                {
                    Ret = Keys[i]; break;
                }
            }
            return Ret;
        }
        public static NameValueCollection ServerVariables
        { get { return System.Web.HttpContext.Current.Request.ServerVariables; } }

    }
}
