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

namespace ITS.Controllers
{
  [AuthorizeUser]
  public class IndicatorAttachmentController : BasicController
  {
    readonly List<IndicatorAttachment> emptyList = new List<IndicatorAttachment>();
    [ISDPageSecured]
    // GET: IndicatorAttachment
    public ActionResult Browse(int times = 0)
    {
      ViewBag.NumberOfPages = new SelectList(ISDCore.Resources.pagingList(), "value", "Text", Session["Paging"] == null ? "5" : Session["Paging"].ToString());
      ViewBag.Title = ISDCore.Language.MapValue(Resources.ResourcesFiles.MapLabel.IndicatorAttachment + " " + Resources.ResourcesFiles.MapLabel.List,
                                               Resources.ResourcesFiles.MapLabel.List + " " + Resources.ResourcesFiles.MapLabel.IndicatorAttachment);

      if (Session["Paging"] == null)
        Session["Paging"] = 5;
      if (times == 1)
      {
        Session["Attribute"] = "";
        Session["SearchValue"] = "";
        Session["FromDate"] = "";
        Session["ToDate"] = "";
        Session["ObjectList"] = Filtter.FiltterIndicatorAttachment();
      }
      else
      {
        List<IndicatorAttachment> attachments = Session["ObjectList"] as List<IndicatorAttachment>;
        if (attachments == null)
        {
          Session["ObjectList"] =  Filtter.FiltterIndicatorAttachment();
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
    public ActionResult Browse(string attr,string value, string FromDate, string ToDate, int? NumOfPages)
    {
      IntialSearchList();
      ViewBag.NumberOfPages = new SelectList(ISDCore.Resources.pagingList(), "value", "Text", NumOfPages != null ? NumOfPages.ToString() : Session["Paging"].ToString());
      ViewBag.Title = ISDCore.Language.MapValue(Resources.ResourcesFiles.MapLabel.IndicatorAttachment + " " + Resources.ResourcesFiles.MapLabel.List,
                                               Resources.ResourcesFiles.MapLabel.List + " " + Resources.ResourcesFiles.MapLabel.IndicatorAttachment);

      List<IndicatorAttachment> indicatorsAttachment = Filtter.FiltterIndicatorAttachment();
      List<IndicatorAttachment> Result = new List<IndicatorAttachment>();
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

      if((attr == "CreateDate" || attr == "ReleaseDate" || attr == "UploadDate") && FromDate != null && ToDate != null )
      {
        Session["FromDate"] = FromDate;
        Session["ToDate"] = ToDate;
      }
      if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(value) && (attr == "ID"))
      {
          try
          {
              int id = ConvertHelper.Convert<int>(value, 0);
              Result = indicatorsAttachment.Where(u => u.IndicatorID == id).ToList();
          }
          catch (Exception)
          {
              Result = emptyList;
          }
                
      }
      else if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(value) && (attr == "Name"))
      {
          try
          {
              int indicatorID = DatabaseObject.db.Indicators.Where(u => (Language.UserLanguage == "en" && !string.IsNullOrEmpty(u.Name_E) && u.Name_E.ToLower() == value) ||
              Language.UserLanguage == "Ar" && !string.IsNullOrEmpty(u.Name_A) && u.Name_A.ToLower() == value).FirstOrDefault().ID;

              Result = indicatorsAttachment.Where(u => u.IndicatorID == indicatorID).ToList();
          }
          catch (Exception)
          {
              Result = emptyList;
          }
                
      }
      else if(!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate) && (attr == "CreateDate"))
      {
        try
        {
          var From = GetCurrentDate(FromDate);
          var To = GetCurrentDate(ToDate);
          Result = indicatorsAttachment.Where(u => (u.CreateDate.Ticks >= From.Ticks && u.CreateDate.Ticks <= To.Ticks)).ToList();
        }
        catch
        {

        }
        
      }
      else if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate) && (attr == "ReleaseDate"))
      {
        try
        {
          var From = GetCurrentDate(FromDate);
          var To = GetCurrentDate(ToDate);
          Result = indicatorsAttachment.Where(u => (u.ReleaseDate.Ticks >= From.Ticks && u.ReleaseDate.Ticks <= To.Ticks)).ToList();
        }
        catch
        {

        }

      }
      else if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate) && (attr == "UploadDate"))
      {
        try
        {
          var From = GetCurrentDate(FromDate);
          var To = GetCurrentDate(ToDate);
          Result = indicatorsAttachment.Where(u => (u.UploadDate.Ticks >= From.Ticks && u.UploadDate.Ticks <= To.Ticks)).ToList();
        }
        catch
        {

        }

      }
      else
      {
          Result = emptyList;
      }
      Session["ObjectList"] = Result;
      return View(Result);

    }
    private void IntialSearchList()
    {
      List<ListItemObject> Attr = new List<ListItemObject>
      {
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.IndicatorID, value = "ID" },
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.IndicatorName, value = "Name" },
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.CreateDate, value = "CreateDate"},
        new ListItemObject {Text = Resources.ResourcesFiles.MapLabel.ReleaseDate, value = "ReleaseDate"},
        new ListItemObject {Text = Resources.ResourcesFiles.MapLabel.UploadDate, value = "UploadDate"}
      };
      ViewBag.AttributesName = new SelectList(Attr, "value", "Text", Session["Attribute"] == null ? "" : Session["Attribute"]);
    }
    public JsonResult Download(int? id)
    {
        var result = false;

        if (id == null)
        {
            result = false;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        IndicatorAttachment attachment = DatabaseObject.db.IndicatorAttachments.Find(id);

        result = AppHelper.DownloadFile(attachment.AttachmentUrl, attachment.Attachment);

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