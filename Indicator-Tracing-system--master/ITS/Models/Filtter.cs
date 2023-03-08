using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISDCore;
namespace ITS.Models
{
  public class Filtter
  {
    public static List<User> FiltterUser()
    {
      string objectName = "User";
      FunctionPermission functionPermission = new FunctionPermission();
      if (LoginToken.ObjectPermissionList != null)
        functionPermission = LoginToken.ObjectPermissionList[objectName][Functions.FunctionName_Browse];
      if (functionPermission.Restricted)
      {
        return DatabaseObject.db.Users.Where(obj => obj.OwnerID == LoginToken.LoginUser.ID).ToList();
      }
      return DatabaseObject.db.Users.ToList();
    }
    public static List<Group> FiltterGroup()
    {
      string objectName = "Group";
      FunctionPermission functionPermission = new FunctionPermission();
      if (LoginToken.ObjectPermissionList != null)
        functionPermission = LoginToken.ObjectPermissionList[objectName][Functions.FunctionName_Browse];
      if (functionPermission.Restricted)
      {
        return DatabaseObject.db.Groups.Where(obj => obj.OwnerID == LoginToken.LoginUser.ID).ToList();
      }
      return DatabaseObject.db.Groups.ToList();
    }

     public static List<Publisher> FiltterPublisher()
        {
            string objectName = "Publisher";
            FunctionPermission functionPermission = new FunctionPermission();
            if (LoginToken.ObjectPermissionList != null)
                functionPermission = LoginToken.ObjectPermissionList[objectName][Functions.FunctionName_Browse];
            if (functionPermission.Restricted)
            {
                return DatabaseObject.db.Publishers.Where(obj => obj.OwnerID == LoginToken.LoginUser.ID).ToList();
            }
            return DatabaseObject.db.Publishers.ToList();
        }

        public static List<Indicator> FiltterIndicator()
        {
            string objectName = "Indicator";
            FunctionPermission functionPermission = new FunctionPermission();
            if (LoginToken.ObjectPermissionList != null)
                functionPermission = LoginToken.ObjectPermissionList[objectName][Functions.FunctionName_Browse];
            if (functionPermission.Restricted)
            {
                return DatabaseObject.db.Indicators.Where(obj => obj.OwnerID == LoginToken.LoginUser.ID).ToList();
            }
            return DatabaseObject.db.Indicators.ToList();
        }

        public static List<IndicatorAttachment> FiltterIndicatorAttachment()
        {
            string objectName = "IndicatorAttachment";
            FunctionPermission functionPermission = new FunctionPermission();
            if (LoginToken.ObjectPermissionList != null)
                functionPermission = LoginToken.ObjectPermissionList[objectName][Functions.FunctionName_Browse];
            if(functionPermission.Restricted)
            {
                return DatabaseObject.db.IndicatorAttachments.Where(obj => obj.OwnerID == LoginToken.LoginUser.ID).OrderByDescending(c => c.CreateDate).ToList();

            }
            return DatabaseObject.db.IndicatorAttachments.OrderByDescending(c => c.CreateDate).ToList();
        }
    }
}