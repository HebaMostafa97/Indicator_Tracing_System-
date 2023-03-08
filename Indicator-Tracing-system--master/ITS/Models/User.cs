using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ISDCore;
using System.Web.Mvc;

namespace ITS.Models
{
    [MetadataType(typeof(UserMetaData))]  
    [Table("ITS_User")]
    public partial class User : UserObject
    {
    public User()
    {
      Owners = new HashSet<User>();
      Modifiers = new HashSet<User>();

      GroupsOwner = new HashSet<Group>();
      GroupsModifier = new HashSet<Group>();
      Groups = new HashSet<Group>();
      IndicatorsOwner = new HashSet<Indicator>();
      IndicatorsModifier = new HashSet<Indicator>();
      IndicatorAttachmentsOwner = new HashSet<IndicatorAttachment>();
      IndicatorAttachmentsModifier = new HashSet<IndicatorAttachment>();
      PublishersOwner = new HashSet<Publisher>();
      PublishersModifier = new HashSet<Publisher>();

      CountriesOwner = new HashSet<Country>();
      CountriesModifier = new HashSet<Country>();
      PeriodicitiesOwner = new HashSet<Periodicity>();
      PeriodicitiesModifier = new HashSet<Periodicity>();
      IssuesOwner = new HashSet<Issue>();
      IssuesModifier = new HashSet<Issue>();
      SectorsOwner = new HashSet<Sector>();
      SectorsModifier = new HashSet<Sector>();
    }
        
    public virtual User Owner { set; get; }
    public virtual User Modifier { set; get; }

    public virtual ICollection<User> Owners { set; get; }
    public virtual ICollection<User> Modifiers { set; get; }

    //Objects
    public virtual ICollection<Group> GroupsOwner { set; get; }
    public virtual ICollection<Group> GroupsModifier { set; get; }
    public virtual ICollection<Group> Groups { set; get; }
    public virtual ICollection<Indicator> IndicatorsOwner { set; get; }
    public virtual ICollection<Indicator> IndicatorsModifier { set; get; }
    public virtual ICollection<IndicatorAttachment> IndicatorAttachmentsOwner { set; get; }
    public virtual ICollection<IndicatorAttachment> IndicatorAttachmentsModifier { set; get; }
    public virtual ICollection<Publisher> PublishersOwner { set; get; }
    public virtual ICollection<Publisher> PublishersModifier { set; get; }

    //Lookups
    public virtual ICollection<Country> CountriesOwner { set; get; }
    public virtual ICollection<Country> CountriesModifier { set; get; }
    public virtual ICollection<Periodicity> PeriodicitiesOwner { set; get; }
    public virtual ICollection<Periodicity> PeriodicitiesModifier { set; get; }
    public virtual ICollection<Issue> IssuesOwner { set; get; }
    public virtual ICollection<Issue> IssuesModifier { set; get; }
    public virtual ICollection<Sector> SectorsOwner { set; get; }
    public virtual ICollection<Sector> SectorsModifier { set; get; }
    public Dictionary<string, string> UserData(string Operation_A,string Operation_E)
    {
      Dictionary<string, string> Data = new Dictionary<string, string>();
      Data.Add("ID", this.ID.ToString());
      Data.Add("OwnerID", LoginToken.LoginUser.ID.ToString());
      Data.Add("Owner", LoginToken.LoginUser.Name);
      Data.Add("Operation_A", Operation_A);
      Data.Add("Operation_E", Operation_E);
      Data.Add("OperationDate", DateTime.Now.ToString());
      Data.Add("Name", this.Name);
      Data.Add("NameNormalized", this.NameNormalized);
      Data.Add("JobTitle", this.JobTitle);
      Data.Add("Notes", this.Notes);
      Data.Add("Email", this.Email);
      Data.Add("HomePage", this.HomePage);
      Data.Add("Photo", this.Photo);
      Data.Add("Username", this.Username);
      Data.Add("Password", this.Password.ToString());
      Data.Add("TempPassword", this.TempPassword.ToString());
      Data.Add("LastLogonDate", this.LastLogonDate.ToString());
      Data.Add("LastLogoutDate", this.LastLogoutDate.ToString());
      Data.Add("LogonCount", this.LogInCount.ToString());
      Data.Add("Admin", this.Admin.ToString());
      Data.Add("SortIndex", this.SortIndex.ToString());
      Data.Add("Focus", this.Focus.ToString());
      Data.Add("Active", this.Active.ToString());
      return Data;
    }
    public bool CheckReference()
    {
      bool ret = false;
      if(this.Owners.Count()>0  || this.IndicatorsOwner.Count()>0 || this.PublishersOwner.Count>0 || this.GroupsOwner.Count()>0 || this.CountriesOwner.Count()>0
        || this.SectorsOwner.Count()>0 || this.IndicatorAttachmentsOwner.Count()>0 || this.IssuesOwner.Count()>0 || this.PeriodicitiesOwner.Count()>0)
      {
        ret = true;
      }
      return ret;

    }
  }
    class UserMetaData
    {


      [Required(ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired"), RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "EmailNotValid")]
      public string Email { get; set; }
      [RegularExpression(@"(http://|https://)?(www\.)?\w+\.(com|net|edu|org)", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "HomePageNotValid")]
      public string HomePage { get; set; }
      [Required(ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
      public string Name { get; set; }
      [Required(ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
      public string Username { get; set; }
    //[RegularExpression("^[\u0621-\u064A\u0660-\u0669a-zA-Z-_\\s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    //public string Name { get; set; }
    //[RegularExpression("^[\u0621-\u064A\u0660-\u0669a-zA-Z-_\\s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    //public string JobTitle { get; set; }
    //[RegularExpression("^[\u0621-\u064A\u0660-\u0669a-zA-Z-_\\s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    //public string Notes { get; set; }
    //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "EmailNotValid")]
    //public string Email { get; set; }
    //[RegularExpression(@"(http://|https://)?(www\.)?\w+\.(com|net|edu|org)", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "HomePageNotValid")]
    //public string HomePage { get; set; }
    //[RegularExpression("([^\\s]+(\\.(?i)(jpg|png|gif|bmp))$)",ErrorMessageResourceType =typeof(Resources.ResourcesFiles.MapLabel),ErrorMessageResourceName ="ThisFieldisRequired")]
    //public string Photo { get; set; }
    //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    //public string Username { get; set; }
    //[RegularExpression(@"\d{2, 2}/\d{2,2}/\d{4,4} \d{2,2}:\d{2,2}:\d{2,2}",ErrorMessageResourceType =typeof(Resources.ResourcesFiles.MapLabel),ErrorMessageResourceName ="ThisFieldisRequired")]
    //public DateTime? LastLogonDate { get; set; }
    //[RegularExpression(@"\d{2, 2}/\d{2,2}/\d{4,4} \d{2,2}:\d{2,2}:\d{2,2}", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    //public DateTime? LastLogoutDate { get; set; }



  }

}