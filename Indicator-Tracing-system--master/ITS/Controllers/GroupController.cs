using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITS.Models;
using System.Xml;
using System.Net;
using ISDCore;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Data.Entity;

namespace ITS.Controllers
{
  [AuthorizeUser]
  public class GroupController : BasicController
  {
    [ISDPageSecured]
    public ActionResult Browse(int times = 0)
    {
      IntialSearchList();
      ViewBag.NumberOfPages = new SelectList(ISDCore.Resources.pagingList(), "value", "Text", Session["Paging"] == null ? "5" : Session["Paging"].ToString());
      ViewBag.Title = ISDCore.Language.MapValue(Resources.ResourcesFiles.MapLabel.Groups + " " + Resources.ResourcesFiles.MapLabel.List,
                                                Resources.ResourcesFiles.MapLabel.List + " " + Resources.ResourcesFiles.MapLabel.Groups);
      ViewBag.EditId = 0;
      if (Session["Paging"] == null)
        Session["Paging"] = 5;
      if (times == 1)
      {
        Session["SearchValue"] = "";
        Session["Attribute"] = "";
        Session["ObjectList"] = Filtter.FiltterGroup();
      }
      else
      {
        List<Group> groups = Session["ObjectList"] as List<Group>;
        if(groups == null)
        {
          Session["ObjectList"] = Filtter.FiltterGroup();
          Session["SearchValue"] = "";
          Session["Attribute"] = "";
        }
      }
      IntialSearchList();
      return View(Session["ObjectList"]);
    }
         
    [HttpPost]
    public ActionResult Browse(string attr, string value, int? NumOfPages)
    {
      IntialSearchList();
      ViewBag.NumberOfPages = new SelectList(ISDCore.Resources.pagingList(), "value", "Text", NumOfPages != null ? NumOfPages.ToString() : Session["Paging"].ToString());
      ViewBag.Title = ISDCore.Language.MapValue(Resources.ResourcesFiles.MapLabel.Groups + " " + Resources.ResourcesFiles.MapLabel.List,
                                                Resources.ResourcesFiles.MapLabel.List + " " + Resources.ResourcesFiles.MapLabel.Groups);
      List<Group> groups = Filtter.FiltterGroup();
      List<Group> Result = new List<Group>();
      if (NumOfPages != null)
      {
        Session["Paging"] = NumOfPages;
        return View(Session["ObjectList"]);
      }
      if (value != null)
      {
        Session["SearchValue"] = value;
        Session["Attribute"] = attr;
      }
      if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(value) && (attr == "ID"))
      {
        int id = ConvertHelper.Convert<int>(value, 0);
        Result = groups.Where(g => g.ID == id).ToList();
      }
      else if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(value) && (attr =="Name"))
      {
        Result = groups.Where(g => g.Name.ToLower().Contains(value.ToLower())).ToList();
      }
      else if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(value) && (attr == "Email"))
      {
        Result = groups.Where(g => g.Email.ToLower().Contains(value.ToLower())).ToList();
      }
      else
      {
        Result = groups.ToList();
      }
      Session["ObjectList"] = Result;
      return View(Result);
    }

    public JsonResult Active(int? ID)
    {
      Group group = DatabaseObject.db.Groups.Find(ID);
      if (group.Active == 1)
        group.Active = 0;
      else
        group.Active = 1;
      DatabaseObject.db.Entry(group).State = EntityState.Modified;
      DatabaseObject.db.SaveChanges();
      return Json("",JsonRequestBehavior.AllowGet);
    }

    public ActionResult Preview(int? id)
    {
       ViewBag.PreviewTitle = @ITS.Resources.ResourcesFiles.MapLabel.PreviewTitle;
       ViewBag.UsersOf = ITS.Resources.ResourcesFiles.MapLabel.UsersOf;
       if (id == null)
       {
         return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
       }
            Group group = DatabaseObject.db.Groups.Find(id);
            if(group==null)
            {
                return HttpNotFound();
            }
            return View(group);
    }

    public ActionResult Add()
    {
      PermissionHelper.VerifyPermission(new FunctionSecurityContext(Functions.FunctionName_Add, "Group", -1));
      return View("Form", new Group());
    }

    public ActionResult Edit(int? ID)
    {
      if (ID == null)
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      Group group = DatabaseObject.db.Groups.Find(ID);
      if (group == null)
        return HttpNotFound();
      PermissionHelper.VerifyPermission(new FunctionSecurityContext(Functions.FunctionName_Edit, "Group", group.ID, (int)group.OwnerID));
      return View("Form",group);
    }

    public ActionResult ResetEdit(int ID)
    {
      Group group = new Group() { ID = ID };
      return View("Form", group);
    }

    [HttpPost]
    public ActionResult Form(Group group,FormCollection formCollection)
    {
      group.Active = formCollection["active"] == "on" ? 1 : 0;
      group.Focus = formCollection["focus"] == "on" ? 1 : 0;
      group.NameNormalized = group.Name;
      if (!string.IsNullOrEmpty(group.NameNormalized))
        ModelState["NameNormalized"].Errors.Clear();

      if (ModelState["Focus"] != null)
        ModelState["Focus"].Errors.Clear();

      if (ModelState["Active"] != null)
        ModelState["Active"].Errors.Clear();

      if(ModelState.IsValid)
      {
        if (group.ID <= 0)
        {
          group.CreateDate = DateTime.Now;
          group.OwnerID = LoginToken.LoginUser.ID;
          DatabaseObject.db.Groups.Add(group);
          DatabaseObject.db.SaveChanges();
          XmlHelper.InsertXmlValue(group.GroupData("اضافة", "Add"), Server.MapPath("~/App_Data/Log/ITS_LOG_Group.xml"), "Operation");
          if (Request["Form"] == Resources.ResourcesFiles.MapLabel.SaveAdd)
            return RedirectToAction("Add");
          else if (Request["Form"] == Resources.ResourcesFiles.MapLabel.Save)
            return RedirectToAction(Functions.FunctionName_Browse, new { times = 1 });
        }
        else
        {
          Group g = DatabaseObject.db.Groups.Find(group.ID);
          g.Name = group.Name;
          g.NameNormalized = group.NameNormalized;
          g.Email = group.Email;
          g.Description = group.Description;
          g.HomePage = group.HomePage;
          g.ModifierID = LoginToken.LoginUser.ID;
          g.ModifyDate = DateTime.Now;
          g.Active = group.Active;
          g.SortIndex = group.SortIndex;
          g.Focus = group.Focus;
          DatabaseObject.db.Entry(g).State = EntityState.Modified;
          DatabaseObject.db.SaveChanges();
          XmlHelper.InsertXmlValue(g.GroupData("تعديل", "Edit"), Server.MapPath("~/App_Data/Log/ITS_LOG_Group.xml"), "Operation");
          if (Request["Form"] == Resources.ResourcesFiles.MapLabel.SaveEdit)
            return RedirectToAction("Edit", new { ID = group.ID });
          else if (Request["Form"] == Resources.ResourcesFiles.MapLabel.Save)
            return RedirectToAction(Functions.FunctionName_Browse, new { times = 1 });
        }
      }
      return View(group);
    }

    public JsonResult CheckDeletePermission(int? id)
    {
      bool ret = true;
      if (id == null)
        ret = false;
      Group group = DatabaseObject.db.Groups.Find(id);
      if (group == null)
        ret = false;
      if (!PermissionHelper.IsPermitted(new FunctionSecurityContext(Functions.FunctionName_Delete, "Group", group.ID, (int)group.OwnerID)))
        ret = false;
      return Json(ret, JsonRequestBehavior.AllowGet);
    }

    public JsonResult Delete(int id)
    {
      string response = "OK";
      Group group = DatabaseObject.db.Groups.Find(id);
      if (group.Users.Count() > 0)
      {
        response = Resources.ResourcesFiles.MapLabel.GroupError;
        return Json(response, JsonRequestBehavior.AllowGet);
      }
      XmlHelper.InsertXmlValue(group.GroupData("حذف", "Delete"), Server.MapPath("~/App_Data/Log/ITS_LOG_Group.xml"), "Operation");
      DatabaseObject.db.Groups.Remove(group);
      DatabaseObject.db.SaveChanges();
      List<Group> groups = (Session["ObjectList"]) as List<Group>;
      groups.Remove(group);
      Session["ObjectList"] = groups;
      return Json(response, JsonRequestBehavior.AllowGet);
    }

    public ActionResult Permission(int id)
    {
      List<ListItemObject> ObjectsName = LoadObjectsName();
      ViewBag.objectsName = new SelectList(ObjectsName, "value", "Text");
      Session["SelectedValue"] = Resources.ResourcesFiles.MapLabel.selcttObject;
      Session["CurrentGroup"] = DatabaseObject.db.Groups.Find(id);
      PermissionHelper.IsPermitted(new FunctionSecurityContext(Functions.FunctionName_Permission, "Group", id, (int)((Group)Session["CurrentGroup"]).OwnerID));
      return View();
    }
    [HttpPost]
    public ActionResult Permission(FormCollection collection)
    {
      List<ListItemObject> ObjectsName = LoadObjectsName();
      ViewBag.objectsName = new SelectList(ObjectsName, "value", "Text", collection["Objects"]);
      List<GroupPermission> groupPermissions = new List<GroupPermission>();
      var group = (Group)Session["CurrentGroup"];
      foreach (var GP in DatabaseObject.db.Groupspermission.ToList())
      {
        if (GP.Objectname == collection["Objects"] && GP.GroupID == group.ID)
        {
          groupPermissions.Add(GP);
        }
      }
      var List = ListObjectFunctions(collection["Objects"]);
      TempData["FunctionsList"] = List;
      Session["SelectedValue"] = collection["Objects"];
      return View(groupPermissions);
    }

    [HttpPost]
    public ActionResult Save(FormCollection collection)
    {
      string[] arr;
      int restricted = 0, permission = 0;
      Group group = new Group();
      for (int i = 0; i < collection.Count; i++)
      {
        arr = collection.GetKey(i).Split('-');
        if (arr[1] == EnumPermission.Allow.ToString())
          permission = 1;
        else if (arr[1] == EnumPermission.Inherit.ToString())
          permission = 0;
        else if (arr[1] == EnumPermission.Deny.ToString())
          permission = 2;
        else if (arr[1] == "Restricted")
          restricted = 1;
        group = (Group)Session["CurrentGroup"];
        if (ConvertHelper.Convert<int>(arr[0], 0) == 0)
        {
          DatabaseObject.db.Groupspermission.Add(new GroupPermission
          {
            FunctionName = arr[2], Active=1, CreateDate= DateTime.Now, Focus=0, SortIndex=0, OwnerID = LoginToken.LoginUser.ID,
             Objectname = Session["SelectedValue"].ToString(), GroupID = group.ID, Permission =permission, Restricted = restricted
          });
          DatabaseObject.db.SaveChanges();
        }
        else if (ConvertHelper.Convert<int>(arr[0], 0) > 0)
        {
          var permGroup = DatabaseObject.db.Groupspermission.Find(Convert.ToInt64(arr[0]));
          permGroup.Permission = permission;
          permGroup.Restricted = restricted;
          permGroup.ModifierID = LoginToken.LoginUser.ID;
          permGroup.ModifyDate = DateTime.Now;
          DatabaseObject.db.Entry(permGroup).State = EntityState.Modified;
          DatabaseObject.db.SaveChanges();

        }
      }
      return RedirectToAction("Permission", new { id = group.ID});
    }

    private List<ListItemObject> ListObjectFunctions(string ObjectName)
    {
      List<ListItemObject> listItemObjects = new List<ListItemObject>();
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

            if (!string.IsNullOrEmpty(funcName))
            {
              ListItemObject li = new ListItemObject();
              li.Text = ISDCore.Resources.MapLabel[funcName];
              li.value = funcName;
              listItemObjects.Add(li);
            }
          }
        }
      }
      xmlObjectMetaData = null;
      return listItemObjects;
    }

    private List<ListItemObject> LoadObjectsName()
    {
      List<ListItemObject> listItemObjects = new List<ListItemObject>();
      XmlDocument xmlDocMetaData = new XmlDocument();
      string path = Server.MapPath("~/App_Data/XML/Objects.xml");
      xmlDocMetaData.Load(path);

      XmlNodeList xmlNodeListObject = xmlDocMetaData.SelectNodes("//objects/object");
      foreach (XmlNode xmlNodeObject in xmlNodeListObject)
      {
        bool exist = false;
        if (XmlHelper.GetXmlAttributeValue(xmlNodeObject, "hidden", ref exist) != "true")
        {
          string objectName = XmlHelper.GetXmlAttributeValue(xmlNodeObject, "name", ref exist);
          ListItemObject li = new ListItemObject();
          li.Text = ISDCore.Resources.MapLabel[objectName];
          li.value = objectName;
          listItemObjects.Add(li);
        }
      }
      xmlDocMetaData = null;
      return listItemObjects;
    }
    private void IntialSearchList()
    {
      List<ListItemObject> Attr = new List<ListItemObject>
      {
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.ID, value = "ID" },
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.Name, value = "Name" },
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.Email, value="Email"}
      };
      ViewBag.AttributesName = new SelectList(Attr, "value", "Text",Session["Attribute"]== null?"": Session["Attribute"]);
    }
  }
}