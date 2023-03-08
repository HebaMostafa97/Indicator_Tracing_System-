using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISDCore
{
    public class PermissionGroupObject :ObjectBase
    {
        [Required, MaxLength((int)EnumDataTypesLength.ISD_Name)]
        public string FunctionName { get; set; }
        
        public int GroupID { set; get; }
        public int Permission { get; set; }
        public int Restricted { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_ObjectName)]
        public string Objectname { set; get; }
    }
}
