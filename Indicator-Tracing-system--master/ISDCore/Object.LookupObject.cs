using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISDCore
{
    public class LookupObject : ObjectBase
    {
        [Required, MaxLength((int)EnumDataTypesLength.ISD_Name)]
        public string Name_A { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_Name)]
        public string Name_E { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_Description)]
        public string Description_A { get; set; }
        [MaxLength((int)EnumDataTypesLength.ISD_Description)]
        public string Description_E { get; set; }
        // default value : 0    
        public int IsDefault { get; set; }

    }
}
