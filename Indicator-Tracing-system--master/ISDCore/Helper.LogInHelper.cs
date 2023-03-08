using System;
using System.Web;
using System.Web.Security;

namespace ISDCore
{
    public static class LogInHelper
    {
        public static string LogoutController = Settings.LogController;

        public static string LogoutAction = Settings.LogAction;

        public static string SuccessController = Settings.HomeController;

        public static string SuccessAction = Settings.StartPage;

        public static string FailController = Settings.LogController;

        public static string FailAction = Settings.LogAction;


        public static EnumLoginStatus LogIn(UserObject user , LogInModel logInModel)
        {
            EnumLoginStatus loginStatus = EnumLoginStatus.Success;
            if (user != null)
            {
                if (ConvertHelper.CompareByte(user.Password,ConvertHelper.EncryptValue(logInModel.password)))
                {
                    bool active = (user.Active == 1) ? true : false;
                    if (!active)
                        loginStatus = EnumLoginStatus.AccountBlocked;
                }
                else
                {
                    loginStatus = EnumLoginStatus.InvalidAccount;
                }

            }
            else
            {
                loginStatus = EnumLoginStatus.UserNotFound;
            }
            if (EnumLoginStatus.Success == loginStatus)
            {
                user.LastLogonDate = DateTime.Now;
                user.LogInCount = user.LogInCount + 1;
                LoginToken.LoginUser = user;
                LoginToken.LoginStatus = loginStatus;
                HttpContext.Current.Session["UserIsLogIn"] = true;
                FormsAuthentication.SetAuthCookie(user.Username, true);
            }
            return loginStatus;
        }
        public static void LogOut()
        {
            SessionHandler.ClearSession();
        }
        public static bool ForgetPassword(UserObject user)
        {
            bool ret = false;
            if (user != null)
            {
                string content = "username is " + user.Username + " " + "And Password Is " + user.Password;
                try
                {
                    MailHelper.sendEmail(user.Email, content);
                    ret = true;
                }
                catch
                {
                    ret = true;
                }
            }
            else
            {
                ret = true;
            }
            return ret;
        }

    }
}
