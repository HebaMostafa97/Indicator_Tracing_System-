using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using ISDCore;
using System.Web;
using System.Web.Mvc;
using ITS.Models;

namespace ITS.Controllers
{
  [AuthorizeUser]
  public class PublisherController : BasicController
  {
    // GET: Publisher
    [ISDPageSecured]
    public ActionResult Browse(int times = 0)
    {
      ViewBag.NumberOfPages = new SelectList(ISDCore.Resources.pagingList(), "value", "Text", Session["Paging"] == null ? "5" : Session["Paging"].ToString());
      ViewBag.Title = ISDCore.Language.MapValue(Resources.ResourcesFiles.MapLabel.Publisher + " " + Resources.ResourcesFiles.MapLabel.List,
                                                Resources.ResourcesFiles.MapLabel.List + " " + Resources.ResourcesFiles.MapLabel.Publisher);
      if (Session["Paging"] == null)
        Session["Paging"] = 5;
      if (times == 1)
      {
        Session["SearchValue"] = "";
        Session["Attribute"] = "";
        Session["ObjectList"] = Filtter.FiltterPublisher();
      }
      else
      {
        List<Publisher> publishers = Session["ObjectList"] as List<Publisher>;
        if(publishers == null)
        {
          Session["ObjectList"] = Filtter.FiltterPublisher();
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
      ViewBag.Title = ISDCore.Language.MapValue(Resources.ResourcesFiles.MapLabel.Publisher + " " + Resources.ResourcesFiles.MapLabel.List,
                                               Resources.ResourcesFiles.MapLabel.List + " " + Resources.ResourcesFiles.MapLabel.Publisher);
      List<Publisher> publishers = Filtter.FiltterPublisher();
      List<Publisher> Result = new List<Publisher>();
      if (NumOfPages != null)
      {
        Session["Paging"] = NumOfPages;
        return View(Session["ObjectList"]);
      }
      if (value != null)
      {
        Session["SearchValue"] = value;
      }
      if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(value) && (attr == "ID"))
      {
        int id = ConvertHelper.Convert<int>(value, 0);
        Result = publishers.Where(u => u.ID == id).ToList();
      }
      else if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(value) && (attr == "Name"))
      {
        Result = publishers.Where(P => (Language.UserLanguage == "en" && !string.IsNullOrEmpty(P.Name_E) && P.Name_E.ToLower().Contains(value.ToLower())) ||
                 Language.UserLanguage == "Ar" && !string.IsNullOrEmpty(P.Name_A) && P.Name_A.ToLower().Contains(value.ToLower())).ToList();
      }
      else
      {
        Result = publishers;
      }
      Session["ObjectList"] = Result;
      return View(Result);
    }

    public JsonResult Active(int? ID)
    {
      Publisher publisher = DatabaseObject.db.Publishers.Find(ID);
      if (publisher.Active == 1)
        publisher.Active = 0;
      else
        publisher.Active = 1;
      DatabaseObject.db.Entry(publisher).State = EntityState.Modified;
      DatabaseObject.db.SaveChanges();
      return Json("", JsonRequestBehavior.AllowGet);
    }

    public ActionResult Preview(int? id)
    {
        ViewBag.PreviewTitle = ITS.Resources.ResourcesFiles.MapLabel.PreviewTitle;
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Publisher publisher = DatabaseObject.db.Publishers.Find(id);
        if (publisher == null)
        {
            return HttpNotFound();
        }
        return View(publisher);
    }

    // GET: Publisher/Create
    public ActionResult Add()
    {
      PermissionHelper.VerifyPermission(new FunctionSecurityContext(Functions.FunctionName_Add, "Publisher", -1));
      ViewBag.CountryID = new SelectList(DatabaseObject.db.Countries.Where(P => (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_E" && !string.IsNullOrEmpty(P.Name_E))
      || (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_A" && !string.IsNullOrEmpty(P.Name_A))), "ID", Resources.ResourcesFiles.MapLabel.LookUpName);
      return View("Form", new Publisher());
    }
    public ActionResult Edit(int? ID)
    {

      if (null == ID)
          return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      Publisher publisher = DatabaseObject.db.Publishers.Find(ID);
      if (null == publisher)
          return HttpNotFound();

      ViewBag.CountryID = new SelectList(DatabaseObject.db.Countries.Where(P => (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_E" && !string.IsNullOrEmpty(P.Name_E))
      || (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_A" && !string.IsNullOrEmpty(P.Name_A))), "ID", Resources.ResourcesFiles.MapLabel.LookUpName);
      PermissionHelper.VerifyPermission(new FunctionSecurityContext(Functions.FunctionName_Edit, "Publisher", publisher.ID, (int)publisher.OwnerID));
      return View("Form", publisher);
    }
    public ActionResult ResetEdit(int ID)
    {
      Publisher publisher = new Publisher() { ID = ID };
      return View("Form", publisher);
    }
    [HttpPost]
    public ActionResult Form(Publisher publisher, FormCollection formCollection)
    {
      ViewBag.CountryID = new SelectList(DatabaseObject.db.Countries.Where(P => (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_E" && !string.IsNullOrEmpty(P.Name_E))
      || (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_A" && !string.IsNullOrEmpty(P.Name_A))), "ID", Resources.ResourcesFiles.MapLabel.LookUpName);
      publisher.Active = formCollection["active"] == "on" ? 1 : 0;
      publisher.Focus = formCollection["focus"] == "on" ? 1 : 0;
      if (ModelState["Focus"] != null)
          ModelState["Focus"].Errors.Clear();

      if (ModelState["Active"] != null)
          ModelState["Active"].Errors.Clear();

      if (ModelState["HomePage"] != null)
          ModelState["HomePage"].Errors.Clear();

      if (ModelState.IsValid)
      {
        if(publisher.ID <= 0)
        {
          publisher.OwnerID = LoginToken.LoginUser.ID;
          publisher.CreateDate = DateTime.Now;
          DatabaseObject.db.Publishers.Add(publisher);
          DatabaseObject.db.SaveChanges();
          XmlHelper.InsertXmlValue(publisher.PublisherData("اضافة", "Add"), Server.MapPath("~/App_Data/Log/ITS_LOG_Publisher.xml"), "Operation");
          if (Request["Form"] == Resources.ResourcesFiles.MapLabel.SaveAdd)
            return RedirectToAction("Add");
          else if (Request["Form"] == Resources.ResourcesFiles.MapLabel.Save)
            return RedirectToAction(Functions.FunctionName_Browse, new { times = 1 });
        }
        else
        {
          Publisher p = DatabaseObject.db.Publishers.Find(publisher.ID);
          p.ModifierID = LoginToken.LoginUser.ID;
          p.ModifyDate = DateTime.Now;
          p.Name_A = publisher.Name_A;
          p.Name_E = publisher.Name_E;
          p.Description_A = publisher.Description_A;
          p.Description_E = publisher.Description_E;
          p.HomePage = publisher.HomePage;
          p.CountryID = publisher.CountryID;
          p.SortIndex = publisher.SortIndex;
          p.Focus = publisher.Focus;
          p.Active = publisher.Active;
          DatabaseObject.db.Entry(p).State = EntityState.Modified;
          DatabaseObject.db.SaveChanges();
          XmlHelper.InsertXmlValue(p.PublisherData("تعديل", "Edit"), Server.MapPath("~/App_Data/Log/ITS_LOG_Publisher.xml"), "Operation");
          if (Request["Form"] == Resources.ResourcesFiles.MapLabel.SaveEdit)
            return RedirectToAction("Edit", new { ID = publisher.ID });
          else if (Request["Form"] == Resources.ResourcesFiles.MapLabel.Save)
            return RedirectToAction(Functions.FunctionName_Browse, new { times = 1 });
        }
      }
      return View(publisher);
    }
    public JsonResult CheckDeletePermission(int? id)
    {
      bool ret = true;
      if (id == null)
        ret = false;
      Publisher publisher = DatabaseObject.db.Publishers.Find(id);
      if (publisher == null)
        ret = false;
      if (!PermissionHelper.IsPermitted(new FunctionSecurityContext(Functions.FunctionName_Delete, "Publisher", publisher.ID, (int)publisher.OwnerID)))
        ret = false;
      return Json(ret, JsonRequestBehavior.AllowGet);
    }
    public JsonResult Delete(int id)
    {
      Publisher publisher = DatabaseObject.db.Publishers.Find(id);
      XmlHelper.InsertXmlValue(publisher.PublisherData("حذف", "Delete"), Server.MapPath("~/App_Data/Log/ITS_LOG_Publisher.xml"), "Operation");
      DatabaseObject.db.Publishers.Remove(publisher);
      DatabaseObject.db.SaveChanges();
      List<Publisher> publishers = (Session["ObjectList"]) as List<Publisher>;
      publishers.Remove(publisher);
      Session["ObjectList"] = publishers;
      return Json("OK", JsonRequestBehavior.AllowGet);
    }
    private void IntialSearchList()
    {
      List<ListItemObject> Attr = new List<ListItemObject>
      {
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.ID, value = "ID" },
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.Name, value = "Name" },
      };
      ViewBag.AttributesName = new SelectList(Attr, "value", "Text", Session["Attribute"] == null ? "" : Session["Attribute"]);
    }
  }
}
