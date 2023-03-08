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
using ITS.App_Code;
using System.Globalization;
using System.Threading;
using System.Xml.Linq;
// Don't use Namespace PagedList //

namespace ITS.Controllers
{
  //[AuthorizeUser]
  public class IndicatorController : BasicController
  {
    //[ISDPageSecured]
    public ActionResult Browse(int times = 0)
    {
       // DropDownlist mn pagedlist //
      // if Session["Paging"] lw b null yb2a b 5 8er kda Session["PagedList"].toString() //
      ViewBag.NumberOfPages = new SelectList(ISDCore.Resources.pagingList(), "value", "Text", Session["Paging"] == null ? "5" : Session["Paging"].ToString());
      ViewBag.Title = ISDCore.Language.MapValue(Resources.ResourcesFiles.MapLabel.Indicators + " " + Resources.ResourcesFiles.MapLabel.List,
                                               Resources.ResourcesFiles.MapLabel.List + " " + Resources.ResourcesFiles.MapLabel.Indicators);
      if (Session["Paging"] == null)
        Session["Paging"] = 5;
      if (times == 1)
      {
        Session["Attribute"] = "";
        Session["SearchValue"] = "";
        Session["FromDate"] = "";
        Session["ToDate"] = "";
                // Session //
        Session["ObjectList"] = Filtter.FiltterIndicator();
      }
      else
      {
        List<Indicator> indicators = Session["ObjectList"] as List<Indicator>;
        if (indicators == null)
        {
          Session["ObjectList"] = Filtter.FiltterIndicator();
          Session["SearchValue"] = "";
          Session["FromDate"] = "";
          Session["ToDate"] = "";
          Session["Attribute"] = "";
        }
      }
      IntialSearchList();
      return View(Session["ObjectList"]);
    }
    [HttpPost]
    public ActionResult Browse(string attr, string value,string FromDate,string ToDate, int? NumOfPages)
    {
      IntialSearchList();
      ViewBag.NumberOfPages = new SelectList(ISDCore.Resources.pagingList(), "value", "Text", NumOfPages != null ? NumOfPages.ToString() : Session["Paging"].ToString());
      ViewBag.Title = ISDCore.Language.MapValue(Resources.ResourcesFiles.MapLabel.Indicators + " " + Resources.ResourcesFiles.MapLabel.List,
                                               Resources.ResourcesFiles.MapLabel.List + " " + Resources.ResourcesFiles.MapLabel.Indicators);
      List<Indicator> indicators = Filtter.FiltterIndicator();
      List<Indicator> Result = new List<Indicator>();
      if (NumOfPages != null)
      {
        Session["Paging"] = NumOfPages;
        return View(Session["ObjectList"]);
      }
      if (value != null)
      {
        Session["SearchValue"] = value;
        Session["Attribute"] = attr;
        Session["FromDate"] = "";
        Session["ToDate"] = "";
      }
      if((attr == "StartDate" || attr == "EndDate") && FromDate != null && ToDate != null )
      {
        Session["FromDate"] = FromDate;
        Session["ToDate"] = ToDate;
      }
      if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(value) && (attr == "ID"))
      {
        int id = ConvertHelper.Convert<int>(value, 0);
        Result = indicators.Where(u => u.ID == id).ToList();
      }
      else if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(value) && (attr == "Name"))
      {
        Result = indicators.Where(u => (Language.UserLanguage == "en" && !string.IsNullOrEmpty(u.Name_E) && u.Name_E.ToLower().Contains(value.ToLower())) ||
                 Language.UserLanguage == "Ar" && !string.IsNullOrEmpty(u.Name_A) && u.Name_A.ToLower().Contains(value.ToLower())).ToList();
      }
      else if(!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate) && (attr == "StartDate"))
      {
        try
        {
          var From = GetCurrentDate(FromDate);
          var To = GetCurrentDate(ToDate);
          Result = indicators.Where(u => (u.StartMonth.Ticks >= From.Ticks && u.StartMonth.Ticks <= To.Ticks)).ToList();
        }
        catch
        {

        }
        
      }
      else if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate) && (attr == "EndDate"))
      {
        try
        {
          var From = GetCurrentDate(FromDate);
          var To = GetCurrentDate(ToDate);
          Result = indicators.Where(u => (u.EndMonth.Ticks >= From.Ticks && u.EndMonth.Ticks <= To.Ticks)).ToList();
        }
        catch
        {

        }

      }
      else
      {
        Result = indicators;
      }
      Session["ObjectList"] = Result;
      return View(Result);
    }


    public JsonResult Active(int? ID)
    {
      Indicator indicator = DatabaseObject.db.Indicators.Find(ID);
      if (indicator.Active == 1)
        indicator.Active = 0;
      else
        indicator.Active = 1;
      DatabaseObject.db.Entry(indicator).State = EntityState.Modified;
      DatabaseObject.db.SaveChanges();
      return Json("", JsonRequestBehavior.AllowGet);
    }


    public ActionResult Preview(int? id)
    {
      ViewBag.PreviewTitle = @ITS.Resources.ResourcesFiles.MapLabel.PreviewTitle;
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Indicator indicator = DatabaseObject.db.Indicators.Find(id);
      if (indicator == null)
      {
        return HttpNotFound();
      }
      return View(indicator);
    }



    // GET: Publisher/Create
    public ActionResult Add()
    {
      DateTime dd = DateTime.Now;
      PermissionHelper.VerifyPermission(new FunctionSecurityContext(Functions.FunctionName_Add, "Indicator", -1));
      ViewBag.PublisherID = new SelectList(DatabaseObject.db.Publishers.Where(P => (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_E" && !string.IsNullOrEmpty(P.Name_E))
      || (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_A" && !string.IsNullOrEmpty(P.Name_A))), "ID", Resources.ResourcesFiles.MapLabel.LookUpName);
      ViewBag.PeriodicityID = new SelectList(DatabaseObject.db.Periodicities.Where(P => (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_E" && !string.IsNullOrEmpty(P.Name_E))
      || (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_A" && !string.IsNullOrEmpty(P.Name_A))), "ID", Resources.ResourcesFiles.MapLabel.LookUpName);
      // rerurn new Indicator to Add New Indicator//
      return View("Form", new Indicator() { StartMonth = DateTime.Now, EndMonth = DateTime.Now });
    }


    public ActionResult Edit(int? ID)
    {
      if (null == ID)
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

      Indicator indicator = DatabaseObject.db.Indicators.Find(ID);
      if (null == indicator)
        return HttpNotFound();

      ViewBag.PublisherID = new SelectList(DatabaseObject.db.Publishers.Where(P => (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_E" && !string.IsNullOrEmpty(P.Name_E))
      || (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_A" && !string.IsNullOrEmpty(P.Name_A))), "ID", Resources.ResourcesFiles.MapLabel.LookUpName);
      ViewBag.PeriodicityID = new SelectList(DatabaseObject.db.Periodicities.Where(P => (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_E" && !string.IsNullOrEmpty(P.Name_E))
      || (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_A" && !string.IsNullOrEmpty(P.Name_A))), "ID", Resources.ResourcesFiles.MapLabel.LookUpName);
      PermissionHelper.VerifyPermission(new FunctionSecurityContext(Functions.FunctionName_Edit, "Indicator", indicator.ID, (int)indicator.OwnerID));
      return View("Form", indicator);
    }


    public ActionResult ResetEdit(int ID)
    {
      Indicator indicator = new Indicator() { ID = ID };
      return View("Form", indicator);
    }

    
    [HttpPost]
    public ActionResult Form(Indicator indicator, FormCollection formCollection)
    {
      ViewBag.PublisherID = new SelectList(DatabaseObject.db.Publishers.Where(P => (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_E" && !string.IsNullOrEmpty(P.Name_E))
      || (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_A" && !string.IsNullOrEmpty(P.Name_A))), "ID", Resources.ResourcesFiles.MapLabel.LookUpName);
      ViewBag.PeriodicityID = new SelectList(DatabaseObject.db.Periodicities.Where(P => (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_E" && !string.IsNullOrEmpty(P.Name_E))
      || (Resources.ResourcesFiles.MapLabel.LookUpName == "Name_A" && !string.IsNullOrEmpty(P.Name_A))), "ID", Resources.ResourcesFiles.MapLabel.LookUpName);
      indicator.Active = formCollection["active"] == "on" ? 1 : 0;
      indicator.Focus = formCollection["focus"] == "on" ? 1 : 0;

      if (ModelState["Focus"] != null)
        ModelState["Focus"].Errors.Clear();

      if (ModelState["Active"] != null)
        ModelState["Active"].Errors.Clear();
      if (ModelState["StartMonth"] != null)
      {
        ModelState["StartMonth"].Errors.Clear();
        indicator.StartMonth = GetCurrentDate(formCollection["StartMonth"]);
      }
      if (ModelState["EndMonth"] != null)
      {
        ModelState["EndMonth"].Errors.Clear();
        indicator.EndMonth = GetCurrentDate(formCollection["EndMonth"]);
      }
      if (ModelState.IsValid)
      {
        if (indicator.ID <= 0)
        {
          indicator.OwnerID = LoginToken.LoginUser.ID;
          indicator.CreateDate = DateTime.Now;
          DatabaseObject.db.Indicators.Add(indicator);
          DatabaseObject.db.SaveChanges();
          XmlHelper.InsertXmlValue(indicator.IndicatorData("اضافة", "Add"), Server.MapPath("~/App_Data/Log/ITS_LOG_Indicator.xml"), "Operation");
          bool result = CrawlerHelper.CreateXML(indicator.ID, indicator.IndexUrl, indicator.Abbreviation);

          if (Request["Form"] == Resources.ResourcesFiles.MapLabel.SaveAdd)
            return RedirectToAction("Add");
          else if (Request["Form"] == Resources.ResourcesFiles.MapLabel.Save)
            return RedirectToAction(Functions.FunctionName_Browse, new { times = 1 });
        }
        else
        {
          Indicator i = DatabaseObject.db.Indicators.Find(indicator.ID);
          i.ModifierID = LoginToken.LoginUser.ID;
          i.ModifyDate = DateTime.Now;
          i.Name_A = indicator.Name_A;
          i.Name_E = indicator.Name_E;
          i.ReportName_A = indicator.ReportName_A;
          i.ReportName_E = indicator.ReportName_E;
          i.Description_A = indicator.Description_A;
          i.Description_E = indicator.Description_E;
          i.Notes_A = indicator.Notes_A;
          i.Notes_E = indicator.Notes_E;
          i.WebsiteUrl = indicator.WebsiteUrl;
          i.IndexUrl = indicator.IndexUrl;
          i.Abbreviation = indicator.Abbreviation;
          i.StartMonth = indicator.StartMonth;
          i.EndMonth = indicator.EndMonth;
          i.PublisherID = indicator.PublisherID;
          i.PeriodicityID = indicator.PeriodicityID;
          i.SortIndex = indicator.SortIndex;
          i.Focus = indicator.Focus;
          i.Active = indicator.Active;
          DatabaseObject.db.Entry(i).State = EntityState.Modified;
          DatabaseObject.db.SaveChanges();
          XmlHelper.InsertXmlValue(i.IndicatorData("تعديل", "Edit"), Server.MapPath("~/App_Data/Log/ITS_LOG_Indicator.xml"), "Operation");
          if (Request["Form"] == Resources.ResourcesFiles.MapLabel.SaveEdit)
            return RedirectToAction("Edit", new { ID = indicator.ID });
          else if (Request["Form"] == Resources.ResourcesFiles.MapLabel.Save)
            return RedirectToAction(Functions.FunctionName_Browse, new { times = 1 });
        }
      }
      return View(indicator);
    }

    // GET Delete return json //
    public JsonResult CheckDeletePermission(int? id)
    {
      bool ret = true;
      if (id == null)
        ret = false;
      Indicator indicator = DatabaseObject.db.Indicators.Find(id);
      if (indicator == null)
        ret = false;
      if (!PermissionHelper.IsPermitted(new FunctionSecurityContext(Functions.FunctionName_Delete, "Indicator", indicator.ID, (int)indicator.OwnerID)))
        ret = false;
      return Json(ret, JsonRequestBehavior.AllowGet);
    }
    
    public JsonResult Delete(int id)
    {
       // Get Indicator with Find and FirstorDefault //
      Indicator indicator = DatabaseObject.db.Indicators.Find(id);
      XmlHelper.InsertXmlValue(indicator.IndicatorData("حذف", "Delete"), Server.MapPath("~/App_Data/Log/ITS_LOG_Indicator.xml"), "Operation");
       // Remove Indicator //
      DatabaseObject.db.Indicators.Remove(indicator);
        // kda delete from database //
      DatabaseObject.db.SaveChanges();
       
       //  3yza e list of indicators m3ha el session mn objectlist //
      List<Indicator> indicators = (Session["ObjectList"]) as List<Indicator>;
            // delete from list //
      indicators.Remove(indicator);
      Session["ObjectList"] = indicators;
      return Json("OK", JsonRequestBehavior.AllowGet);
    }

    public ActionResult XMLOperation(int? id)
    {
      Indicator indicator = DatabaseObject.db.Indicators.Find(id);
      return View(indicator);
    }

    [HttpPost]
    public ActionResult XMLOperation(FormCollection formCollection , HttpPostedFileBase file)
    {
      string Result = "";
      Indicator indicator = DatabaseObject.db.Indicators.Find(ConvertHelper.Convert<int>(formCollection["ID"],0));
      if (file != null)
      {
        string path = Server.MapPath(string.Format("~/Uploads/SiteSettings/{0}_{1}", indicator.Abbreviation, indicator.ID.ToString()));
        //file.SaveAs(path);
        try
        {
          XDocument document = XDocument.Load(new System.IO.StreamReader(file.InputStream));
          document.Save(path);
          bool result = CrawlerHelper.CreateXML(indicator.ID, indicator.IndexUrl, indicator.Abbreviation, path);
          if (result)
            Result = ITS.Resources.ResourcesFiles.MapLabel.FileError;
        }
        catch(Exception e)
        {
          Result = ITS.Resources.ResourcesFiles.MapLabel.FileError;
        }
       
      }
      ViewBag.FileMsg = Result;
      return View(indicator);

    }

    public FileResult ShowFile(int id)
    {
      byte[] EmptyFile = new byte [1000];
      try
      {
        Indicator indicator = DatabaseObject.db.Indicators.Find(id);
        string path = "~/Uploads/Indicator/" + indicator.Abbreviation + "_" + indicator.ID;
        string FileURL = Server.MapPath(path);
        byte[] FileBytes = System.IO.File.ReadAllBytes(FileURL);
        return File(FileBytes, "application/xml");
      }
      catch
      {
        return File(EmptyFile, "application/xml");
      }
      
      
    }

    private void IntialSearchList()
    {
      List<ListItemObject> Attr = new List<ListItemObject>
      {
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.ID, value = "ID" },
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.Name, value = "Name" },
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.StartDate, value = "StartDate" },
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.EndDate, value = "EndDate" },
      };
      ViewBag.AttributesName = new SelectList(Attr, "value", "Text", Session["Attribute"] == null ? "" : Session["Attribute"]);
    }

    /* Download File */ 
    public JsonResult Download(int? id)
    {
      var result = false;

      if (id == null)
      {
        result = false;
        return Json(result, JsonRequestBehavior.AllowGet);
      }

      Indicator indicator = DatabaseObject.db.Indicators.Find(id);

      string address = Server.MapPath(string.Format("~/Uploads/SiteSettings/{0}_{1}", indicator.Abbreviation, indicator.ID.ToString()));
      result = AppHelper.DownloadFile(address, indicator.Name_A);

      return Json(result, JsonRequestBehavior.AllowGet);
    }

    private DateTime GetCurrentDate(string date)
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-UK");
      DateTime currentdate = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
      return currentdate;
    }
  }
}
