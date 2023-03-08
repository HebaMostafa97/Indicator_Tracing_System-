using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ISDCore
{
    public class UserObject : ObjectBase
    {
        public UserObject()
        {
        }
        [Required, MaxLength((int)EnumDataTypesLength.ISD_Name,ErrorMessageResourceName = "ThisFieldisRequired")]
        public string Name { get; set; }
        [Required, MaxLength((int)EnumDataTypesLength.ISD_Name, ErrorMessageResourceName = "ThisFieldisRequired")]
        public string NameNormalized { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_Name)]
        public string JobTitle { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_Notes)]
        public string Notes { get; set; }
        [Required, MaxLength((int)EnumDataTypesLength.ISD_Email, ErrorMessageResourceName = "ThisFieldisRequired"), Index(IsUnique = true)]
        public string Email { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_Email)]
        public string HomePage { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_Attachment)]
        public string Photo { get; set; }
        [Required, MaxLength((int)EnumDataTypesLength.ISD_ObjectName, ErrorMessageResourceName = "ThisFieldisRequired"), Index(IsUnique = true)]
        public string Username { get; set; }
        [Required, MaxLength((int)EnumDataTypesLength.ISD_Password, ErrorMessageResourceName = "ThisFieldisRequired")]
        public Byte[] Password { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_Password)]
        public Byte[] TempPassword { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? LastLogonDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? LastLogoutDate { get; set; }
        public int? LogInCount { get; set; }
        public int? Admin { get; set; }
    }
}
