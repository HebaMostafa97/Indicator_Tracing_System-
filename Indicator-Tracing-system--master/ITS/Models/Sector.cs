using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ISDCore;
namespace ITS.Models
{
    [Table("ITS_LKP_Sector")]
    public class Sector : LookupObject
    {
        public Sector()
        {
            Indicators = new HashSet<Indicator>();
            Sectors = new HashSet<Sector>();
        }

        public int? ParentID { get; set; }
        


        public virtual User Owner { set; get; }
        public virtual User Modifier { set; get; }
        public virtual Sector _Sector { set; get; }

        public virtual ICollection<Indicator> Indicators { set; get; }
        public virtual ICollection<Sector> Sectors { set; get; }
    }
}