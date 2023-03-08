using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Resources;
using System.Reflection;
using ISDCore;
using ITS.Models;
using System.Xml;

namespace ITS.Controllers
{
  public class LogInController : BasicController
  {
    // GET: LogIn
    public ActionResult LogIN()
    {
      return View();
    }
    [HttpPost]
    public ActionResult LogIN(LogInModel logInModel)
    {
      if (ModelState.IsValid)
      {
        var user = DatabaseObject.db.Users.Where(u => u.Username == logInModel.username).FirstOrDefault();
        var status = LogInHelper.LogIn(user, logInModel);
        if (status == EnumLoginStatus.Success)
        {
          DatabaseObject.db.Entry((User)LoginToken.LoginUser).State = System.Data.Entity.EntityState.Modified;
          DatabaseObject.db.SaveChanges();
          ResourceManager RM = new ResourceManager("ITS.Resources.ResourcesFiles.MapLabel", Assembly.GetExecutingAssembly());
          ISDCore.Resources.MapLabels(ref RM);
          return RedirectToAction(LogInHelper.SuccessAction, LogInHelper.SuccessController);
        }
        else if (status == EnumLoginStatus.AccountBlocked)
        {
          Session["ErrorLogInMsg"] = Resources.ResourcesFiles.MapLabel.BlockedAccount;
          return RedirectToAction(LogInHelper.FailAction, LogInHelper.FailController);
        }
        else if (status == EnumLoginStatus.InvalidAccount)
        {
          Session["ErrorLogInMsg"] = Resources.ResourcesFiles.MapLabel.InvalidPassword;
          return RedirectToAction(LogInHelper.FailAction, LogInHelper.FailController);
        }
        else if (status == EnumLoginStatus.UserNotFound)
        {
          Session["ErrorLogInMsg"] = Resources.ResourcesFiles.MapLabel.Invalidusername;
          return RedirectToAction(LogInHelper.FailAction, LogInHelper.FailController);
        }
      }
      return RedirectToAction(Settings.HomeAction, Settings.HomeController);
    }
    public ActionResult LogOut()
    {
      LogInHelper.LogOut();
      return RedirectToAction(LogInHelper.LogoutAction, LogInHelper.LogoutController);
    }
    public ActionResult ForgetPassword()
    {
      return View();
    }
    [HttpPost]
    public ActionResult ForgetPassword(string Email)
    {
      var user = DatabaseObject.db.Users.Where(u => u.Email == Email).FirstOrDefault();
      var result = LogInHelper.ForgetPassword(user);
      if (result)
      {
        ViewBag.Result = "Password Sent Correctly";
      }
      else
      {
        ViewBag.Result = "Something Went Wrong Please try Again";
      }
      return View();
    }
  }
}