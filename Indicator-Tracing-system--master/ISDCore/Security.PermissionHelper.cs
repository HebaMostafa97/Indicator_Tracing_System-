using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;

namespace ISDCore
{
    public class FunctionPermission
    {
        public EnumPermission Permission = EnumPermission.Inherit;
        public bool Restricted = false;
    }

    public class FunctionPermissionList : DictionaryBase
    {
        public void Add(string FunctionName, FunctionPermission Value)
        {
            this.InnerHashtable.Add(FunctionName, Value);
        }
        public ICollection Keys
        { get { return this.InnerHashtable.Keys; } }
        public FunctionPermission this[string FunctionName]
        { get { return ConvertHelper.Convert<FunctionPermission>(this.InnerHashtable[FunctionName], null); } }
    }

    public class ObjectPermissionList : DictionaryBase
    {
        public void Add(string ObjectName, FunctionPermissionList Value)
        {
            this.InnerHashtable.Add(ObjectName, Value);
        }
        public ICollection Keys
        { get { return this.InnerHashtable.Keys; } }
        public FunctionPermissionList this[string ObjectName]
        { get { return ConvertHelper.Convert<FunctionPermissionList>(this.InnerHashtable[ObjectName], null); } }
    }
    public class FunctionSecurityContext
    {
        public string FunctionName;
        public string ObjectName;
        public int ObjectID;
        public int UserID;
        public int OwnerID;
        public FunctionSecurityContext(string FunctionName, string ObjectName, int ObjectID)
        {
            Construct(FunctionName, ObjectName, ObjectID, -1);
        }
        public FunctionSecurityContext(string FunctionName, string ObjectName, int ObjectID, int OwnerID)
        {
            Construct(FunctionName, ObjectName, ObjectID, OwnerID);
        }
        private void Construct(string FunctionName, string ObjectName, int ObjectID, int OwnerID)
        {
            this.FunctionName = FunctionName;
            this.ObjectName = ObjectName;
            this.ObjectID = ObjectID;
            this.UserID = LoginToken.LoginUser.ID;
            this.OwnerID = OwnerID;
        }
    }
    public static class PermissionHelper
    {
        public static int OwnerIDDefault = -1;
        public static FunctionPermissionList LoadGroupPermissions(int GroupID, string ObjectName)
        {
            string Query = string.Format("SELECT * FROM {0}_GroupPermission_VIW WHERE GroupID = {1} AND Objectname = '{2}'",Settings.SchemaPrefix, GroupID, ObjectName);
            return LoadPermissions(Query, ObjectName);
        }
        public static FunctionPermissionList LoadUserPermissions(int UserID, string ObjectName)
        {
            string Query = string.Format("SELECT * FROM {0}_UserPermission_VIW WHERE UserID = {1} AND Objectname = '{2}'",Settings.SchemaPrefix, UserID, ObjectName);
            return LoadPermissions(Query, ObjectName);
        }
        private static FunctionPermissionList LoadPermissions(string Query, string ObjectName)
        {
            FunctionPermissionList ret = new FunctionPermissionList();
            DataTable dt = new DataTable();
            SqlDataAdapter Adapter = new SqlDataAdapter(Query, Settings.ConnectionString);
            Adapter.Fill(dt);
            if (null != dt && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    FunctionPermission funcPermission = new FunctionPermission();
                    funcPermission.Permission = (DBNull.Value != dr["Permission"]) ? (EnumPermission)Convert.ChangeType(dr["Permission"], typeof(int)) : EnumPermission.Inherit;
                    funcPermission.Restricted = (DBNull.Value != dr["Restricted"]) ? (bool)Convert.ChangeType(dr["Restricted"], typeof(bool)) : NullHelper.Boolean;
                    string funcName = (DBNull.Value != dr["FunctionName"]) ? (string)Convert.ChangeType(dr["FunctionName"], typeof(string)) : NullHelper.String;
                    ret.Add(funcName, funcPermission);
                }
            }
            // set functions that doesn't exist in the database
            string xPath = string.Format("//objects/object[@name='{0}']", ObjectName);
            XmlDocument xmlObjectMetaData = new XmlDocument();
            string path = SessionHandler.Server.MapPath("~/App_Data/XML/Objects.xml");
            xmlObjectMetaData.Load(path);
            XmlNode xmlNodeObject = xmlObjectMetaData.SelectSingleNode(xPath);
            if (null != xmlNodeObject)
            {
                XmlNode xmlNodeFunctions = xmlNodeObject.SelectSingleNode("functions");
                if (null != xmlNodeFunctions)
                {
                    XmlNodeList xmlNodeListFunction = null;

                    bool exist = false;
                    string templateID = XmlHelper.GetXmlAttributeValue(xmlNodeFunctions, "templateId", ref exist);
                    if (exist)
                        xmlNodeListFunction = xmlObjectMetaData.SelectNodes(string.Format("//functionsTemplate[@id='{0}']/function", templateID));
                    else
                        xmlNodeListFunction = xmlNodeFunctions.SelectNodes("function");

                    foreach (XmlNode xmlNodeFunction in xmlNodeListFunction)
                    {
                        exist = false;
                        string funcName = XmlHelper.GetXmlAttributeValue<string>(xmlNodeFunction, "name", ref exist, "");

                        if (!string.IsNullOrEmpty(funcName) && null == ret[funcName]) // function doean't exist, use defaults
                            ret.Add(funcName, new FunctionPermission());
                    }
                }
            }
            xmlObjectMetaData = null;

            return ret;
        }
        public static FunctionPermissionList LoadUserPermissionsEx(int UserID, string ObjectName)
        {
            FunctionPermissionList Ret = null;

            ObjectPermissionList opl = LoginToken.ObjectPermissionList;
            if (null != opl)
                Ret = opl[ObjectName];

            if (null == Ret)
            {
                Ret = LoadUserPermissions(UserID, ObjectName);
                if (null != opl)
                    opl.Add(ObjectName, Ret);
                else
                {
                    opl = new ObjectPermissionList();
                    opl.Add(ObjectName, Ret);
                }
                LoginToken.ObjectPermissionList = opl;
            }

            return Ret;
        }
        public static bool IsPermitted(FunctionSecurityContext fsc)
        {
            bool Ret = false;
            if (null != fsc)
            {
                FunctionPermissionList fpl = LoadUserPermissionsEx(LoginToken.LoginUser.ID, fsc.ObjectName);
                FunctionPermission fp = fpl[fsc.FunctionName];
                if (null != fp)
                {
                    if (EnumPermission.Allow == fp.Permission)
                    {
                        if (fp.Restricted && 0 != fsc.ObjectID)
                            Ret = (fsc.OwnerID == LoginToken.LoginUser.ID);
                        else
                            Ret = true; // function not restricted, or no specific object
                    }
                }
                else
                    Ret = true; // function is not secured

            }
            else
                Ret = true; // no secuirty context

            return Ret;
        }
        public static void VerifyPermission(FunctionSecurityContext fsc)
        {
            if (!IsPermitted(fsc))
                Navigate(fsc.FunctionName, fsc.ObjectName, fsc.ObjectID.ToString());
        }
        private static void Navigate(string FunctionName, string ObjectName, string ObjectID)
        {
            RequestHelper.HttpContextPost("FunctionName", FunctionName);
            RequestHelper.HttpContextPost("ObjectName", ObjectName);
            RequestHelper.HttpContextPost("ObjectID", ObjectID);
            HttpContext.Current.Response.RedirectToRoute(new { Controller = Settings.HomeController, Action = Settings.AccessDeniedAction });
        }

    }

}
