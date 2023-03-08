using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ISDCore;

namespace ITS.Models
{
    [MetadataType(typeof(GroupPermissionMetaData))]
    [Table("ITS_GroupPermission")]
    public class GroupPermission
    {
        public GroupPermission()
        {

        }
        //[Key, Column(Order = 0)]
        //public int GroupID { get; set; }
        //[Key, Column(Order = 1)]
        //public int PermissionID { get; set; }
        //public string Objectname { get; set; }

        public int ID { get; set; }
        public int? OwnerID { get; set; }
        public int? ModifierID { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreateDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ModifyDate { get; set; }
        [Required, MaxLength((int)EnumDataTypesLength.ISD_Name)]
        public string FunctionName { get; set; }
        public int GroupID { set; get; }
        public int Permission { get; set; }
        public int Restricted { get; set; }
        [Required, MaxLength((int)EnumDataTypesLength.ISD_ObjectName)]
        public string Objectname { set; get; }
        public virtual Group Group { set; get; }
        public int SortIndex { get; set; }
        public int Focus { get; set; }
        public int Active { get; set; }

    }
    class GroupPermissionMetaData
    {
        [RegularExpression("^[\u0621-\u064A\u0660-\u0669a-zA-Z-_\\s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
        public string FunctionName { get; set; }
        [RegularExpression("^[\u0621-\u064A\u0660-\u0669a-zA-Z-_\\s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
        public string Objectname { set; get; }
    }

}