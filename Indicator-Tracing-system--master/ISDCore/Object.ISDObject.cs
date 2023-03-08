using System;
namespace ISDCore
{
    public class ISDObject
    {
        public static void RaiseException(string Message)
        {
            throw (new ApplicationException(Message));
        }
        public static void RaiseException(string format, params object[] args)
        {
            RaiseException(string.Format(format, args));
        }
    }
}
