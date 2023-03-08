using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ISDCore;

namespace ITS.Models
{
    [MetadataType(typeof(GroupMetaData))]
    [Table("ITS_Group")]
    public class Group : GroupObject
    {
        public Group()
        {
            Users = new HashSet<User>();
            Group_Permissions = new HashSet<GroupPermission>();
        }

        public virtual User Owner { set; get; }
        public virtual User Modifier { set; get; }

        public virtual ICollection<User> Users { set; get; }
        public virtual ICollection<GroupPermission> Group_Permissions { get; set; }
        public Dictionary<string, string> GroupData(string Operation_A, string Operation_E)
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
            Data.Add("Description", this.Description);
            Data.Add("Email", this.Email);
            Data.Add("HomePage", this.HomePage);
            Data.Add("SortIndex", this.SortIndex.ToString());
            Data.Add("Focus", this.Focus.ToString());
            Data.Add("Active", this.Active.ToString());
            return Data;
        }
        class GroupMetaData
        {
            [RegularExpression("^[\u0621-\u064A\u0660-\u0669a-zA-Z-_//s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisNotValid")]
            public string Name { get; set; }
            [RegularExpression("^[\u0621-\u064A\u0660-\u0669a-zA-Z-_//s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisNotValid")]
            public string NameNormalized { get; set; }
            [RegularExpression("^[\u0621-\u064A\u0660-\u0669a-zA-Z-_//s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisNotValid")]
            public string Description { get; set; }
            [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "EmailNotValid")]
            public string Email { get; set; }
            [RegularExpression(@"(http://|https://)?(www\.)?\w+\.(com|net|edu|org)", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "HomePageNotValid")]
            public string HomePage { get; set; }
            
        }
    }
}


    //class GroupMetaData
    //{
    //    [RegularExpression("^[\u0621-\u064A\u0660-\u0669a-zA-Z-_//s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    //    public string Name { get; set; }
    //    [RegularExpression("^[\u0621-\u064A\u0660-\u0669a-zA-Z-_//s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    //    public string NameNormalized { get; set; }
    //    [RegularExpression("^[\u0621-\u064A\u0660-\u0669a-zA-Z-_//s]+$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "ThisFieldisRequired")]
    //    public string Description { get; set; }
    //    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "EmailNotValid")]
    //    public string Email { get; set; }
    //    [RegularExpression(@"(http://|https://)?(www\.)?\w+\.(com|net|edu|org)", ErrorMessageResourceType = typeof(Resources.ResourcesFiles.MapLabel), ErrorMessageResourceName = "HomePageNotValid")]
    //    public string HomePage { get; set; }
    //}
