using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ISDCore;
namespace ITS.Models
{
    [Table("ITS_IndicatorAttachment")]
    public class IndicatorAttachment : ObjectBase
    {
        public IndicatorAttachment()
        {

        }
        [Required ,MaxLength((int)EnumDataTypesLength.ISD_Name)]
        public string ReportName { get; set; }
        [Required, MaxLength((int)EnumDataTypesLength.ISD_Attachment)]
        public string Attachment { get; set; }
        [Required ,MaxLength((int)EnumDataTypesLength.ISD_Name)]
        public string AttachmentTitle { get; set; }
        [Required ,MaxLength((int)EnumDataTypesLength.ISD_Url)]
        public string AttachmentUrl { get; set; }
        [Required, MaxLength((int)EnumDataTypesLength.ISD_Name)]
        public string Keyword { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_Description)]
        public string Description { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime UploadDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ReleaseDate { get; set; }
        public int IndicatorID { get; set; }

        public virtual User Owner { set; get; }
        public virtual User Modifier { set; get; }
        public virtual Indicator Indicator { set; get; }

    }

    
}