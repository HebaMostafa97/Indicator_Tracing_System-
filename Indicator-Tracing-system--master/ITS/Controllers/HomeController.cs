using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITS.Models;
using ISDCore;
using System.Resources;
using System.Reflection;
namespace ITS.Controllers
{
  public class HomeController : BasicController
  {
    public ActionResult Index()
    {
      Session["ErrorLogInMsg"] = "";
      Language.UserLanguage = string.IsNullOrEmpty(Request.Cookies["Language"].Value)? "en": Request.Cookies["Language"].Value;
      return RedirectToAction("LogIN", "LogIn");
      //User u = new User
      //{
      //  Active = 1,
      //  Admin = 1,
      //  Focus = 1,
      //  SortIndex = 0,
      //  Email = "Mohamed.fouad208@gmail.com",
      //  Username = "Admin",
      //  Password = ConvertHelper.EncryptValue("Admin"),
      //  CreateDate = DateTime.Now,
      //  Name = "Mohamed",
      //  NameNormalized = "Mohamed",
      //  OwnerID = 1
      //};
      //db.Users.Add(u);
      //db.SaveChanges();
    }
    public ActionResult CahngeLanguage(string LanguageAbbreviation)
    {
      HttpCookie cookie = Language.Change(LanguageAbbreviation);
      Response.Cookies.Add(cookie);
      return RedirectToAction("LogIN", "LogIn");
    }
    public ActionResult AccessDenied()
    {
      return View();
    }
    [AuthorizeUser]
    public ActionResult Control()
    {
      return View();
    }
  }
}