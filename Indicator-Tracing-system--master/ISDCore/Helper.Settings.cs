using System;
using System.Configuration;

namespace ISDCore
{
    public static class Settings
    {
        static Settings()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        }
        static string APP_SETTING = "__APP_SETTING_";
        public static string ReadAppSettings(string key)
        { return (null != ConfigurationManager.AppSettings[key]) ? ConvertHelper.Convert<string>(ConfigurationManager.AppSettings[key].ToString(), "") : ""; }

        private static string _ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public static string ConnectionString
        { get { return _ConnectionString; } }

        public static string ReadConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ToString();
        }

        private static string _XmlPath = ReadAppSettings("xmlPath");
        public static string XmlPath
        { get { return SessionHandler.Server.MapPath(string.Format("{0}/{1}", _ServerRootPath, _XmlPath)); } }

        private static string _ObjectsXml = ReadAppSettings("objectsXml");
        public static string ObjectsXml
        { get { return string.Format("{0}{1}", XmlPath, _ObjectsXml); } }

        private static string _LookupsXml = ReadAppSettings("lookupsXml");
        public static string LookupsXml
        { get { return string.Format("{0}{1}", XmlPath, _LookupsXml); } }

        private static string _ServerUrl = ReadAppSettings("serverUrl");
        public static string ServerUrl
        { get { return _ServerUrl; } }

        private static string _ServerRootPath = ReadAppSettings("serverRootPath");
        public static string ServerRootPath
        { get { return _ServerRootPath; } }

        public static string ServerRootPhysicalPath
        { get { return SessionHandler.Server.MapPath(_ServerRootPath); } }

        private static string _VirtualDirectory = ReadAppSettings("virtualDirectory");
        public static string VirtualDirectory
        { get { return _VirtualDirectory; } }

        private static string _NavigateUrl = ReadAppSettings("navigateUrl");
        public static string NavigateUrl
        { get { return string.Format("{0}/{1}", _ServerRootPath, _NavigateUrl); } }

        private static string _HomeController = ReadAppSettings("HomeController");
        public static string HomeController
        { get { return _HomeController; } }

        private static string _HomeAction = ReadAppSettings("HomeAction");
        public static string HomeAction
        { get { return _HomeAction; } }

        private static string _AccessDeniedAction = ReadAppSettings("AccessDeniedAction");
        public static string AccessDeniedAction
        { get { return _AccessDeniedAction; } }


        private static string _ServerDateFormat = ReadAppSettings("serverDateFormat");
        public static string ServerDateFormat
        { get { return _ServerDateFormat; } }

        private static string _DatabaseDateFormat = ReadAppSettings("databaseDateFormat");
        public static string DatabaseDateFormat
        { get { return _DatabaseDateFormat; } }

        private static string _DatabaseDateTimeFormat = ReadAppSettings("databaseDateTimeFormat");
        public static string DatabaseDateTimeFormat
        { get { return _DatabaseDateTimeFormat; } }

        private static string _SSRSDateFormat = ReadAppSettings("ssrsDateFormat");
        public static string SSRSDateFormat
        { get { return _SSRSDateFormat; } }

        private static string _SSRSDateTimeFormat = ReadAppSettings("ssrsDateTimeFormat");
        public static string SSRSDateTimeFormat
        { get { return _SSRSDateTimeFormat; } }

        private static string _SchemaPrefix = ReadAppSettings("schemaPrefix");
        public static string SchemaPrefix
        { get { return _SchemaPrefix; } }

        private static int _PageStep = ConvertHelper.Convert<int>(ReadAppSettings("pageStep"), 10);
        public static int PageStep
        { get { return _PageStep; } }

        private static EnumLanguage _LookupLanguage = EnumHelper.MapLanguage(ReadAppSettings("lookupLanguage"));
        public static EnumLanguage LookupLanguage
        { get { return _LookupLanguage; } }

        private static string _DefaultTheme = ReadAppSettings("defaultTheme");
        public static string DefaultTheme
        { get { return _DefaultTheme; } }

        private static bool _MarkMissingResources = ConvertHelper.Convert<bool>(ReadAppSettings("markMissingResources"), false);
        public static bool MarkMissingResources
        { get { return _MarkMissingResources; } }

        private static string _EmailContactUs = ReadAppSettings("emailContactUs");
        public static string EmailContactUs
        { get { return _EmailContactUs; } }

        private static string _Organization = ReadAppSettings("Organization");
        public static string Organization
        { get { return _Organization; } }

        private static bool _EmailUnifyFrom = ConvertHelper.Convert<bool>(ReadAppSettings("email.UnifyFrom"), false);
        public static bool EmailUnifyFrom
        { get { return _EmailUnifyFrom; } }

        private static string _WebAdminEmail = ReadAppSettings("webAdminEmail");
        public static string WebAdminEmail
        { get { return _WebAdminEmail; } }

        private static string _WebAdminName = ReadAppSettings("webAdminName");
        public static string WebAdminName
        { get { return _WebAdminName; } }

        private static string _NotifyEmailBodyHtml = ReadAppSettings("notifyBodyHtml");
        public static string NotifyEmailBodyHtml
        { get { return string.Format("{0}{1}", XmlPath, _NotifyEmailBodyHtml); } }

        private static string _SmtpServer = ReadAppSettings("smtpServer");
        public static string SmtpServer
        { get { return _SmtpServer; } }
        private static string _SmtpServerCredential = ReadAppSettings("smtpServer.credential");
        public static string SmtpServerUsername
        {
            get
            {
                string[] credential = _SmtpServerCredential.Split(' ');
                return credential[0];
            }
        }
        public static string SmtpServerPassword
        {
            get
            {
                string[] credential = _SmtpServerCredential.Split(' ');
                return credential[1];
            }
        }

        private static string _GrantedIPs = ReadAppSettings("grantedIPs");
        public static string GrantedIPs
        { get { return _GrantedIPs; } }

        private static bool _BypassSecurity = ConvertHelper.Convert<bool>(ReadAppSettings("bypassSecurity"), false);
        public static bool BypassSecurity
        { get { return _BypassSecurity; } }

        private static string _ReportServer = ReadAppSettings("reportserver.url");
        public static Uri ReportServer
        { get { return new Uri(_ReportServer); } }

        private static string _ReportsFolder = ReadAppSettings("reportserver.reportsfolder");
        public static string ReportsFolder
        { get { return _ReportsFolder; } }
       
        private static string _LogController = ReadAppSettings("LogController");
        public static string LogController
        { get { return _LogController; } }
        private static string _LogAction = ReadAppSettings("LogAction");
        public static string LogAction
        { get { return _LogAction; } }
        private static string _StartPage = ReadAppSettings("StartPage");
        public static string StartPage
        { get { return _StartPage; } }
       




    private static string _ReportServerCredential = ReadAppSettings("reportserver.credential");
        public static string ReportServerUsername
        {
            get
            {
                string[] credential = _ReportServerCredential.Split(' ');
                return credential[0];
            }
        }
        public static string ReportServerPassword
        {
            get
            {
                string[] credential = _ReportServerCredential.Split(' ');
                return credential[1];
            }
        }

        private static string _WebServiceCredential = ReadAppSettings("WebService.credential");
        public static string WebServiceUsername
        {
            get
            {
                string[] credential = _WebServiceCredential.Split(' ');
                return credential[0];
            }
        }
        public static string WebServicePassword
        {
            get
            {
                string[] credential = _WebServiceCredential.Split(' ');
                return credential[1];
            }
        }

    }
}
