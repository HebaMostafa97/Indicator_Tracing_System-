using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITS.Models;
using ISDCore;
using System.Data;
using System.Net;
using System.Data.Entity.Validation;
using System.Runtime.CompilerServices;
using System.Data.Entity;
using System.Web.Security;

namespace ITS.Controllers
{
  [AuthorizeUser]
  public class UserController : BasicController
  {
    [ISDPageSecured]
    // get 
    public ActionResult Browse(int times = 0)
    {
       // NumberOfPages de selectlist mn pagingList feh el value w text //
       // Default beta3 Session["Paging"] b null equal 5 else tostring //
       // (2) //
      ViewBag.NumberOfPages = new SelectList(ISDCore.Resources.pagingList(), "value", "Text", Session["Paging"] == null ? "5" : Session["Paging"].ToString());
      ViewBag.Title = ISDCore.Language.MapValue(ITS.Resources.ResourcesFiles.MapLabel.Users + " " + ITS.Resources.ResourcesFiles.MapLabel.List,
                                                ITS.Resources.ResourcesFiles.MapLabel.List + " " + ITS.Resources.ResourcesFiles.MapLabel.Users);
       // Paging lw b null yb2a b 5 (1)//
      if (Session["Paging"] == null)
        Session["Paging"] = 5;
      if (times == 1)
      {
        Session["SearchValue"] = "";
        Session["Attribute"] = "";
        Session["ObjectList"] = Filtter.FiltterUser();
      }
      else
      {
        List<User> users = Session["ObjectList"] as List<User>;
        if (users == null)
        {
          Session["ObjectList"] = Filtter.FiltterUser();
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
      ViewBag.Title = ISDCore.Language.MapValue(ITS.Resources.ResourcesFiles.MapLabel.Users + " " + ITS.Resources.ResourcesFiles.MapLabel.List,
                                               ITS.Resources.ResourcesFiles.MapLabel.List + " " + ITS.Resources.ResourcesFiles.MapLabel.Users);
      List<User> Users = Filtter.FiltterUser();
      List<User> Result = new List<User>();
       // lw NumOfPages not equal null //
      if (NumOfPages != null)
      {
        // Session["Paging"] elly kan = 5 fe browse get //
        // hna ha5leh equal NumOfPages //
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
        Result = Users.Where(u => u.ID == id).ToList();
      }
      else if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(value) && (attr == "Name"))
      {
        Result = Users.Where(u => u.Name.ToLower().Contains(value.ToLower())).ToList();
      }
      else if (!string.IsNullOrEmpty(attr) && !string.IsNullOrEmpty(value) && (attr == "Email"))
      {
        Result = Users.Where(u => u.Email.ToLower().Contains(value.ToLower())).ToList();
      }
      else
      {
        Result = Users;
      }
      Session["ObjectList"] = Result;
      return View(Result);
    }

    public JsonResult Active(int? ID)
    {
      User user = DatabaseObject.db.Users.Find(ID);
      if (user.Active == 1)
        user.Active = 0;
      else
        user.Active = 1;
      DatabaseObject.db.Entry(user).State = EntityState.Modified;
      DatabaseObject.db.SaveChanges();
      return Json("", JsonRequestBehavior.AllowGet);
    }

    public ActionResult Preview(int? id)
    {
       ViewBag.PreviewTitle = @ITS.Resources.ResourcesFiles.MapLabel.PreviewTitle;
       ViewBag.GroupOf = ITS.Resources.ResourcesFiles.MapLabel.Groupof;
       if (id == null)
       {
           return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
       }
         User user = DatabaseObject.db.Users.Find(id);
      if (user == null)
      {
            return HttpNotFound();
      }
          return View(user);
    }
        //get 
    public ActionResult Add()
    {
      ViewBag.Groups = new SelectList(DatabaseObject.db.Groups, "ID", "Name");
      PermissionHelper.VerifyPermission(new FunctionSecurityContext(Functions.FunctionName_Add, "User", 0));
      return View("Form", new User());
    }

        //get
    public ActionResult Edit(int? ID)
    {
      ViewBag.Groups = new SelectList(DatabaseObject.db.Groups, "ID", "Name");
      if (ID == null)
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      User user = DatabaseObject.db.Users.Find(ID);
      if (user == null)
        return HttpNotFound();
      PermissionHelper.VerifyPermission(new FunctionSecurityContext(Functions.FunctionName_Edit, "User", user.ID, (int)user.OwnerID));
      return View("Form", user);
    }

    public ActionResult ResetEdit(int ID)
    {
      User user = new User() { ID = ID };
      return View("Form", user);
    }
    // Exclude passsword/temppassword //
    // 3ndy object mn user 
    [HttpPost]
    public ActionResult Form([Bind(Exclude = "Password,TempPassword")]User U, FormCollection formCollection)
    {
      bool ErrorInModel = false;
        // dropdownlist from groups //
      ViewBag.Groups = new SelectList(DatabaseObject.db.Groups, "ID", "Name");
        // Active &Admin &Focus with formcollecction 3ashan hena b int(checkbox) not bool //
      U.Active = formCollection["active"] == "on" ? 1 : 0;
      U.Admin = formCollection["admin"] == "on" ? 1 : 0;
      U.Focus = formCollection["focus"] == "on" ? 1 : 0;

        //Name hwa nfs el NameNormalized //
      U.NameNormalized = U.Name;
        // sortindex b int brdo bs text and using ConvertHelper .Convert//
      U.SortIndex = ConvertHelper.Convert<int>(formCollection["SortIndex"], 0);
      if (!string.IsNullOrEmpty(formCollection["pass"]))
      {
        U.Password = ConvertHelper.EncryptValue(formCollection["pass"]);
        U.TempPassword = ConvertHelper.EncryptValue(formCollection["pass"]);
        ModelState["Password"].Errors.Clear();
        if (!Functions.ValidatePassword(formCollection["pass"]))
        {
          ModelState.AddModelError("Password", Resources.ResourcesFiles.MapLabel.PasswordValidation);
          ViewBag.UserPassword = formCollection["pass"];
        }
      }
      else
      {
        ModelState["Password"].Errors.Clear();
        if(U.ID<=0)
        {
          string password = Membership.GeneratePassword(8, 4);
          U.Password = ConvertHelper.EncryptValue(password);
          U.TempPassword = ConvertHelper.EncryptValue(password);
        }
        
      }
      if (ModelState["Groups"] != null)
        ModelState["Groups"].Errors.Clear();
      if (!string.IsNullOrEmpty(U.NameNormalized))
        ModelState["NameNormalized"].Errors.Clear();
      if (ModelState["Admin"] != null)
        ModelState["Admin"].Errors.Clear();
      if (ModelState["Focus"] != null)
        ModelState["Focus"].Errors.Clear();
      if (ModelState["Active"] != null)
        ModelState["Active"].Errors.Clear();
      if (ModelState.IsValid)
      {
        if (U.ID <= 0)
        {
            // fe el Add el Username yb2a Unique y3ne mesh bytkrr //
            // Validation with serverside //
          if (IsUserNameExists(U.Username))
          {
            ModelState.AddModelError("Username", Resources.ResourcesFiles.MapLabel.Exist);
            ErrorInModel = true;
          }
          if (IsUserEmailExists(U.Email))
          {
            ModelState.AddModelError("Email", Resources.ResourcesFiles.MapLabel.EmailExist);
            ErrorInModel = true;
          }
          if (ErrorInModel)
            return View(U);
          if (!string.IsNullOrEmpty(formCollection["Groups"]))
          {
            U.Groups.Add(DatabaseObject.db.Groups.Find(Convert.ToInt32(formCollection["Groups"])));
          }
          U.OwnerID = LoginToken.LoginUser.ID;
          U.CreateDate = DateTime.Now;
          DatabaseObject.db.Users.Add(U);
          DatabaseObject.db.SaveChanges();
          XmlHelper.InsertXmlValue(U.UserData("اضافة","Add"), Server.MapPath("~/App_Data/Log/ITS_LOG_User.xml"), "Operation");
          if (Request["Form"] == Resources.ResourcesFiles.MapLabel.SaveAdd)
            return RedirectToAction("Add");
          else if(Request["Form"] == Resources.ResourcesFiles.MapLabel.Save)
            return RedirectToAction(Functions.FunctionName_Browse, new { times = 1 });
        }
        else
        {
          // Edit User //
          //Find -- FirstOrDefault //
          User user = DatabaseObject.db.Users.Find(U.ID);
           
           // Edit user with validation Unique UseerName //
          if (U.Username != user.Username && IsUserNameExists(U.Username))
          {
            ModelState.AddModelError("Username", Resources.ResourcesFiles.MapLabel.Exist);
            ErrorInModel = true;
          }
          if (U.Email != user.Email && IsUserEmailExists(U.Email))
          {
            ModelState.AddModelError("Email", Resources.ResourcesFiles.MapLabel.EmailExist);
            ErrorInModel = true;
          }
          if (ErrorInModel)
            return View(U);
          if (user.Username!=U.Username && IsUserNameExists(U.Username))
          {
            ModelState.AddModelError("Username", Resources.ResourcesFiles.MapLabel.Exist);
            ErrorInModel = true;
          }
          if (user.Email != U.Email && IsUserEmailExists(U.Email))
          {
            ModelState.AddModelError("Email", Resources.ResourcesFiles.MapLabel.EmailExist);
            ErrorInModel = true;
          }
          if (ErrorInModel)
            return View(U);
          user.Name = U.Name;
          user.NameNormalized = U.Name;
          user.Admin = U.Admin;
          user.Focus = U.Focus;
          user.Active = U.Active;
          user.Email = U.Email;
          user.HomePage = U.HomePage;
          user.Username = U.Username;
          user.JobTitle = U.JobTitle;
          user.Notes = U.Notes;
          user.SortIndex = U.SortIndex;

          if (!string.IsNullOrEmpty(formCollection["pass"]))
          {
            user.Password = U.Password;
            user.TempPassword = U.Password;
          }
          user.ModifierID = LoginToken.LoginUser.ID;
          user.ModifyDate = DateTime.Now;
          DatabaseObject.db.Entry(user).State = EntityState.Modified;
          DatabaseObject.db.SaveChanges();
          XmlHelper.InsertXmlValue(user.UserData("تعديل", "Edit"), Server.MapPath("~/App_Data/Log/ITS_LOG_User.xml"), "Operation");
          if (Request["Form"] == Resources.ResourcesFiles.MapLabel.SaveEdit)
            return RedirectToAction("Edit",new { ID= user.ID});
          else if (Request["Form"] == Resources.ResourcesFiles.MapLabel.Save)
            return RedirectToAction(Functions.FunctionName_Browse, "User");
        }
      }
      return View(U);
    }

    public JsonResult CheckDeletePermission(int? id)
    {
      bool ret = true;
      if (id == null)
        ret = false;
      User user = DatabaseObject.db.Users.Find(id);
      if (user == null)
        ret = false;
      if (!PermissionHelper.IsPermitted(new FunctionSecurityContext(Functions.FunctionName_Delete, "User", user.ID, (int)user.OwnerID)))
        ret = false;
      return Json(ret, JsonRequestBehavior.AllowGet);
    }

    public JsonResult Delete(int id)
    {
      User user = DatabaseObject.db.Users.Find(id);
      if(user.CheckReference())
      {
        return Json(Resources.ResourcesFiles.MapLabel.UserError, JsonRequestBehavior.AllowGet);
      }
      XmlHelper.InsertXmlValue(user.UserData("حذف", "Delete"), Server.MapPath("~/App_Data/Log/ITS_LOG_User.xml"), "Operation");
      DatabaseObject.db.Users.Remove(user);
      DatabaseObject.db.SaveChanges();
            // List of users after Fillter //
      List<User> users = (Session["ObjectList"]) as List<User>;
      users.Remove(user);
      Session["ObjectList"] = users;
      return Json("OK", JsonRequestBehavior.AllowGet);
    }
    // Get LinkGroup take parameter ID //
    // Return View list from groups //
    public ActionResult LinkGroup(int ID)
    {
      // Find -->FirstOrDefault //
      var user = DatabaseObject.db.Users.Find(ID);
      Session["UserToGroups"] = user;
      PermissionHelper.VerifyPermission(new FunctionSecurityContext(Functions.FunctionName_LinkGroup, "User", ID, (int)user.OwnerID));
      return View(DatabaseObject.db.Groups.ToList());

    }
    [HttpPost]
    public ActionResult LinkGroup(int[] Items)
    {
      var user = ConvertHelper.Convert<User>(Session["UserToGroups"], null);
      var uSer = DatabaseObject.db.Users.Find(user.ID);
      if (Items != null)
      {
                // to Add group //
        foreach (var id in Items)
        {
          if (!uSer.Groups.Where(g => g.ID == id).Any())
          {
            var group = DatabaseObject.db.Groups.Find(id);
            uSer.Groups.Add(group);
          }
        }
        // to cancel group //
        foreach (var item in uSer.Groups.ToList())
        {
          if (!Items.Where(i => i == item.ID).Any())
          {
            var group = DatabaseObject.db.Groups.Find(item.ID);
            uSer.Groups.Remove(group);
          }
        }
      }
      else
      {
        foreach (var item in uSer.Groups.ToList())
        {
          var group = DatabaseObject.db.Groups.Find(item.ID);
          uSer.Groups.Remove(group);
        }

      }
      DatabaseObject.db.Entry(uSer).State = EntityState.Modified;
      DatabaseObject.db.SaveChanges();
      return RedirectToAction("Browse");
    }

    // Function IsUserNameExists take username to validate //

    public bool IsUserNameExists(string Username)
    {
      return (DatabaseObject.db.Users.Any(x => x.Username == Username));
    }

    public bool IsUserEmailExists(string Email)
    {
      return (DatabaseObject.db.Users.Any(x => x.Email == Email));
    }
    private void IntialSearchList()
    {
      List<ListItemObject> Attr = new List<ListItemObject>
      {
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.ID, value = "ID" },
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.Name, value = "Name" },
        new ListItemObject { Text = Resources.ResourcesFiles.MapLabel.Email, value="Email"}
      };
      ViewBag.AttributesName = new SelectList(Attr, "value", "Text", Session["Attribute"] == null ? "" : Session["Attribute"]);
    }

  }
}