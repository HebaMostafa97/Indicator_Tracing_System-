namespace ISDCore
{
    public enum EnumLoginStatus
    { Unknown = 0, Success, UserNotFound, InvalidAccount, AccountBlocked, Inconsistent = -1 } //, InvalidSSNIntegration

    public static class LoginToken
    {
        private static string _SESSION_KEY_USER_OBJECT = "SESSION_KEY_UserObject";
        private static string _SESSION_KEY_USER_LOGINSTATUS = "SESSION_KEY_UserLoginStatus";
        private static string _SESSION_KEY_USER_OBJECTPERMISSIONLIST = "UserObjectPermissionList";

        public static UserObject LoginUser
        {
            get { return SessionHandler.Load<UserObject>(_SESSION_KEY_USER_OBJECT, null); }
            set
            {
                SessionHandler.Save<UserObject>(_SESSION_KEY_USER_OBJECT, value);
                if (null != value)
                {
                    LoginStatus = EnumLoginStatus.Success;
                }
            }
        }

        public static EnumLoginStatus LoginStatus
        {
            get
            {
                EnumLoginStatus Ret = SessionHandler.Load<EnumLoginStatus>(_SESSION_KEY_USER_LOGINSTATUS, EnumLoginStatus.Unknown);
                if (EnumLoginStatus.Success == Ret && null == LoginUser)
                    Ret = EnumLoginStatus.Inconsistent;
                return Ret;
            }
            set
            {
                SessionHandler.Save<EnumLoginStatus>(_SESSION_KEY_USER_LOGINSTATUS, value);
                if (EnumLoginStatus.Success != value)
                {
                    LoginUser = null;
                }
            }
        }

        public static ObjectPermissionList ObjectPermissionList
        {
            get { return SessionHandler.Load<ObjectPermissionList>(_SESSION_KEY_USER_OBJECTPERMISSIONLIST, null); }
            set { SessionHandler.Save<ObjectPermissionList>(_SESSION_KEY_USER_OBJECTPERMISSIONLIST, value); }
        }
    }
}
