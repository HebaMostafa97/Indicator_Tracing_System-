using System.Globalization;
using System.Threading;
using System.Web;

namespace ISDCore
{
    public static class Language
    {
        private static string _SESSION_KEY_USER_LANGUAGE = "SESSION_KEY_UserLanguage";
        public static HttpCookie Change(string LanguageAbbreviation)
        {
            if (LanguageAbbreviation != null)
            {
              Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbreviation);
              Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbreviation);
            }
            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LanguageAbbreviation;
            UserLanguage = LanguageAbbreviation;
            return cookie;
            //Response.Cookies.Add(cookie);
        }
        public static string MapValue(string EnglishValue,string ArabicValue)
        {
            // Language de el Language elly User byst5dmha //
            string language = UserLanguage;
             // Ret:EnglishValue de default  el ReturnValue //
            string ret = EnglishValue;
            // lw el Language NotEmpty or NotNull //
            if(!string.IsNullOrEmpty(language))
            {
                if (language == "en")
                    ret =  EnglishValue;
                else
                    ret =  ArabicValue;
            }
            return ret;
        }
        public static string UserLanguage
        {
            get { return SessionHandler.Load(_SESSION_KEY_USER_LANGUAGE, "en"); }
            set
            {
                SessionHandler.Save(_SESSION_KEY_USER_LANGUAGE, value);
            }
        }

    }
}
