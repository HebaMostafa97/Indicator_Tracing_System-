using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISDCore
{
    public class GroupObject : ObjectBase
    {
        [Required, MaxLength((int)EnumDataTypesLength.ISD_Name,ErrorMessageResourceName ="ThisFieldisRequired")]
        public string Name { get; set; }
        [Required, MaxLength((int)EnumDataTypesLength.ISD_Name,ErrorMessageResourceName ="ThisFieldisRequired")]
        public string NameNormalized { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_Description)]
        public string Description { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_Email)]
        public string Email { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_Url)]
        public string HomePage { get; set; }
    }
}
