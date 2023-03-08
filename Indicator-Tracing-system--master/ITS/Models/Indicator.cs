using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ISDCore;
namespace ITS.Models
{
    //[MetadataType(typeof(IndicatorMetaData))]
    [Table("ITS_Indicator")]
    public class Indicator : ObjectBase
    {
        public Indicator()
        {
            IndicatorAttachments = new HashSet<IndicatorAttachment>();
            Sectors = new HashSet<Sector>();
            Issues = new HashSet<Issue>();
        }

        [Required(ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired"), MaxLength((int)EnumDataTypesLength.ISD_Name)]
        //[RegularExpression("[-_\\sء-ي]+", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisNotValid")]
        public string Name_A { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired"), MaxLength((int)EnumDataTypesLength.ISD_Name)]
        //[RegularExpression("^[a-zA-Z-_\\s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisNotValid")]
        public string Name_E { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired"), MaxLength((int)EnumDataTypesLength.ISD_Name)]
        public string ReportName_A { get; set; }

       
        [MaxLength((int)EnumDataTypesLength.ISD_Name)]
        public string ReportName_E { get; set; }

        [MaxLength((int)EnumDataTypesLength.ISD_Description)]
        public string Description_A { get; set; }

        [MaxLength((int)EnumDataTypesLength.ISD_Description)]
        public string Description_E { get; set; }
        
        [MaxLength((int)EnumDataTypesLength.ISD_Notes)]
        public string Notes_A { get; set; }

        [MaxLength((int)EnumDataTypesLength.ISD_Notes)]
        public string Notes_E { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired"), MaxLength((int)EnumDataTypesLength.ISD_Url)]
        [RegularExpression(@"(http://|https://)?(www\.)?\w+\.(com|net|edu|org)", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "HomePageNotValid")]
        public string WebsiteUrl { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired"), MaxLength((int)EnumDataTypesLength.ISD_Url)]
        [RegularExpression(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "HomePageNotValid")]
        public string IndexUrl { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired"), MaxLength((int)EnumDataTypesLength.ISD_abbreviation), Index(IsUnique = true)]
        public string Abbreviation { get; set; }
        [Column(TypeName = "datetime2")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartMonth { get; set; }
        [Column(TypeName = "datetime2")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndMonth { get; set; }
        public int PublisherID { get; set; }
        public int PeriodicityID { get; set; }

        public virtual User Owner { set; get; }
        public virtual User Modifier { set; get; }
        public virtual Periodicity Periodicity { set; get; }
        public virtual Publisher Publisher { set; get; }

        public virtual ICollection<IndicatorAttachment> IndicatorAttachments { set; get; }
        public virtual ICollection<Sector> Sectors { set; get; }
        public virtual ICollection<Issue> Issues { set; get; }
        public Dictionary<string, string> IndicatorData(string Operation_A, string Operation_E)
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
          Data.Add("ReportName_A", this.ReportName_A);
          Data.Add("ReportName_E", this.ReportName_E);
          Data.Add("Description_A", this.Description_A);
          Data.Add("Description_E", this.Description_E);
          Data.Add("Notes_A", this.Notes_A);
          Data.Add("Notes_E", this.Notes_E);
          Data.Add("WebsiteUrl", this.WebsiteUrl);
          Data.Add("IndexUrl", this.IndexUrl);
          Data.Add("Abbreviation", this.Abbreviation);
          Data.Add("StartMonth", this.StartMonth.ToString());
          Data.Add("EndMonth", this.EndMonth.ToString());
          Data.Add("PublisherID", this.PublisherID.ToString());
          Data.Add("Publisher_A", string.IsNullOrEmpty(this.Publisher.Name_A) ? "" : this.Publisher.Name_A);
          Data.Add("Publisher_E", string.IsNullOrEmpty(this.Publisher.Name_E) ? "" : this.Publisher.Name_E);
          Data.Add("PeriodicityID", this.PeriodicityID.ToString());
          Data.Add("Periodicity_A", string.IsNullOrEmpty(this.Periodicity.Name_A) ? "" : this.Periodicity.Name_A);
          Data.Add("Periodicity_E", string.IsNullOrEmpty(this.Periodicity.Name_E) ? "" : this.Periodicity.Name_E);
          Data.Add("SortIndex", this.SortIndex.ToString());
          Data.Add("Focus", this.Focus.ToString());
          Data.Add("Active", this.Active.ToString());
          return Data;
        }
  }
}