using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ISDCore;

namespace ITS.Models
{
  [Table("ITS_Publisher")]
  public class Publisher :ObjectBase
  {
    public Publisher()
    {
        Indicators = new HashSet<Indicator>();
    }
    [Required(ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired"), MaxLength((int)EnumDataTypesLength.ISD_Name)]
    public string Name_A { get; set; }

    [Required(ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired"),MaxLength((int)EnumDataTypesLength.ISD_Name),RegularExpression("^[a-zA-Z-_\\s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    public string Name_E { get; set; }
    [MaxLength((int)EnumDataTypesLength.ISD_Description),RegularExpression("[-_\\sء-ي]+", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    public string Description_A { get; set; }
    [MaxLength((int)EnumDataTypesLength.ISD_Description),RegularExpression("^[a-zA-Z-_\\s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    public string Description_E { get; set; }
    [MaxLength((int)EnumDataTypesLength.ISD_Url)]
    public string HomePage { get; set; }
    public int CountryID { get; set; }

    public virtual User Owner { set; get; }
    public virtual User Modifier { set; get; }
    public virtual Country Country { set; get; }

    public virtual ICollection<Indicator> Indicators { get; set; }
    public Dictionary<string, string> PublisherData(string Operation_A, string Operation_E)
    {
      Dictionary<string, string> Data = new Dictionary<string, string>();
      Data.Add("ID", this.ID.ToString());
      Data.Add("OwnerID", LoginToken.LoginUser.ID.ToString());
      Data.Add("Owner", LoginToken.LoginUser.Name);
      Data.Add("Operation_A", Operation_A);
      Data.Add("Operation_E", Operation_E);
      Data.Add("OperationDate", DateTime.Now.ToString());
      Data.Add("Name_A", this.Name_A);
      Data.Add("Name_E", this.Name_E);
      Data.Add("Description_A", this.Description_A);
      Data.Add("Description_E", this.Description_E);
      Data.Add("HomePage", this.HomePage);
      Data.Add("CountryID", this.CountryID.ToString());
      Data.Add("Country_A", string.IsNullOrEmpty(this.Country.Name_A) ? "" : this.Country.Name_A);
      Data.Add("Country_E", string.IsNullOrEmpty(this.Country.Name_E) ? "" : this.Country.Name_E);
      Data.Add("SortIndex", this.SortIndex.ToString());
      Data.Add("Focus", this.Focus.ToString());
      Data.Add("Active", this.Active.ToString());
      return Data;
    }

  }
        
  

    //class PublisherMetaData
    //{
    //    [RegularExpression("[-_\\sء-ي]+", ErrorMessageResourceType =typeof(Resources.ResourcesFiles.MapLabel),ErrorMessageResourceName ="ThisFieldisRequired")]
    //    public string Name_A { get; set; }
    //    [RegularExpression("^[a-zA-Z-_\\s]+$",ErrorMessageResourceType =typeof(Resources.ResourcesFiles.MapLabel),ErrorMessageResourceName ="ThisFieldisRequired")]
    //    public string Name_E { get; set; }
    //    [RegularExpression("[-_\\sء-ي]+", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    //    public string Description_A { get; set; }
    //    [RegularExpression("^[a-zA-Z-_\\s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    //    public string Description_E { get; set; }
    //    [RegularExpression(@"(http://|https://)?(www\.)?\w+\.(com|net|edu|org)", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "HomePageNotValid")]
    //    public string HomePage { get; set; }

    //}
}